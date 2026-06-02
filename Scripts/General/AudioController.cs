using System;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    void Awake()
    {
        // Ensure only one AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            instance.Start();
            Destroy(gameObject);
        }
    }

    static Dictionary<string, AudioSource[]> sounds = new ();

    public AudioSource[] intro = null;      //pop_sound     [0]
    public AudioSource[] cutscene2 = null;  //kick_bird     [0], spaceship_fall [1], ring_ears  [2]
    public AudioSource[] ui = null;         //menuClick     [0], tape           [1]
    public AudioSource[] phone = null;      //phoneClick    [0], phoneUnlock    [1]
    public AudioSource[] steps = null;      //трава       [0,1], пол          [2,3], асфальт  [4,5]
    public AudioSource[] gun = null;        //gun_shot      [0], gun_reload     [1]
    public AudioSource[] door = null;       //come_through  [0], locked         [1]
    public AudioSource[] voice = null;      //john          [0], stivia         [1], di         [3]

    private void InitializeSounds() //тут звуки в словарь записываются
    {
        sounds.Add("intro", intro);
        sounds.Add("cutscene2", cutscene2);
        sounds.Add("ui", ui);
        sounds.Add("phone", phone);
        sounds.Add("steps", steps);
        sounds.Add("gun", gun);
        sounds.Add("door", door);
        sounds.Add("voice", voice);
    }

    void Start()
    {
        sounds.Clear();
        InitializeSounds();

        //Debug.Log($"Значения звука: {SaveManager.instance.activeSettings.volume[0]}, {SaveManager.instance.activeSettings.volume[1]}, {SaveManager.instance.activeSettings.volume[2]}");
        
    }
    //-----Проигрывание звуков
    public void PlaySound(string category, int number)
    {
        sounds[category][number].Play();
    }
}