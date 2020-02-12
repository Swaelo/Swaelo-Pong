// ================================================================================================================================
// File:        BallMovement.cs
// Description:	Moves the ball around the table during gameplay
// Author:	    Harley Laurie https://www.github.com/Swaelo/
// ================================================================================================================================

using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float StartingSpeed = 25f;   //Balls speed at the start of a new match
    public float HitSpeedIncrease = 0.5f;   //Horizontal speed added to ball whenever it hits a paddle
    public float PaddleBounceSpeedIncrease = 0.25f; //Vertical speed added to ball whenever it hits a moving paddle

    private Rigidbody RigidBody;    //The balls rigidbody component

    public PaddleMovement LeftPaddle;   //Movement trackers for the 2 paddles
    public PaddleMovement RightPaddle;

    private void Awake()
    {
        //Store reference to ball components
        RigidBody = GetComponent<Rigidbody>();

        //Apply a force randomly left or right to get the ball moving
        ResetBall();
    }

    //Reset the balls position and velocity, then apply force to send it in random direction either left or right
    private void ResetBall()
    {
        transform.position = Vector3.zero;
        RigidBody.velocity = Vector3.zero;
        bool StartLeft = Random.value > 0.5f;
        Vector3 StartingForce = new Vector3(StartLeft ? 1 : -1, 0, 0);
        StartingForce *= StartingSpeed;
        RigidBody.AddForce(StartingForce);
    }

    private void Update()
    {
        //Pressing R resets the ball
        if (Input.GetKeyDown(KeyCode.R))
            ResetBall();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Read the tag of the object that the ball collided with
        string CollisionTag = other.transform.tag;

        //Use the tag to determine the correct actions that should be taken
        switch(CollisionTag)
        {
            case "Top":
                HandleTopBottomCollision();
                break;
            case "Bottom":
                HandleTopBottomCollision();
                break;
            case "LeftPaddle":
                HandlePaddleCollision(true);
                break;
            case "RightPaddle":
                HandlePaddleCollision(false);
                break;
            case "Left":
                HandleRoundComplete(false);
                break;
            case "Right":
                HandleRoundComplete(true);
                break;
        }
    }

    //Reverses the balls vertical direction
    private void HandleTopBottomCollision()
    {
        //Take the current velocity, flip the Y value, then reapply it
        Vector3 Velocity = RigidBody.velocity;
        Velocity.y = -Velocity.y;
        RigidBody.velocity = Velocity;
    }

    //Reverse and increase the balls horizontal direction, adjust vertical velocity based on paddle movement
    private void HandlePaddleCollision(bool HitLeftPaddle)
    {
        //Take the current velocity and flip the X value
        Vector3 Velocity = RigidBody.velocity;
        Velocity.x = -Velocity.x;

        //Increase the balls horizontal velocity by a small amount
        Velocity.x += Velocity.x > 0.0f ? HitSpeedIncrease : -HitSpeedIncrease;

        //Adjust the balls vertical velocity if the paddle is currently moving
        PaddleMovement CurrentPaddle = HitLeftPaddle ? LeftPaddle : RightPaddle;
        if (CurrentPaddle.Movement != Direction.None)
            Velocity.y += CurrentPaddle.Movement == Direction.Up ? PaddleBounceSpeedIncrease : -PaddleBounceSpeedIncrease;
        //Make a small random adjustment to the balls vertical velocity if the paddle isnt moving at all
        else
            Velocity.y += Random.Range(-0.25f, 0.25f);

        //Apply the new velocity vector to the ball
        RigidBody.velocity = Velocity;
    }

    //Resets the ball and adds a point to the appropriate players score
    private void HandleRoundComplete(bool LeftWins)
    {
        ScoreKeeper.Instance.ScorePoint(LeftWins);
        ResetBall();
    }
}
