using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TrunkHaver : MonoBehaviour
{
    public float trunkFlexibility = 10f;
    public float moveRadius = 1f;
    [FormerlySerializedAs("offset")] public Vector2 relativeStartOffset = Vector2.zero;
    public float trunkStrength = 1f;
    [Range(0f, 1f)] public float trunkDamping = 1f;
    [Range(-1f, 0f)]
    public float gravityScale = -1f;

    private BoxCollider2D[] _trunkElements;
    private Vector3 _relativeNeutralPosition;
    private Vector2 _trunkMovementInput = Vector2.zero;

    public GameObject debugSphere;

    public void Start()
    {
        _trunkElements = GetComponentsInChildren<BoxCollider2D>().Where(x => x.gameObject != gameObject)
            .ToArray();
        Debug.Log("Found " + _trunkElements.Length + " trunk elements");
        BoxCollider2D prev = null;
        foreach (var boxCollider2D in _trunkElements)
        {
            var hinge = boxCollider2D.gameObject.AddComponent<HingeJoint2D>();
            hinge.anchor = new Vector2(boxCollider2D.size.x / 2f, 0f);
            if (prev != null)
            {
                hinge.connectedBody = prev.attachedRigidbody;
                hinge.useLimits = true;

                var limits = hinge.limits;
                limits.min = hinge.jointAngle - trunkFlexibility;
                limits.max = hinge.jointAngle + trunkFlexibility;
                hinge.limits = limits;
            }
            else
            {
                hinge.connectedBody = GetComponent<Rigidbody2D>();
                hinge.useLimits = true;

                var limits = hinge.limits;
                limits.min = hinge.jointAngle - trunkFlexibility * 2;
                limits.max = hinge.jointAngle + trunkFlexibility * 2;
                hinge.limits = limits;
            }

            boxCollider2D.attachedRigidbody.gravityScale = 0f;

            prev = boxCollider2D;
        }

        _relativeNeutralPosition =
            RelativeCurrentTrunkEndPos() + transform.InverseTransformDirection(relativeStartOffset);
    }

    public void OnTrunkMovement(InputAction.CallbackContext ctx)
    {
        _trunkMovementInput = ctx.ReadValue<Vector2>();
    }

    private Vector3 RelativeCurrentTrunkEndPos()
    {
        var lastTrunkElement = _trunkElements.Last();
        var colliderSpaceSize = new Vector3(lastTrunkElement.size.x, lastTrunkElement.size.y, lastTrunkElement.size.y);
        var colliderSpaceOffset = new Vector3(colliderSpaceSize.x / 2f, colliderSpaceSize.y, colliderSpaceSize.z / 2f);
        var worldSpacePos = lastTrunkElement.transform.TransformPoint(colliderSpaceOffset);

        return transform.InverseTransformPoint(worldSpacePos);
    }

    public void Update()
    {
        var relativeTargetPos = _relativeNeutralPosition;
        relativeTargetPos += transform.InverseTransformDirection(_trunkMovementInput) * moveRadius;

        SetGravity(_trunkMovementInput.y * -gravityScale);

        var worldSpaceVectorMoveDir =
            transform.TransformVector(relativeTargetPos - RelativeCurrentTrunkEndPos()); // wenn ein fehler is dann hier
        var worldSpaceVelocity = _trunkElements.Last().attachedRigidbody.velocity;
        var worldSpaceForce = (worldSpaceVectorMoveDir - (Vector3)worldSpaceVelocity * trunkDamping) * trunkStrength;
        _trunkElements.Last().attachedRigidbody.AddForce(worldSpaceForce);

        Debug.DrawLine(transform.TransformPoint(RelativeCurrentTrunkEndPos()),
            transform.TransformPoint(_relativeNeutralPosition), Color.red);

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
}