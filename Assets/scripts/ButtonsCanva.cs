using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsCanva : MonoBehaviour
{
    // Asigna el nombre de la escena en el Inspector
    public string sceneToLoad;
    public string sceneToRetry;


    public void Volver()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void SiguienteNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }
    
    public void Reintentar()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(sceneToRetry);
    }
}