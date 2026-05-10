using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsCanva : MonoBehaviour
{
    // Asigna el nombre de la escena en el Inspector
    public string sceneToLoad;

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
}