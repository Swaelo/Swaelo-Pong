// ================================================================================================================================
// File:        BallMovement.cs
// Description:	Moves the ball around the table during gameplay
// Author:	    Harley Laurie https://www.github.com/Swaelo/
// ================================================================================================================================

using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float DefaultBallSpeed = 1.5f;   //Starting speed of the ball
    public float BallSpeed = 1.5f;  //Current speed of the ball
    public float HitSpeedIncrease = 0.5f;  //Speed added when the ball is hit by a paddle
    public float XDirection = 0f;   //Current direction of movement on X axis
    public float YDirection = 0f;   //Current direction of movement on Y axis

    private void Awake()
    {
        //Reset the ball immediately when the scene begins
        ResetBall();
    }

    //Resets the ball back speed and position, then sends it off in a random direction
    private void ResetBall()
    {
        BallSpeed = DefaultBallSpeed;
        transform.position = Vector3.zero;
        XDirection = Random.Range(-1f, 1f);
        YDirection = Random.Range(-1f, 1f);
    }

    private void Update()
    {
        //Press R resets the ball
        if (Input.GetKeyDown(KeyCode.R))
            ResetBall();

        //Create a new movement vector for the ball based on its current speed and movement direction
        Vector3 BallMovement = Vector3.zero;
        BallMovement.x = XDirection;
        BallMovement.y = YDirection;
        BallMovement = BallMovement.normalized * BallSpeed * Time.deltaTime;

        //Apply this movement to the ball
        transform.position += BallMovement;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Hitting the Top or Bottom borders reverses the balls YDirection value
        if (other.transform.CompareTag("Top") || other.transform.CompareTag("Bottom"))
            YDirection = -YDirection;
        //Hitting one of the paddles sends the ball back in the opposite direction
        else if(other.transform.CompareTag("LeftPaddle") || other.transform.CompareTag("RightPaddle"))
        {
            //Reverse the balls XDirection value to send it towards the players side
            XDirection = -XDirection;
            //Add a small amount of speed to the ball
            BallSpeed += HitSpeedIncrease;
        }
        //Hitting the left border resets the ball and gives a point to the player
        else if (other.transform.CompareTag("Left"))
        {
            ScoreKeeper.Instance.ScoreRight();
            ResetBall();
        }
        //Hitting the right border resets the ball and gives a point to the computer
        else if (other.transform.CompareTag("Right"))
        {
            ScoreKeeper.Instance.ScoreLeft();
            ResetBall();
        }
    }
}
