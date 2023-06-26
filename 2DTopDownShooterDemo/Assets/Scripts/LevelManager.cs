using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public PlayerController player;
    public float gameTime { 
        get { return timer; } 
        set { timer = value; }
    }
    private float timer;
    public GameUIController MainUI;

    
    void Start()
    {
        gameTime = 300f;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;
            CompleteLevel();
        }

        //检测Esc键是否按下
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //显示Canvas并暂停游戏
            MainUI.ShowGamePauseScreen();
            Time.timeScale = 0;
        }
    }

    private void CompleteLevel()
    {
        MainUI.ShowGameCompleteScreen();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        MainUI.ResetUI();
    }

    public void GameOver()
    {
        MainUI.ShowGameOverScreen();
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        MainUI.ResetUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
