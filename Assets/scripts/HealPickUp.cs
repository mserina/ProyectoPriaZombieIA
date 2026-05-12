using UnityEngine;

public class HealPickup : MonoBehaviour
{
    [Header("Rotación")]
    public float rotationSpeed = 90f;
    [Header("Curación")]
    public int healAmount = 1;

    void Update()
    {
        // Gira como coleccionable
        transform.Rotate(new Vector3(0, rotationSpeed, rotationSpeed * 0.5f) * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance.currentLives >= GameManager.Instance.maxLives) return;

        GameManager.Instance.Heal(healAmount);
        gameObject.SetActive(false);
    }
}