using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour {
    public bool PlayerFacingRight;
    // public bool PlayerLastFacingRight;
    // public bool PlayerMovedToRight { get { return PlayerFacingRight && !PlayerLastFacingRight; } }
    public Animator PlayerAnimatorRef;

    public bool MovePlayer(bool right) {
        // if (right && PlayerFacingRight) 
        bool PlayerFacingChanged = false;
        if (right && !PlayerFacingRight) {
            PlayerFacingChanged = true;
            PlayerFacingRight = true;
            PlayerAnimatorRef.ResetTrigger("moveLeft");
            PlayerAnimatorRef.SetTrigger("moveRight");
        }
        if (!right && PlayerFacingRight) {
            PlayerFacingChanged = true;
            PlayerFacingRight = false;
            PlayerAnimatorRef.ResetTrigger("moveRight");
            PlayerAnimatorRef.SetTrigger("moveLeft");
        }
        return PlayerFacingChanged;
    }

}
