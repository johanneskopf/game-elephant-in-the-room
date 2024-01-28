using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Controls
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _movementSpeedModifier = 2f;

        private Vector2 _movementInput = Vector2.zero;
        private bool _isJumping = false;
        public int touchingGround = 0;

        public TrunkHaver trunkHaver;
        public float jumpStrength = 50f;

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

            if (trunkHaver != null)
            {
                if (_movementInput.x > 0.5)
                    trunkHaver.FaceRight(true);

                if (_movementInput.x < -0.5)
                    trunkHaver.FaceRight(false);
            }

            if (_isJumping)
            {
                var rb = GetComponent<Rigidbody2D>();
                if (touchingGround > 0)
                {
                    rb.velocityY = 0;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
                }

                _isJumping = false;
            }
        }

        public void RestartLevel(InputAction.CallbackContext ctx)
        {
            var restart = ctx.ReadValueAsButton();
            if (restart)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        
        public void Trumpet(InputAction.CallbackContext ctx)
        {
            var trumpet = ctx.ReadValueAsButton();
            if (trumpet)
            {
                AudioManager.Instance.PlayElephantTrumpet();
            }
        }
    }
}