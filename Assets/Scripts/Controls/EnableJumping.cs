using UnityEngine;

namespace Controls
{
    public class EnableJumping : MonoBehaviour
    {
        public PlayerController playerController;

        private void OnTriggerEnter2D(Collider2D other)
        {
            playerController.touchingGround++;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            playerController.touchingGround--;
            if (playerController.touchingGround < 0)
            {
                Debug.LogError("Somehow touching negative ground. Fixing...");
                playerController.touchingGround = 0;
            }
        }
    }
}