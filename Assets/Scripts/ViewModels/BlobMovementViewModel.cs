using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class BlobMovementViewModel 
{
    public ReactiveProperty<bool> isWall;

    private CharacterController characterController;

    //Jump Variables
    private bool isGrounded => characterController.isGrounded;
    private float jumpHeight;
    private float jumpSpeed;
    private bool isJump = false;

    //Move Variables
    private float horizontal = 0;

    private float gravity = -9.81f;

    private Vector3 dir;

    private float startJumpHeight = 6f;
    private float moveSpeed;
    private float startSpeed;
    private float maxSpeed;
    private float minSpeed;
    private float startTemp;
    private float maxTemp;
    private float minTemp;

    public bool isTouchingWall;

    private float startSlideSpeed;
    private float slideSpeed = 0;
    private bool isSlide = false;

    private bool isAbove = false;

    private bool isWaterWall = false;
    private bool isWaterWallCollision = false;
    private int waterWallMovementDirection;
    private float lastJumpHeight;


    public BlobMovementViewModel(ReactiveProperty<float> blobTemp, ReactiveProperty<float> blobSize,CharacterController characterController)
    {
        isTouchingWall = false;

        this.characterController = characterController;
        characterController.slopeLimit = GlobalModel.Instance.SlopeLimit;

        startJumpHeight = GlobalModel.Instance.StartJumpHeight;
        jumpHeight = startJumpHeight;
        jumpSpeed = gravity * Time.deltaTime;

        maxSpeed = GlobalModel.Instance.MaxBlobSpeed;
        minSpeed = GlobalModel.Instance.MinBlobSpeed;
        startSpeed = GlobalModel.Instance.StartBlobSpeed;
        moveSpeed = startSpeed;
        maxTemp = GlobalModel.Instance.BoilingTemp;
        minTemp = GlobalModel.Instance.FreezingTemp;
        startTemp = GlobalModel.Instance.StartBlobTemp;

        startSlideSpeed = GlobalModel.Instance.SlideSpeed;


        GameInput.Instance.OnJump += GameInput_OnJump;
        GameInput.Instance.OnMove += GameInput_OnMove;

        blobTemp.Subscribe(blobTemp => ChangeMoveSpeed(blobTemp));
        blobSize.Subscribe(blobSize => ChangeJumpForce(blobSize));

        CheckpointManager.Instance.OnGoToLastCheckpoint += CheckpointManagerOnGoToLastCheckpoint;
        CheckpointManager.Instance.OnGoToCheckpointComplete += CheckpointManager_OnGoToCheckpointComplete; 
    }

    public void Update()
    {
        if (isGrounded)
        {
            jumpSpeed = 0;
            isAbove = true;
        }

        if ((characterController.collisionFlags & CollisionFlags.Above) != 0 && isAbove)
        {
            jumpSpeed = 0;
            isJump = false;
            isAbove = false;
        }

        if (isJump)
        {
            jumpSpeed += Mathf.Sqrt(jumpHeight * (-1) * gravity);
            isJump = false;
        }

        if (isGrounded && isWaterWall && waterWallMovementDirection * Math.Abs(horizontal) != horizontal * Math.Abs(waterWallMovementDirection))
        {
            isWaterWall = false;
        }

        if (lastJumpHeight > characterController.transform.position.y && 
            isWaterWallCollision && 
            waterWallMovementDirection * Math.Abs(horizontal) == horizontal * Math.Abs(waterWallMovementDirection) &&
            horizontal != 0)
        {
            WaterWallTouchAction();
            
        }

        if (!isWaterWall)
        {
            jumpSpeed += gravity * Time.deltaTime;
            lastJumpHeight = characterController.transform.position.y;
        }
        else
        {
            jumpSpeed = 0;
        }
        
        float movement;
        if (isSlide)
        {
            movement = slideSpeed * Time.deltaTime;
        }
        else
        {
            movement = horizontal * moveSpeed * Time.deltaTime;;
        }

        if (isWaterWall)
        {
            dir = new Vector3(0, waterWallMovementDirection *  (movement), 0);
        }
        else
        {
            dir = new Vector3(movement, jumpSpeed * Time.deltaTime, 0);
        }
        characterController.Move(dir);
    }

    private void CheckpointManagerOnGoToLastCheckpoint(object sender, EventArgs e)
    {
        characterController.enabled = false;
        moveSpeed = CheckpointManager.Instance.BlobMoveSpeedCheckpoint;

    }
    private void CheckpointManager_OnGoToCheckpointComplete(object sender, EventArgs e)
    {
        characterController.enabled = true;
    }

    private void ChangeMoveSpeed(float blobTemp)
    {
        if (blobTemp > startTemp)
        {
            moveSpeed = startSpeed + (blobTemp - startTemp) / ((maxTemp - startTemp) / (maxSpeed - startSpeed));
        }

        if (blobTemp < startTemp)
        {
            moveSpeed = startSpeed + (blobTemp - startTemp) / ((startTemp - minTemp) / (startSpeed - minSpeed));
        }
        if(blobTemp == startTemp)
        {
            moveSpeed = startSpeed;
        }
    }
    private void ChangeJumpForce(float blobSize)
    {
        jumpHeight =  startJumpHeight * blobSize;
    }

    private void GameInput_OnMove(object sender, GameInput.MovementVectorEventArgs e)
    {
        horizontal = e.movementVector.x;
        if (horizontal != 0)
        {
            isSlide = false;
        }
        else
        {
            isSlide = true;
        }

        characterController.transform.rotation = Quaternion.LookRotation(new Vector2(horizontal*(-1), 0));
    }

    private void GameInput_OnJump(object sender, System.EventArgs e)
    {
        if (slideSpeed == 0 && isGrounded || isWaterWall)
        {
            isJump = true;
            isWaterWall = false;
        }
    }

    public void DryAreaTouchActions(int moveDir)
    {
        slideSpeed = startSlideSpeed * moveDir;
    }

    public void DryAreaEndTouchActions()
    {
        slideSpeed = 0;
    }

    public void WaterWallTouchAction()
    {
        isWaterWall = true;
        isWaterWallCollision = true;
        waterWallMovementDirection = horizontal > 0?1:-1;
    }

    public void WaterWallEndTouchAction()
    {
        isWaterWall = false;
        isWaterWallCollision = false;
    }
}
