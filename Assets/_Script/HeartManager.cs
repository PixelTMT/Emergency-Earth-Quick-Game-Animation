using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    [SerializeField] GameObject GameOverUI;
    [SerializeField] int heart = 5;
    [SerializeField] Texture Heart;
    [SerializeField] Texture EmptyHeart;

    GameManager gameManager;
    private void Awake()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        GameOverUI.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (gameManager == null) return;


        if (heart <= 0)
        {
            GameOverUI.SetActive(true);
            gameManager.gameOver();
        }
    }
    void updateHeartUI()
    {
        if (gameManager.isGameOver) return;
        heart = Mathf.Clamp(heart, 0, 5);
        for (int i = 0; i < heart; i++)
        {
            transform.GetChild(i).GetComponent<RawImage>().texture = Heart;
        }
        for (int i = heart; i < 5; i++)
        {
            transform.GetChild(i).GetComponent<RawImage>().texture = EmptyHeart;
        }
    }
    public void ReduceHeart(int amount)
    {
        heart -= amount;
        updateHeartUI();
    }
    public void AddHeart(int amount)
    {
        heart += amount;
        updateHeartUI();
    }
}
