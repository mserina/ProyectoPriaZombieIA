using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BotonPuente : MonoBehaviour
{
    private Animation animation;
    [SerializeField] private Animator puenteMover;
    [SerializeField] private NavMeshSurface navMeshSurfaceZombiePeque, navMeshSurfaceZombieGordo; // NavMeshSurfaces
    [SerializeField] private NavMeshObstacle navMeshObstacle; // el obstáculo del puente

    [SerializeField] private float tiempoAnimacion = 1f; //ajusta según la duración de tu animación

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        animation.enabled = true;
        puenteMover.enabled = true;
        StartCoroutine(ActualizarNavMesh());
    }

    IEnumerator ActualizarNavMesh()
    {
        // Espera a que el puente termine de desplegarse
        yield return new WaitForSeconds(tiempoAnimacion);

        // Desactiva el obstáculo para que los zombies puedan cruzar
        if (navMeshObstacle != null)
            navMeshObstacle.enabled = false;

        navMeshSurfaceZombiePeque.BuildNavMesh();
        navMeshSurfaceZombieGordo.BuildNavMesh();
    }
}