using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] AudioSource audioPrefab;
    void Awake() //Singleton
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundEffect(AudioClip audioClip,float volume)
    {
        //AudioClip audioClip = audioClips[Random.Range(0,audioClips.Length)];
        AudioSource audioSource = Instantiate(audioPrefab,new Vector3(0f,0f,0f),Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioClip.length;
        Destroy(audioSource.gameObject,clipLength);
    }
    //USE THIS TO PLAY SOUNDS
    //SoundManager.instance.PlaySoundEffect(AudioClip,this.transform.position,1f);
}
