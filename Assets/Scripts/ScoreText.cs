using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private int score;
    public TextMeshProUGUI _scoreText;

    private void Start()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
        GameManager.OnCubeSpawned += GameManager_OnCubeSpawned;
    }

    private void OnDestroy()
    {
        GameManager.OnCubeSpawned -= GameManager_OnCubeSpawned;
    }

    // private void Update()
    // {
    //     score++;
    //     _scoreText.text = "Score: " + score.ToString();
    //     _scoreText.SetText("Score: " + score.ToString());
    // }

    private void GameManager_OnCubeSpawned()
    {
        score++;
        _scoreText.text = "Score: " + score.ToString();
        _scoreText.SetText("Score: " + score.ToString());
    }
}