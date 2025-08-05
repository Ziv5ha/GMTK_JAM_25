using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Hoop: MonoBehaviour {
    public MainFlow MainFlowRef;
    public SpriteRenderer HoopFrontSpriteRenderer;
    public SpriteRenderer HoopBackSpriteRenderer;
    public Player PlayerRef;
    public Rigidbody2D Rigidbody2DRef;
    public Animator PlayerAnimatorRef;
    public TextMeshProUGUI PerfectStreakText;
    public MusicManager MusicManagerRef;
    public SoundManager SoundManagerRef;

    private float _timeToStartGravityOnEdgeTouch = .3f;
    private float _gracePeriodTimeToStartGravityOnEdgeTouch = .5f;
    private float _timeToStartGravityAfterPerfectSpin = .1f;
    private int _spinForce = 100;
    private int _upforce = 10;
    private bool _hoopIsOnTheRight = true;
    private bool _enableHoopSpin = true;
    private float _lastHoopEdgeTouchTime;
    public int PerfectStreak = 0;
    public int accumulatedSuccessCounter = 0;
    private float _perfectSpinTimeWindow = .2f;
    private float _perfectSpinTimeWindowNarrower = .01f;
    private int _perfectStreakSpinForceMultiplier = 10;
    private float _perfectStreakUpForceMultiplier = .1f;

    public void RestartHoop() {
        transform.position = new Vector3(0, -1.19f, -0.16f);
        transform.rotation = Quaternion.identity;
        Rigidbody2DRef.velocity = new Vector2(-10, 0);
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
            accumulatedSuccessCounter++;
            if (SoundManagerRef != null) {
                SoundManagerRef.PlayRandomHit();
            }
        } else {
            PerfectStreak = 0;
            accumulatedSuccessCounter--;
        }
        PerfectStreakText.text = $"{PerfectStreak}";

        // Update adaptive music 🎵
        if (MusicManagerRef != null) {
            // each threshold adds a new layer on top of the previous ones.
            // 0-5: layer 0, 6-10: layers 0+1, 11-15: layers 0+1+2, etc.
            int maxEnabledLayer = Mathf.Clamp(accumulatedSuccessCounter / 6, 0, MusicManagerRef.musicLayers.Length - 1);
            for (int i = 0; i < MusicManagerRef.musicLayers.Length; i++) {
                float targetVolume = (i <= maxEnabledLayer) ? 1f : 0f;
                MusicManagerRef.FadeLayerVolume(i, targetVolume, 1.0f);
            }


        }
        HoopFrontSpriteRenderer.color = new Color(0, 0, 0);
        HoopBackSpriteRenderer.color = new Color(0, 0, 0);
    }
    private void OnCollisionStay2D(Collision2D other) {
        float totalPerfectSpinTimeWindow = _perfectSpinTimeWindow - (_perfectSpinTimeWindowNarrower * PerfectStreak);
        if (MainFlowRef.PerfectAssist && Time.time - _lastHoopEdgeTouchTime < totalPerfectSpinTimeWindow) {
            HoopFrontSpriteRenderer.color = new Color(0, 255, 0);
            HoopBackSpriteRenderer.color = new Color(0, 255, 0);
        } else {
            HoopFrontSpriteRenderer.color = new Color(0, 0, 0);
            HoopBackSpriteRenderer.color = new Color(0, 0, 0);
        }
    }

    public void SpinHoop(bool right, bool playerMoved) {
        float totalSpinForce = _spinForce + (_perfectStreakSpinForceMultiplier * PerfectStreak) + 5;
        float totalUpForce = (_upforce + (_perfectStreakUpForceMultiplier * PerfectStreak)) * (transform.position.y > -0.5 ? 0 : 1);

        if (!_enableHoopSpin || !playerMoved) return;
        if (!right && _hoopIsOnTheRight && PlayerRef.PlayerFacingRight) {
            _hoopIsOnTheRight = false;
            Rigidbody2DRef.AddForce(new Vector2(-totalSpinForce, totalUpForce));
        }
        if (right && !_hoopIsOnTheRight && !PlayerRef.PlayerFacingRight) {
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
