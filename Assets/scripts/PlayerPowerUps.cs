using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PlayerPowerUps : MonoBehaviour
{
    public GameObject esferaEscudo;

    public void ActivarEscudo(float duracion)
    {
        StartCoroutine(EscudoRoutine(duracion));
    }

    IEnumerator EscudoRoutine(float duracion)
    {
        NavMeshObstacle obstaculo = GetComponentInChildren<NavMeshObstacle>();
        if (obstaculo != null) obstaculo.enabled = true;
        if (esferaEscudo != null) esferaEscudo.SetActive(true);

        yield return new WaitForSeconds(duracion);

        if (obstaculo != null) obstaculo.enabled = false;
        if (esferaEscudo != null) esferaEscudo.SetActive(false);
    }
}