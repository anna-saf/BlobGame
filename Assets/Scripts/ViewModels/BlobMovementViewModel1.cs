using System;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class BlobMovementViewModel1 
{
    public ReactiveProperty<Vector3> blobMovement;
    public ReactiveProperty<bool> isWall;

    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float ySpeed;
    private float jumpSpeed = 1f;
    private float gravityValue = -20.81f;

    private float _moveSpeed;
    private float _maxSpeed;
    private float _minSpeed;
    private float _currentTemp;
    private float _prevTemp;
    private float _speedIncreaseFactor;
    private float _maxJumpHeight;
    public bool isTouchingWall;
    private Vector3 move;

    

    public BlobMovementViewModel1(ReactiveProperty <float> blobTemp, CharacterController characterController)
    {
        blobMovement = new ReactiveProperty<Vector3>();
        isTouchingWall = false;

        
        
        this.characterController = characterController;
       
        _maxJumpHeight = GlobalModel.Instance.MaxJumpHeight;
        _speedIncreaseFactor = GlobalModel.Instance.SpeedIncreaseFactor;
        _maxSpeed = GlobalModel.Instance.StartBlobSpeed * 2;
        _minSpeed = GlobalModel.Instance.StartBlobSpeed / 2;
        _moveSpeed = GlobalModel.Instance.StartBlobSpeed;
        _currentTemp = GlobalModel.Instance.StartBlobTemp;

        _prevTemp = _currentTemp;

        GameInput.Instance.OnJump += GameInput_OnJump;
        GameInput.Instance.OnMove += GameInput_OnMove;

        

        var t = Observable.EveryUpdate().Subscribe(_ => {

            if (characterController.isGrounded)
            {
                ySpeed = -1f;
            };

            RaycastHit hitSide;
            if (Physics.Raycast(characterController.transform.position,
              characterController.transform
              .TransformDirection(Vector3.forward),out hitSide, characterController.transform.localScale.x/2 - 0.1f)
            )
            {
                if(hitSide.transform.tag == "VerticalWall")
                {
                    isTouchingWall = true;
                }
            }
            else
            {
                isTouchingWall = false;
            }
            

            if ((characterController.collisionFlags & CollisionFlags.Above) != 0)
            {
                ySpeed = gravityValue * Time.deltaTime * 10;
            }
            


            if (isTouchingWall)
            {
                ySpeed = 0;
              
            }
            else
            {
                ySpeed += gravityValue * Time.deltaTime;
               
            }
            
            characterController.Move(new Vector3(0, ySpeed, 0) * Time.deltaTime);
            characterController.transform.position =
                new Vector3(characterController.transform.position.x,
                characterController.transform.position.y,0);
        });

        blobTemp.Subscribe(blobTemp =>
        {
            _currentTemp = blobTemp;

            if (_currentTemp > _prevTemp)
            {
                _moveSpeed += _speedIncreaseFactor;
            }

            if (_currentTemp < _prevTemp)
            {
                _moveSpeed -= _speedIncreaseFactor;

            }

        });
        CheckpointManager.Instance.OnGoToLastCheckpoint += CheckpointManagerOnGoToLastCheckpoint;
    }

    private void CheckpointManagerOnGoToLastCheckpoint(object sender, EventArgs e)
    {
        _moveSpeed = CheckpointManager.Instance.BlobMoveSpeedCheckpoint;
    }

    private void GameInput_OnMove(object sender, GameInput.MovementVectorEventArgs e)
    {
        PlayerMove(e.movementVector);
    }

    private void GameInput_OnJump(object sender, EventArgs e)
    {
        PlayerJump();
    }

    private float BlobSpeed()
    {
     
        if (_moveSpeed >= _maxSpeed)
        {
            _moveSpeed = _maxSpeed;
        }
        if (_moveSpeed <= _minSpeed)
        {
            _moveSpeed = _minSpeed;
        }
        return _moveSpeed;

    }    

    private void PlayerJump()
    {

        if (characterController.isGrounded)
        {
            float jumpHeight = _maxJumpHeight * characterController.transform.localScale.x;
            ySpeed = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue * jumpSpeed);
        } else
        {
            return;
        }


        characterController.Move(new Vector3(0, ySpeed, 0) * Time.deltaTime);

    }

    private void PlayerMove(Vector2 movement)
    {
        
        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }        
        

        if (isTouchingWall)
        {
            if (movement.x < 0 || movement.x >0 )
            {
                move = new Vector3(0, 1, 0);
            }
            else
                move = new Vector3(0, -1, 0);

        }
        else
        {
            move = new Vector3(movement.x,0, 0);
        }

        characterController.transform.rotation = Quaternion.LookRotation(new Vector2(movement.x,0));
        characterController.Move(move * Time.deltaTime * BlobSpeed());
        characterController.stepOffset = BlobSpeed() / 200;

        //if (move != Vector3.zero)
        //{
        //    blobMovement.Value = move;
            
        //}
    }
}
