using UnityEngine;

public class Arma : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (other.CompareTag("Player"))
        {
            player.PickWeapon(); // dar arma al jugador
            Destroy(gameObject); // desaparecer del mundo
        }
    }
}