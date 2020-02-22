// ================================================================================================================================
// File:        SoundEffectsPlayer.cs
// Description:	Handles the playing of sound effects during gameplay
// Author:	    Harley Laurie https://www.github.com/Swaelo/
// ================================================================================================================================

using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    public static SoundEffectsPlayer Instance;  //Singleton class instance
    private AudioSource SoundPlayer; //Component usued to play the sound effects

    //Assign singleton and audiosource component references
    private void Awake()
    {
        Instance = this;
        SoundPlayer = GetComponent<AudioSource>();
    }

    public SoundEffect[] SoundEffects;  //Array of sound effect files, with their filename included

    //Plays a sound effect if it can be found to exist in the SoundEffects array
    public void PlaySound(string SoundName)
    {
        foreach(SoundEffect Sound in SoundEffects)
        {
            if(Sound.SoundName == SoundName)
            {
                SoundPlayer.PlayOneShot(Sound.SoundFile);
                return;
            }
        }
    }
}
