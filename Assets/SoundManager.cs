using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] racketHitSound;
    [SerializeField] AudioClip[] tableHitSound;
    [SerializeField] AudioSource audioSource;

    private void PlayAudio(AudioClip[] clips)
    {
        if (audioSource != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip randomClip = clips[randomIndex];
            Debug.Log($"{randomClip.name} 재생");

            audioSource.clip = randomClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("먼가 잘못됨");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Racket")
        {
            PlayAudio(racketHitSound);
            Debug.Log("라켓에 닿음");
        }

        else if(collision.gameObject.tag == "Table")
        {
            PlayAudio(tableHitSound);
            Debug.Log("테이블에 닿음");
        }
    }


}
