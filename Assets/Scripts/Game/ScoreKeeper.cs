﻿// ================================================================================================================================
// File:        ScoreKeeper.cs
// Description:	Tracks and displays the current score to the players
// Author:	    Harley Laurie https://www.github.com/Swaelo/
// ================================================================================================================================

using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper Instance = null;
    private TextMesh ScoreDisplayMesh;   //TextMesh object used to display the current score on the screen
    private int LeftPlayerPoints = 0;   //How many points left-side player has
    private int RightPlayerPoints = 0; //How many points right-side player has

    private void Awake()
    {
        //Assign singleton class reference and TextMesh component reference
        Instance = this;
        ScoreDisplayMesh = GetComponent<TextMesh>();
    }

    //Awards a point to the player who won the round
    public void ScorePoint(bool LeftWins)
    {
        LeftPlayerPoints += LeftWins ? 1 : 0;
        RightPlayerPoints += LeftWins ? 0 : 1;
        UpdateDisplay();
    }

    //Updates the textmesh to display the current score
    private void UpdateDisplay()
    {
        ScoreDisplayMesh.text = LeftPlayerPoints + " : " + RightPlayerPoints;
    }
}
