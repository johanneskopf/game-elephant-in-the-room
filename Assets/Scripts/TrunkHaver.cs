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
    [Range(0f, 1f)] public float trunkDamping = 1f;
    [Range(-1f, 0f)] public float gravityScale = -1f;

    public Rigidbody2D attachPoint;

    private BoxCollider2D[] _trunkElements;
    private GameObject _trunkTip;
    private Vector3 _relativeNeutralPosition;
    private Vector2 _trunkMovementInput = Vector2.zero;
    private bool grabbing = false;
    private Rigidbody2D grabbedObject = null;

    public GameObject debugSphere;

    public void Start()
    {
        _trunkElements = GetComponentsInChildren<BoxCollider2D>().Where(x => x.gameObject != gameObject)
            .ToArray();
        Debug.Log("Found " + _trunkElements.Length + " trunk elements");
        BoxCollider2D prev = null;
        foreach (var boxCollider2D in _trunkElements)
        {
            Debug.Log("Adding hinge joint to " + boxCollider2D.gameObject.name);
            var hinge = boxCollider2D.gameObject.AddComponent<HingeJoint2D>();
            hinge.anchor = new Vector2(boxCollider2D.size.x / 2f, 0f);
            if (prev != null)
            {
                hinge.connectedBody = prev.attachedRigidbody;
            }
            else
            {
                hinge.connectedBody = attachPoint;
                hinge.useLimits = true;
            }

            hinge.useLimits = true;

            var limits = hinge.limits;
            limits.min = hinge.jointAngle - trunkFlexibility;
            limits.max = hinge.jointAngle + trunkFlexibility;
            hinge.limits = limits;


            boxCollider2D.attachedRigidbody.gravityScale = 0f;

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
        var colliderSpaceOffset = new Vector3(colliderSpaceSize.x / 2f, colliderSpaceSize.y, colliderSpaceSize.z / 2f);
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

        var worldSpaceVectorMoveDir =
            transform.TransformVector(relativeTargetPos - RelativeCurrentTrunkEndPos());
        var worldSpaceVelocity = _trunkElements.Last().attachedRigidbody.velocity;
        var worldSpaceForce = (worldSpaceVectorMoveDir - (Vector3)worldSpaceVelocity * trunkDamping) * trunkStrength;
        _trunkElements.Last().attachedRigidbody.AddForce(worldSpaceForce);

        Debug.DrawLine(transform.TransformPoint(RelativeCurrentTrunkEndPos()),
            transform.TransformPoint(relativeTargetPos), Color.red);

        if (debugSphere != null)
        {
            debugSphere.transform.position = transform.TransformPoint(RelativeCurrentTrunkEndPos());
        }
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
        if (grabbedObject == null && grabbing && collision.otherRigidbody.gameObject.CompareTag("Grabbable") &&
            collision.otherRigidbody.gameObject == _trunkTip)
        {
            Grab(collision.otherRigidbody);
        }
    }
}