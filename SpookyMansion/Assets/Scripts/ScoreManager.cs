using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private int score = 0;

    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        //Singleton code
        if (Instance != null && Instance != this) Destroy(this); else Instance = this;
    }

    public void IncreaseScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "SCORE: " + score.ToString();
    }
}
