using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] racketHitSound;
    [SerializeField] AudioClip[] tableHitSound;
    [SerializeField] AudioClip[] dropSound;
    [SerializeField] AudioSource audioSource;

    private void PlayAudio(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        AudioClip randomClip = clips[randomIndex];

        audioSource.clip = randomClip;
        audioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Racket")
        {
            PlayAudio(racketHitSound);
        }

        else if (collision.gameObject.tag == "Table")
        {
            PlayAudio(tableHitSound);
        }

        else if(collision.gameObject.tag == "Floor")
        {
            PlayAudio(dropSound);
        }
    }
}
