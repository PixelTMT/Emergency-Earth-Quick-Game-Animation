using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameFinishUI;
    public bool isGameOver = false;
    public void Restart1()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameFinish();
        }
    }
    public void gameFinish()
    {
        isGameOver = true;
        gameFinishUI.SetActive(true);
    }
    public void gameOver()
    {
        isGameOver = true;
    }
}
