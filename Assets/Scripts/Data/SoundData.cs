using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable,CreateAssetMenu(fileName ="SoundData",menuName = "Data/SoundData")]
public class SoundData : ScriptableObject
{

    public List<Sound> soundsData = new List<Sound>();

    public Sound GetSound(Game_Enum.PlayerSoundType playerSoundType){
        foreach(Sound sound in soundsData){
            if(sound.playerSoundType == playerSoundType){
                return sound;
            }
        }
        return null;
    }
}

[Serializable]
public class Sound{
    public Game_Enum.PlayerSoundType playerSoundType;
    public List<AudioClip> soundClip;
    public AudioClip RandamClip(){

        if(soundClip.Count>0){
            int index =  UnityEngine.Random.Range(0,soundClip.Count-1);
            return soundClip[index];
        }
        return null;
    }
}