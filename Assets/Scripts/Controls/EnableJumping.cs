using UnityEngine;

namespace Controls
{
    public class EnableJumping : MonoBehaviour
    {
        public PlayerController playerController;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (playerController != null)
                playerController.EnableJumpingAgain();
        }
    }
}