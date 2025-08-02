using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainFlow: MonoBehaviour {
    public Hoop HoopRef;
    public GameObject Player;
    public GameObject MenuDancingMan;
    public GameObject GameOverCanvas;
    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI StreakText;
    public bool GameRunning;
    public int Score = 0;
    private float _startTime;
    private float _gracePeriod = 1f;


    // Start is called before the first frame update
    void Start() {
        GameRunning = true;
        HoopRef.SpinHoop(false);
        _startTime = Time.time;
        Score = 0;
        HoopRef.PerfectStreak = 0;
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
    }

    public void HandleGameOver() {
        GameRunning = false;
    }

    public void HandleScore() {

    }

    public void RestartGame() {

    }
}
