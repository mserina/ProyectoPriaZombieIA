using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PowerUpAtraccion : MonoBehaviour
{
    [Header("Atracción")]
    public float duracion = 5f;
    public float radioAtraccion = 30f;

    [Header("Sonido")]
    public AudioSource audioSource;
    
    
    // El jugador lo recoge
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null)
                audioSource.Play();

            StartCoroutine(Atraer(other.transform));
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    IEnumerator Atraer(Transform jugador)
    {
        Vector3 puntoAtraccion = jugador.position;
        EnemyMovement[] zombies = FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None);

        foreach (EnemyMovement zombie in zombies)
        {
            if (zombie == null) continue;
            NavMeshAgent agent = zombie.GetComponent<NavMeshAgent>();
            if (agent != null && agent.isOnNavMesh)
                agent.SetDestination(puntoAtraccion);
            zombie.enabled = false;
        }

        yield return new WaitForSeconds(duracion);

        foreach (EnemyMovement zombie in zombies)
        {
            if (zombie == null) continue;
            zombie.enabled = true;
        }

        // Espera a que termine el sonido antes de destruir
        if (audioSource != null)
            yield return new WaitForSeconds(audioSource.clip.length);

        Destroy(gameObject);
    }
}