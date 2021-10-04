using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioMixerGroup audioOutputMixer;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3)]
        public float pitch;
        [Range(0, 256)]
        public int priority;



        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }

    public Sound[] sounds;
    public static AudioManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {

            Instance = this;
            //DontDestroyOnLoad(this.gameObject);

            //-------------------------------//
            //----Rest of your Awake code----//
            //-------------------------------//

        }
        else
        {
            Destroy(this);
        }


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.priority = s.priority;
            s.source.outputAudioMixerGroup = s.audioOutputMixer;
        } 
    }


    public void MenuStart()
    {
        Play("MusicMenu");
    }
    public void GameStart()
    {
        Play("Music");
        Play("Ambience");
    }

    public void Play (string name)
    {
        Sound s= Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("sound" + name + "not found");
            return;
        }
       
        s.source.Play();
    }


    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("sound" + name + "not found");
            return;
        }

        s.source.Stop();
    }
}
