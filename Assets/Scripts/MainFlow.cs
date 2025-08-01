using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainFlow: MonoBehaviour {
    public Hoop HoopRef;
    public GameObject Player;
    public GameObject StartGameBtn;
    public GameObject MenuDancingMan;
    public GameObject GameOverCanvas;
    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI ScoreText;
    public bool GameRunning;
    public int Score = 0;
    private int _highScore = 0;
    private float _startTime;
    private float _gracePeriod = 5f;


    // Start is called before the first frame update
    void Start() {
        Player.SetActive(false);
        HoopRef.gameObject.SetActive(false);
        GameOverCanvas.SetActive(false);

        _highScore = PlayerPrefs.GetInt("highscore", _highScore);
        HighScoreText.text = $"{_highScore}";
    }

    public void StartGame() {
        Score = 0;
        HoopRef.PerfectStreak = 0;
        _startTime = Time.time;
        HoopRef.RestartHoop();

        StartGameBtn.SetActive(false);
        MenuDancingMan.SetActive(false);
        GameOverCanvas.SetActive(false);

        Player.SetActive(true);
        HoopRef.gameObject.SetActive(true);

        GameRunning = true;
        HoopRef.SpinHoop(false);
    }

    // Update is called once per frame
    void Update() {
        if (!GameRunning) return;
        if (Input.GetKey("left")) {
            HoopRef.SpinHoop(false);
        }

        if (Input.GetKey("right")) {
            HoopRef.SpinHoop(true);
        }
        HoopRef.HandleHoopGravity(Time.time - _startTime < _gracePeriod);

        Score = (int)(Time.time - _startTime);
        ScoreText.text = $"{Score}";
    }

    public void HandleGameOver() {
        Debug.Log($"!@# Score: {Score}, High Score: {_highScore}");
        GameRunning = false;
        if (Score > _highScore) {
            _highScore = Score;
            HighScoreText.text = $"{_highScore}";

            PlayerPrefs.SetInt("highscore", _highScore);
        }
        GameOverCanvas.SetActive(true);
    }
}
