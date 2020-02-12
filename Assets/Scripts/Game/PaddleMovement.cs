// ================================================================================================================================
// File:        PaddleMovement.cs
// Description:	Tracks whether or not the paddle is currently moving, and if so in which direction
// Author:	    Harley Laurie https://www.github.com/Swaelo/
// ================================================================================================================================

using UnityEngine;

public enum Direction
{
    None = 0,
    Up = 1,
    Down = 2
}

public class PaddleMovement : MonoBehaviour
{
    public Direction Movement = Direction.None; //Tracks if the paddle is currently moving, and in which direction
    private Vector3 PreviousPosition;   //Paddles position in the previous frame, compared with current position to check when/where the paddle is moving

    private void Awake()
    {
        //Store initial position as the previous position so the first direction check doesnt mess up
        PreviousPosition = transform.position;
    }

    private void Update()
    {
        //Track paddles movement
        TrackMovement();
    }

    //Tracks when the paddle is moving, and if so in which direction
    private void TrackMovement()
    {
        //Check if the paddle is currently moving
        bool IsMoving = transform.position != PreviousPosition;

        //Set to None if its not moving at all
        if (!IsMoving)
            Movement = Direction.None;
        //Otherwise figure out the movement direction
        else
            Movement = transform.position.y > PreviousPosition.y ? Direction.Up : Direction.Down;

        //Store position for next frames tracking update
        PreviousPosition = transform.position;
    }
}
