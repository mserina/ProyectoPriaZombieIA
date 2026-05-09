using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Rotación")]
    public float rotationSpeed = 90f;

    [Header("Stun")]
    public float stunDuration = 5f;

    void Update()
    {
        // Gira como coleccionable
        transform.Rotate(new Vector3(0, rotationSpeed, rotationSpeed * 0.5f) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivarBomba();
            Destroy(gameObject);
        }
    }

    void ActivarBomba()
    {
        ZombieJump[] zombiesNormales = FindObjectsByType<ZombieJump>(FindObjectsSortMode.None);
        Debug.Log($"Zombies normales encontrados: {zombiesNormales.Length}");
        foreach (ZombieJump zombie in zombiesNormales)
        {
            Debug.Log($"Llamando Stun en: {zombie.gameObject.name}");
            zombie.Stun(stunDuration);
        }

        ZombieGordo[] zombiesGordos = FindObjectsByType<ZombieGordo>(FindObjectsSortMode.None);
        Debug.Log($"Zombies gordos encontrados: {zombiesGordos.Length}");
        foreach (ZombieGordo zombie in zombiesGordos)
        {
            zombie.Stun(stunDuration);
        }
    }
}