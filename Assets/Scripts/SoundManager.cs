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
        AudioClip randomClip = clips[randomIndex]; // 클립 배열 중 randomIndex에 해당하는 클립 재생

        audioSource.clip = randomClip;
        audioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Racket") // 공이 라켓에 부딪히면
        {
            PlayAudio(racketHitSound);
        }

        else if (collision.gameObject.tag == "Table") // 공이 테이블에 부딪히면
        {
            PlayAudio(tableHitSound);
        }

        else if(collision.gameObject.tag == "Floor") // 공이 바닥에 떨어지면
        {
            PlayAudio(dropSound);
        }
    }
}
