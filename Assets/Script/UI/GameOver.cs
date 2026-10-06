using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOver;
    private bool isGameOver = false;

    private void Start()
    {
        gameOver.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowGameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        gameOver.SetActive(true);
    }

    public void ReturnToStart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("StartSence");
    }
}