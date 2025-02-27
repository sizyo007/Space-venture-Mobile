using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;

    public static AudioManager instance;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {

            if (instance == null)
                instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
           DontDestroyOnLoad(gameObject);

        
            foreach (Sounds s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();

                s.source.clip = s.clip;
                s.source.volume =s.volume;
                s.source.pitch = s.pitch;

                s.source.loop = s.loop;

                s.source.mute = s.Mute;
            }
       
        } 
    


public void Play(string name)
{
    Sounds s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
           // Debug.Log("Audio : " + name + " is Missing");
            return;  
        }
        s.source.Play();
}
    public void Stop(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Stop();

        s.source.mute = true;
    }
    public void MuteSound(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        s.source.mute = true;

    }
    public void UnMuteSound(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        s.source.mute = false;

    }
    public void PauseAllGameSound()
    {
        Stop("HealthMinus");
        Stop("GameOver");
        Stop("laserFire");
        Stop("Consume");
        Stop("BackGroundTheme");
        Stop("StartMenuBackGroundTheme");
        Stop("PauseButtonSound");
    }
    public void UnPauseAllGameSound()
    {
        UnMuteSound("HealthMinus");
        UnMuteSound("GameOver");
        UnMuteSound("laserFire");
        UnMuteSound("Consume");
        UnMuteSound("BackGroundTheme");
        UnMuteSound("StartMenuBackGroundTheme");
        UnMuteSound("PauseButtonSound");
    }

}