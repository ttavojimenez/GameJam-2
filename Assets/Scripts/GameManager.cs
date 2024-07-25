using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isPaused = false;
    private AnimalController[] animals;
    private bool allAnimalsHaveLife100 = false;


    private void Start()
    {
        animals = FindObjectsOfType<AnimalController>();
    }

    void Update()
    {
        allAnimalsHaveLife100 = true;

        foreach (AnimalController animal in animals)
        {
            if (animal.GetLife() < 100)
            {
                allAnimalsHaveLife100 = false;
                break;
            }
        }

        if (allAnimalsHaveLife100)
        {
            SceneManager.LoadScene(2);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;

        // Ocultar el cursor
        Cursor.visible = false;
        // Bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }
}