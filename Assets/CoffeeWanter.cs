using UnityEngine;

public class CoffeeWanter : MonoBehaviour
{
    public Disabler disabler;

    void OnTriggerEnter2D(Collider2D other)
    {
        var cc = other.gameObject.GetComponent<CoffeeCup>();
        if (cc != null && cc.filled)
        {
            Debug.Log("Coffee received");
            Destroy(other.gameObject);
            disabler.Disable();
        }
    }
}