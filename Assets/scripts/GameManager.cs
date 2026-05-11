using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Vida")]
    public int maxLives = 5;
    public int currentLives;

    [Header("UI Corazones")]
    public RawImage[] corazones; // arrastra aquí los 5 corazones en orden

    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentLives = maxLives;
        ActualizarCorazones();
    }

    public void TakeDamage(int damage)
    {
        if (gameEnded) return;

        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        ActualizarCorazones();

        if (currentLives <= 0)
            GameOver();
    }

    void ActualizarCorazones()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].enabled = i < currentLives;
        }
    }

    public void CheckWinCondition()
    {
        if (gameEnded) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
            Win();
    }

    public void TogglePause()
    {
        if (gameEnded) return;

        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            Debug.Log("JUEGO EN PAUSA");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("JUEGO REANUDADO");
        }
    }

    void Win()
    {
        gameEnded = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene == 1)
            SceneManager.LoadScene(3);
        else if (currentScene == 4)
            SceneManager.LoadScene(6);
        else if (currentScene == 5)
            SceneManager.LoadScene(7);

        Debug.Log("¡HAS GANADO!");
    }

    public void GameOver()
    {
        gameEnded = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        string escenaActual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("GameOver " + escenaActual);

        Debug.Log("GAME OVER - escena: " + escenaActual);
    }
}