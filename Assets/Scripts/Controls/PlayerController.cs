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
        var myRb = GetComponent<Rigidbody2D>();
        var targetVelocity = _movementInput.x * _movementSpeedModifier;

        myRb.AddForceX(targetVelocity - myRb.velocityX, ForceMode2D.Impulse);

        if (_isJumping)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 40f), ForceMode2D.Impulse);
            _isJumping = false;
        }
    }
}