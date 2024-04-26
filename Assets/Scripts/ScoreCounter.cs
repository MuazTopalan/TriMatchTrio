using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }
    private int _score;
    private int _scoreThreshold;

    public int Score
    {
        get => _score;
        
        set
        {
            if(_score == value) return;

            _score = value;

            scoreText.SetText($"Score = {_score} / {_scoreThreshold}");
        }
    }
    
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake() => Instance = this;

    private void Start()
    {
        _scoreThreshold = Board.Instance.scoreThreshold;
    }
}
