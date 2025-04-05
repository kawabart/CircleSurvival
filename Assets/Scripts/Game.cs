using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private Text scoreUI;
    [SerializeField] private Text highscoreUI;
    [SerializeField] private GameObject lossPanel;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject newBestPanel;
    private static event Action<Vector3> GameEnding;
    private static event Action GameEnded;
    
    public int Score
    {
        get => score;

        set
        {
            score = value;
            if (scoreUI != null)
                scoreUI.text = "Score: " + score;
        }
    }

    private int score;
    private int highscore;

    private void Update()
    {
        Score = (int)Time.timeSinceLevelLoad;
    }
    
    private void Start()
    {
        SetHighscoreUI();
        
        if (SceneManager.GetActiveScene().buildIndex == (int)Scenes.Gameplay)
        {
            GameEnding += SetExplosionEffect;
            GameEnded += ShowLossPanel;
            GameEnded += UpdateHighscore;
            GameEnded += WaitAfterLoss;
        }
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene((int)Scenes.Gameplay);
        Time.timeScale = 1;
    }

    public void SetExplosionEffect(Vector3 position)
    {
        explosionEffect.transform.position = position;
        explosionEffect.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-32f, 32f));
    }

    private void ShowLossPanel()
    {
        lossPanel.SetActive(true);
    }

    public static void EndGame(Vector3 position)
    {
        GameEnding.Invoke(position);
        GameEnded.Invoke();
        Time.timeScale = 0;
        Handheld.Vibrate();
    }

    private void WaitAfterLoss()
    {
        StartCoroutine(DelayEndGame());
    }

    private IEnumerator DelayEndGame()
    {
        yield return new WaitForSecondsRealtime(1);
        yield return new WaitUntil(() => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0));

        GameEnding -= SetExplosionEffect;
        GameEnded -= ShowLossPanel;
        GameEnded -= UpdateHighscore;
        GameEnded -= WaitAfterLoss;
        SceneManager.LoadScene((int)Scenes.Menu);
    }

    private void SetHighscoreUI()
    {
        highscore = PlayerPrefs.GetInt(Prefs.Highscore.ToString(), 0);
        highscoreUI.text = "" + highscore;
    }

    private void UpdateHighscore()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt(Prefs.Highscore.ToString(), score);
            PlayerPrefs.Save();
            highscoreUI.text = "" + highscore;
            newBestPanel.SetActive(true);
        }
    }
}
