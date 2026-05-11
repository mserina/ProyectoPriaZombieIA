using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Vida")]
    public int maxLives = 5;
    public int currentLives;

    [Header("UI")]
    public Slider lifeSlider;

    private bool gameEnded = false;

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
        if (gameEnded) return;

        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        lifeSlider.value = currentLives;

        if (currentLives <= 0)
            GameOver();
    }

    public void CheckWinCondition()
    {
        if (gameEnded) return;

        // Busca todos los enemigos activos en escena
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
        Cursor.lockState = CursorLockMode.None; // libera el cursor
        Cursor.visible = true;
        
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        
        Debug.Log("Escena " +  currentScene);

        
        if (currentScene == 1)       // Nivel 1
            SceneManager.LoadScene(3); // Ganar Nivel 1
        else if (currentScene == 4)  // Nivel 2
            SceneManager.LoadScene(6); // Ganar Nivel 2
        else if (currentScene == 5)  //Nivel 3
            SceneManager.LoadScene(7); //Victoria
        
        Debug.Log("¡HAS GANADO!");
        // aquí puedes cargar escena de victoria, mostrar UI, etc.
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