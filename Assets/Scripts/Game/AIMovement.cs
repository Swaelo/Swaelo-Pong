// ================================================================================================================================
// File:        AIMovement.cs
// Description:	Controls movement of the computers paddle
// Author:	    Harley Laurie https://www.github.com/Swaelo/
// ================================================================================================================================

using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public GameObject Ball;     //The current ball being used for play
    private float MoveSpeed = 5f;   //Paddles movement speed
    private float MinYPos = -3.5f;  //Lowest Y value allowed for the paddles position
    private float MaxYPos = 3.5f;   //Highest Y value allowed for the paddles position

    private void Update()
    {
        //Keep movement speed independant of the current FPS
        float FrameBasedMoveSpeed = MoveSpeed * Time.deltaTime;

        //Get a new position for the paddle based on the balls current position
        Vector3 NewPaddlePos = transform.position;
        NewPaddlePos.y = Ball.transform.position.y;

        //Keep the paddle position within accepted values
        NewPaddlePos.y = Mathf.Clamp(NewPaddlePos.y, MinYPos, MaxYPos);

        //Move the paddle to its new position
        transform.position = NewPaddlePos;
    }
}
