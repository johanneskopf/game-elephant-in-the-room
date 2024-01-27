using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TrunkHaver : MonoBehaviour, ICollider2DListener
{
    public float trunkFlexibility = 10f;
    public float moveRadius = 1f;
    [FormerlySerializedAs("offset")] public Vector2 relativeStartOffset = Vector2.zero;
    public float trunkStrength = 1f;
    public float trunkPower = 5f;
    [Range(0f, 1f)] public float trunkDamping = 1f;
    [Range(-1f, 0f)] public float gravityScale = -1f;

    public Rigidbody2D attachPoint;

    private BoxCollider2D[] _trunkElements;
    private GameObject _trunkTip;
    private Vector3 _relativeNeutralPosition;
    private Vector2 _trunkMovementInput = Vector2.zero;
    private bool grabbing = false;
    private Rigidbody2D grabbedObject = null;
    private bool facingRight = true;

    public void Start()
    {
        _trunkElements = attachPoint.GetComponentsInChildren<BoxCollider2D>().Where(x => x.gameObject != gameObject)
            .ToArray();
        Debug.Log("Found " + _trunkElements.Length + " trunk elements");
        BoxCollider2D prev = null;
        foreach (var boxCollider2D in _trunkElements)
        {
            Debug.Log("Adding hinge joint to " + boxCollider2D.gameObject.name);
            var hinge = boxCollider2D.gameObject.AddComponent<HingeJoint2D>();
            if (prev != null)
            {
                hinge.connectedBody = prev.attachedRigidbody;
            }
            else
            {
                hinge.connectedBody = attachPoint;
                hinge.useLimits = true;
            }

            hinge.anchor = new Vector2(0f, 0f);
            hinge.useLimits = true;

            var limits = hinge.limits;
            limits.min = hinge.jointAngle - trunkFlexibility;
            limits.max = hinge.jointAngle + trunkFlexibility;
            hinge.limits = limits;

            var rb = boxCollider2D.attachedRigidbody;
            rb.gravityScale = 0f;
            rb.mass = trunkPower;

            prev = boxCollider2D;
        }

        _trunkTip = _trunkElements.Last().gameObject;
        _trunkTip.AddComponent<Collider2DBridge>().Initialize(this);

        _relativeNeutralPosition =
            RelativeCurrentTrunkEndPos() + transform.InverseTransformDirection(relativeStartOffset);
    }

    public void OnTrunkMovement(InputAction.CallbackContext ctx)
    {
        _trunkMovementInput = ctx.ReadValue<Vector2>();
    }

    public void OnGrab(InputAction.CallbackContext ctx)
    {
        grabbing = ctx.ReadValueAsButton();
        if (!grabbing)
        {
            Ungrab();
        }
    }

    private void Ungrab()
    {
        if (grabbedObject != null)
        {
            Destroy(_trunkTip.GetComponent<FixedJoint2D>());
            grabbedObject = null;
        }
    }

    private void Grab(Rigidbody2D rb)
    {
        var j = _trunkTip.AddComponent<FixedJoint2D>();
        j.connectedBody = rb;
        grabbedObject = rb;
    }

    private Vector3 RelativeCurrentTrunkEndPos()
    {
        var lastTrunkElement = TrunkTip();
        var colliderSpaceSize = new Vector3(lastTrunkElement.size.x, lastTrunkElement.size.y, lastTrunkElement.size.y);
        var colliderSpaceOffset = new Vector3(colliderSpaceSize.x, 0f, colliderSpaceSize.z / 2f);
        var worldSpacePos = lastTrunkElement.transform.TransformPoint(colliderSpaceOffset);

        return transform.InverseTransformPoint(worldSpacePos);
    }

    private BoxCollider2D TrunkTip()
    {
        return _trunkElements.Last();
    }

    public void Update()
    {
        var relativeTargetPos = _relativeNeutralPosition;
        relativeTargetPos += transform.InverseTransformDirection(_trunkMovementInput) * moveRadius;

        SetGravity(_trunkMovementInput.y * -gravityScale);

        var worldSpaceVectorMoveDir = transform.TransformVector(relativeTargetPos - RelativeCurrentTrunkEndPos());
        var worldSpaceVelocity =
            _trunkElements.Last().attachedRigidbody.velocity - GetComponent<Rigidbody2D>().velocity;
        var worldSpaceForce = (worldSpaceVectorMoveDir - (Vector3)worldSpaceVelocity * trunkDamping) * trunkStrength;
        _trunkElements.Last().attachedRigidbody.AddForce(worldSpaceForce, ForceMode2D.Impulse);

        Debug.DrawLine(
            transform.TransformPoint(RelativeCurrentTrunkEndPos()),
            transform.TransformPoint(RelativeCurrentTrunkEndPos() + worldSpaceForce),
            Color.red);

        attachPoint.velocity = GetComponent<Rigidbody2D>().velocity;
    }

    private void SetGravity(float gs)
    {
        foreach (var boxCollider2D in _trunkElements)
        {
            boxCollider2D.attachedRigidbody.gravityScale = gs;
        }
    }

    public void OnCollisionEnter2DEvent(Collision2D collision)
    {
        if (grabbedObject == null && grabbing && collision.rigidbody.gameObject.CompareTag("Grabbable"))
        {
            Grab(collision.rigidbody);
        }
    }

    public void FaceRight(bool right)
    {
        if (facingRight != right)
        {
            Flip();
        }
    }

    private void Flip()
    {
        if (grabbedObject != null) return;

        gameObject.transform.Find("Model").transform.Rotate(0, 180, 0);

        var localScale = attachPoint.transform.localScale;
        localScale.x *= -1;
        attachPoint.transform.localScale = localScale;

        var localPos = attachPoint.transform.localPosition;
        localPos.x *= -1;
        attachPoint.transform.localPosition = localPos;

        _relativeNeutralPosition.x *= -1;

        facingRight = !facingRight;
    }
}