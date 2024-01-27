using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Min(0)] private float _movementSpeedModifier = 2f;

    private Vector2 _movementInput = Vector2.zero;
    private bool _isJumping = false;

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        _movementInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        _isJumping = ctx.ReadValueAsButton();
    }

    private void Update()
    {
        foreach (var rb in GetComponentsInChildren<Rigidbody2D>())
        {
            rb.velocityX = _movementInput.x * _movementSpeedModifier;
        }

        if (_isJumping)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
            _isJumping = false;
        }
    }
}