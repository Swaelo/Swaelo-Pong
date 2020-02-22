// ================================================================================================================================
// File:        BallMovement.cs
// Description:	Moves the ball around the table during gameplay
// Author:	    Harley Laurie https://www.github.com/Swaelo/
// ================================================================================================================================

using UnityEngine;

public class BallMovement : MonoBehaviour
{
    //Movement variables
    public float StartingSpeed = 3f;  //Balls initial speed at the start of a new match
    public float CurrentSpeed;      //Balls current movement speed increased over time
    public float HitSpeedIncrease = 0.2f;   //Horizontal speed added to the ball whenever it bounces of one of the paddles
    public float PaddleBounceSpeedIncrease = 0.25f; //Vertical speed added to the ball whenever it bounces of a moving paddle

    //Physics variables
    private Rigidbody RigidBody;    //The balls physics rigidbody component
    private Vector3 CurrentDirection;   //Current direction the ball is travelling

    //Movement trackers for the 2 paddles on the table
    public PaddleMovement LeftPaddle;
    public PaddleMovement RightPaddle;

    //Round start variables
    public NumberDisplayer RoundBeginCounter;   //Displays the numbers of the current round begin counter
    private float BeginCounterDuration = 3f;    //How long the start of round countdown goes for
    private float BeginCounterRemaining = 3f;   //Time remaining until the new round begins
    private bool RoundBegun = false;          //Flags if the current round has started yet

    private void Awake()
    {
        //Store reference to the rigidbody component
        RigidBody = GetComponent<Rigidbody>();
    }

    //Returns the ball to the middle of the table and sends it off in a random direction
    private void ResetBall()
    {
        //Reset balls position and speed
        transform.position = Vector3.zero;
        CurrentSpeed = StartingSpeed;

        //Give a new movement direction
        bool MoveLeft = Random.value > 0.5f;
        CurrentDirection = new Vector3(MoveLeft ? 1 : -1, Random.Range(-1f, 1f), 0);
    }

    private void Update()
    {
        //Handle the countdown timer if the round hasnt begun yet
        if(!RoundBegun)
        {
            HandleRoundCountdown();
            return;
        }

        //Apply movement to the ball
        transform.position += CurrentDirection * CurrentSpeed * Time.deltaTime;
    }

    //Manages counting down and displaying of the new round timer
    private void HandleRoundCountdown()
    {
        //Decrement the current timer value
        BeginCounterRemaining -= Time.deltaTime;

        //Start the round if the timer has expired
        if(BeginCounterRemaining <= 0.0f)
        {
            RoundBegun = true;
            RoundBeginCounter.DisplayNone();
            ResetBall();
            return;
        }

        //Update the timer display while the timer is still counting down
        RoundBeginCounter.DisplayNumber((int)BeginCounterRemaining + 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Read the tag of the object that the ball collided with
        string CollisionTag = collision.transform.tag;

        //Use this to pass the collision information onto the relevant collision handler function
        switch(CollisionTag)
        {
            //Top/Bottom wall collision
            case "Boundary":
                HandleBoundaryCollision(collision);
                break;
            //Paddle collisions
            case "LeftPaddle":
                HandlePaddleCollision(collision, true);
                break;
            case "RightPaddle":
                HandlePaddleCollision(collision, false);
                break;
            //Wall collisions
            case "LeftWall":
                HandleRoundComplete(false);
                break;
            case "RightWall":
                HandleRoundComplete(true);
                break;
        }
    }

    private void HandleBoundaryCollision(Collision Boundary)
    {
        ReflectBall(Boundary);
        SoundEffectsPlayer.Instance.PlaySound("Bounce Boundary");
    }

    private void HandlePaddleCollision(Collision Paddle, bool HitLeftPaddle)
    {
        ReflectBall(Paddle);
        SoundEffectsPlayer.Instance.PlaySound("Bounce Paddle");

        //Increase the balls speed by a small amount
        CurrentSpeed += HitSpeedIncrease;

        //Adjust the balls vertical direction if the paddle is moving when the ball bounced off
        PaddleMovement CurrentPaddle = HitLeftPaddle ? LeftPaddle : RightPaddle;
        if (CurrentPaddle.Movement != Direction.None)
            CurrentDirection.y += CurrentPaddle.Movement == Direction.Up ? PaddleBounceSpeedIncrease : -PaddleBounceSpeedIncrease;
        //Otherwise just make a small random adjustment to the balls vertical direction if the paddle was stationary
        else
            CurrentDirection.y += Random.Range(-0.1f, 0.1f);
        //Make sure paddle direction values stay in allowed values
        CurrentDirection.y = Mathf.Clamp(CurrentDirection.y, -1f, 1f);
    }

    //Resets the ball and adds a point to the appropriate players score
    private void HandleRoundComplete(bool LeftWins)
    {
        //Restart the round begin countdown
        RoundBegun = false;
        BeginCounterRemaining = BeginCounterDuration;
        transform.position = Vector3.zero;
        CurrentDirection = Vector3.zero;

        SoundEffectsPlayer.Instance.PlaySound("Ball Lost");
        ScoreKeeper.Instance.ScorePoint(LeftWins);
        ResetBall();
    }

    //Reflects the ball from the surface it collided with
    private void ReflectBall(Collision CollisionSurface)
    {
        Vector3 CollisionNormal = CollisionSurface.contacts[0].normal;
        CurrentDirection = Vector3.Reflect(CurrentDirection, CollisionNormal);
    }
}
