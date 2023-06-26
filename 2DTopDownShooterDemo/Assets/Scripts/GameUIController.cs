using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public TextMeshProUGUI playerSpeedText;
    public TextMeshProUGUI playerDamageText;
    public TextMeshProUGUI playerFireRateText;

    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerExpText;
    public TextMeshProUGUI playerLvText;
    public PlayerController player;
    public LevelManager levelManager;

    public GameObject gameOverUI;
    public GameObject gameCompleteUI;
    public GameObject gamePauseUI;

    void Update()
    {
        timerText.text = string.Format("{0:0}", levelManager.gameTime);

        playerSpeedText.text = $"SPEED: {player.speed}";
        playerDamageText.text = $"DAMAGE: {player.bulletDamage}";
        playerFireRateText.text = $"FIRE RATE: {player.fireRate}";

        playerHealthText.text = $"Health: {player.health}";
        playerExpText.text = $"Experience: {player.experience}/{player.experienceToNextLevel}";
        playerLvText.text = $"Level: {player.level}";
    }

    public void ResetUI()
    {
        gameOverUI.SetActive(false);
        gameCompleteUI.SetActive(false);
        gamePauseUI.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        gameOverUI.SetActive(true);
    }

    public void ShowGameCompleteScreen()
    {
        gameCompleteUI.SetActive(true);
    }

    public void ShowGamePauseScreen()
    {
        gamePauseUI.SetActive(true);
    }

    public void OnClickResume()
    {
        Debug.Log("Button clicked: Resume Game.");

        levelManager.ResumeGame();
    }

    public void OnClickRestart()
    {
        Debug.Log("Button clicked: Restart Game.");

        levelManager.RestartGame();
    }

    public void OnClickQuit()
    {
        Debug.Log("Button clicked: Quit Game.");

        Application.Quit();
    }
}