using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Vida")]
    public int maxLives = 5;
    public int currentLives;

    [Header("UI")]
    public Slider lifeSlider;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentLives = maxLives;

        lifeSlider.maxValue = maxLives;
        lifeSlider.value = currentLives;
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;

        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        lifeSlider.value = currentLives;

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        // aquí puedes pausar juego, reiniciar escena, etc.
    }
}