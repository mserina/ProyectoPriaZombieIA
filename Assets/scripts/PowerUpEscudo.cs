using UnityEngine;
using UnityEngine.AI;

public class PowerUpEscudo : MonoBehaviour
{
    [Header("Rotación")]
    public float rotationSpeed = 90f;

    [Header("Obstáculo")]
    public float duracion = 10f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPowerUps pp = other.GetComponent<PlayerPowerUps>();
            if (pp != null)
                pp.ActivarEscudo(duracion);

            Destroy(gameObject);
        }
    }
}