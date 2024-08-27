using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public bool switchMode;
    public bool jump;
    public Joystick joystick;
    public bool listenMovement = true;


    void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }
    void OnSwitchMode(InputValue value)
    {

        switchMode = value.isPressed;
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jump = true;
        }
    }
    void OnJumpRelease(InputValue value)
    {
        if (value.isPressed)
        {
            jump = false;
        }
    }
    void Update()
    {
        if (listenMovement)
            move = joystick.Direction;
    }
}
