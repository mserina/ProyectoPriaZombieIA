using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieJump : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false; // para controlar tú el salto
    }

    void Update()
    {
        if (agent.isOnOffMeshLink)
        {
            StartCoroutine(Jump(agent.currentOffMeshLinkData));
        }
    }

    IEnumerator Jump(OffMeshLinkData link)
    {
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = link.endPos + Vector3.up * agent.baseOffset;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            agent.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        agent.CompleteOffMeshLink(); // marca como completado
    }
}
