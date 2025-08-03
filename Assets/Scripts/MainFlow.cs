using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainFlow: MonoBehaviour {
    public Hoop HoopRef;
    public Player PlayerRef;
    public GameObject StartGameBtn;
    public GameObject MenuDancingMan;
    public GameObject GameOverCanvas;
    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI AssistsText;
    public MusicManager MusicManagerRef;
    public bool GameRunning;
    public int Score = 0;
    private int _highScore = 0;
    private float _startTime;
    private float _gracePeriod = 5f;

    public bool Mute = false;
    public bool PerfectAssist = true;


    // Start is called before the first frame update
    void Start() {
        PlayerRef.gameObject.SetActive(false);
        HoopRef.gameObject.SetActive(false);
        GameOverCanvas.SetActive(false);

        _highScore = PlayerPrefs.GetInt("highscore", _highScore);
        HighScoreText.text = $"{_highScore}";
    }

    public void StartGame() {
        Score = 0;
        HoopRef.PerfectStreak = 0;
        HoopRef.accumulatedSuccessCounter = 0;
        MusicManagerRef.ResetMusic();
        _startTime = Time.time;
        HoopRef.RestartHoop();

        StartGameBtn.SetActive(false);
        MenuDancingMan.SetActive(false);
        GameOverCanvas.SetActive(false);

        PlayerRef.gameObject.SetActive(true);
        HoopRef.gameObject.SetActive(true);

        GameRunning = true;
        HoopRef.SpinHoop(false, true);
    }

    // Update is called once per frame
    void Update() {
        if (!GameRunning) return;
        if (Input.GetKey("left")) {
            bool playerMoved = PlayerRef.MovePlayer(true);
            HoopRef.SpinHoop(false, playerMoved);
        }

        if (Input.GetKey("right")) {
            bool playerMoved = PlayerRef.MovePlayer(false);
            HoopRef.SpinHoop(true, playerMoved);
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
    public void TogglePerfectAssist(bool toggle) {
        PerfectAssist = toggle;
        AssistsText.text = toggle ? "On" : "Off";
    }
}
