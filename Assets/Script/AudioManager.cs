using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip ArrowAudioClip;
    public AudioClip DeathAudioClip;

    public static AudioManager Instance {get;private set;}

    void Awake(){
        Instance = this;
    }

    public void PlayArrowSound(){
        StartCoroutine(PlaySound(ArrowAudioClip));
    }

    public void PlayDeathSound(){
        StartCoroutine(PlaySound(DeathAudioClip));
    }

    private IEnumerator PlaySound(AudioClip clip){
        GameObject go = ObjectPoolerManager.Instance.AudioPooler.GetPooledObject();
        go.transform.SetParent(GameManager.Instance.otherObjectList.transform);
        go.SetActive(true);
        go.GetComponent<AudioSource>().PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        go.SetActive(false);
    }
}
