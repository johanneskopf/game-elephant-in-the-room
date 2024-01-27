using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Min(0)]
    private float _movementSpeedModifier = 2f;

    private Vector2 _movementInput = Vector2.zero;

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        _movementInput = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
    }
}