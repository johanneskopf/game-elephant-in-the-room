using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Min(0)]
    private float _movementSpeedModifier = 2f;
    [SerializeField, Min(0)]
    private float _trunkMovementSpeedModifier = 2f;

    private Vector2 _trunkMovementInput = Vector2.zero;
    private Vector2 _movementInput = Vector2.zero;

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        _movementInput = ctx.ReadValue<Vector2>();
    }

    public void OnTrunkMovement(InputAction.CallbackContext ctx)
    {
        _trunkMovementInput = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        transform.position += _movementInput.x * _movementSpeedModifier * Time.deltaTime * Vector3.right;
        transform.position += _trunkMovementInput.y * _trunkMovementSpeedModifier * Time.deltaTime * Vector3.up;
    }
}