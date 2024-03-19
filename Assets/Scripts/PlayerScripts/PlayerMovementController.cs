using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementController : CharacterMovementControllerBase
{
    [Header("--Player Movement--")] [SerializeField]
    private FloatingJoystick joystick;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 moveVector;
    private bool isMoving;
    public Animator anim;

    private void Update()
    {
        JoystickCheck();
        Move();
    }

    public override void Move()
    {
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveVector, Time.deltaTime * 5f);
        }
    }


    private void JoystickCheck()
    {
        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * moveSpeed;
        moveVector.z = joystick.Vertical * moveSpeed;

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            isMoving = true;
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalk", true);
        }
        else if (joystick.Horizontal == 0 || joystick.Vertical == 0)
        {
            isMoving = false;
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalk", false);
        }
    }
}