using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hoop: MonoBehaviour {
    public MainFlow MainFlowRef;
    public Rigidbody2D Rigidbody2DRef;
    public Animator PlayerAnimatorRef;
    public TextMeshProUGUI PerfectStreakText;


    private float _timeToStartGravityOnEdgeTouch = .3f;
    private float _gracePeriodTimeToStartGravityOnEdgeTouch = .5f;
    private float _timeToStartGravityAfterPerfectSpin = .1f;
    private int _spinForce = 100;
    private int _upforce = 10;
    private bool _hoopIsOnTheRight = true;
    private bool _enableHoopSpin = true;
    private float _lastHoopEdgeTouchTime;
    public int PerfectStreak = 0;
    private float _perfectSpinTimeWindow = .2f;
    private float _perfectSpinTimeWindowNarrower = .01f;
    private int _perfectStreakSpinForceMultiplier = 10;
    private float _perfectStreakUpForceMultiplier = .1f;

    public void RestartHoop() {
        transform.position = new Vector3(0, -1.19f, -0.16f);
        _hoopIsOnTheRight = true;
        _enableHoopSpin = true;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!MainFlowRef.GameRunning) return;

        if (other.gameObject.name == "Floor") {
            MainFlowRef.HandleGameOver();
        }
        if (other.gameObject.name == "Player") {
            _enableHoopSpin = true;
            _lastHoopEdgeTouchTime = Time.time;
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        if (!MainFlowRef.GameRunning) return;
        _enableHoopSpin = false;
        float totalPerfectSpinTimeWindow = _perfectSpinTimeWindow - (_perfectSpinTimeWindowNarrower * PerfectStreak);
        if (Time.time - _lastHoopEdgeTouchTime < totalPerfectSpinTimeWindow) {
            PerfectStreak++;
        } else {
            PerfectStreak = 0;
        }
        PerfectStreakText.text = $"{PerfectStreak}";
    }

    public void SpinHoop(bool right) {
        float totalSpinForce = _spinForce + (_perfectStreakSpinForceMultiplier * PerfectStreak);
        float totalUpForce = (_upforce + (_perfectStreakUpForceMultiplier * PerfectStreak)) * (transform.position.y > -0.5 ? 0 : 1);

        if (!_enableHoopSpin) return;
        if (!right && _hoopIsOnTheRight) {
            PlayerAnimatorRef.ResetTrigger("moveRight");
            PlayerAnimatorRef.SetTrigger("moveLeft");
            _hoopIsOnTheRight = false;
            Rigidbody2DRef.AddForce(new Vector2(-totalSpinForce, totalUpForce));
        }
        if (right && !_hoopIsOnTheRight) {
            PlayerAnimatorRef.ResetTrigger("moveLeft");
            PlayerAnimatorRef.SetTrigger("moveRight");
            _hoopIsOnTheRight = true;
            Rigidbody2DRef.AddForce(new Vector2(totalSpinForce, totalUpForce));
        }
    }
    public void HandleHoopGravity(bool gracePeriod) {
        if (!MainFlowRef.GameRunning) return;
        float totalHoopHangTime = gracePeriod ? (_gracePeriodTimeToStartGravityOnEdgeTouch + (_timeToStartGravityAfterPerfectSpin * PerfectStreak)) : (_timeToStartGravityOnEdgeTouch + (_timeToStartGravityAfterPerfectSpin * PerfectStreak));
        if (Time.time - _lastHoopEdgeTouchTime < totalHoopHangTime) {
            Rigidbody2DRef.gravityScale = 0;
        } else {
            Rigidbody2DRef.gravityScale = 0.1f;
        }
    }
}
