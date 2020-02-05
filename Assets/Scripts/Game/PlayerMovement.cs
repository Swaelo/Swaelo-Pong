// ================================================================================================================================
// File:        PlayerMovement.cs
// Description:	Allows the player to move their paddle during gameplay
// Author:	    Harley Laurie https://www.github.com/Swaelo/
// ================================================================================================================================

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float MoveSpeed = 5f;   //How fast the players paddles move
    private float MaxYPos = 3.5f;   //How high the paddles are allowed to move
    private float MinYPos = -3.5f;  //How low the paddles are allowed to move

    public KeyCode UpMovement = KeyCode.UpArrow;  //Key used to move the paddle up
    public KeyCode DownMovement = KeyCode.DownArrow;    //Key used to move the paddle down

    private void Update()
    {
        //Read users input for moving the paddle up and down
        bool MoveUp = Input.GetKey(UpMovement);
        bool MoveDown = Input.GetKey(DownMovement);

        //Apply deltatime to keep the movement speed framerate independant
        float FrameBasedMoveSpeed = MoveSpeed * Time.deltaTime;

        //Create a new position for the paddle based on the player input and current movement speed
        Vector3 NewPaddlePos = transform.position;
        NewPaddlePos.y += MoveUp ? FrameBasedMoveSpeed : MoveDown ? -FrameBasedMoveSpeed : 0f;

        //Make sure the new position remains inside the limits
        NewPaddlePos.y = Mathf.Clamp(NewPaddlePos.y, MinYPos, MaxYPos);

        //Move the paddle to its new location
        transform.position = NewPaddlePos;
    }
}