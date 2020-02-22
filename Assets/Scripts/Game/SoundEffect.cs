// ================================================================================================================================
// File:        SoundEffect.cs
// Description:	Serializable sound effect object so sound library can be filled through the inspector
// Author:	    Harley Laurie https://www.github.com/Swaelo/
// ================================================================================================================================

using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string SoundName;
    public AudioClip SoundFile;
}
