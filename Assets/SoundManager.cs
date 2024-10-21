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
            Debug.Log($"{randomClip.name} ���");

            audioSource.clip = randomClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("�հ� �߸���");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Racket")
        {
            PlayAudio(racketHitSound);
            Debug.Log("���Ͽ� ����");
        }

        else if(collision.gameObject.tag == "Table")
        {
            PlayAudio(tableHitSound);
            Debug.Log("���̺� ����");
        }
    }


}
