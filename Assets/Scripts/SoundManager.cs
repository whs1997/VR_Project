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
        AudioClip randomClip = clips[randomIndex]; // Ŭ�� �迭 �� randomIndex�� �ش��ϴ� Ŭ�� ���

        audioSource.clip = randomClip;
        audioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Racket") // ���� ���Ͽ� �ε�����
        {
            PlayAudio(racketHitSound);
        }

        else if (collision.gameObject.tag == "Table") // ���� ���̺� �ε�����
        {
            PlayAudio(tableHitSound);
        }

        else if(collision.gameObject.tag == "Floor") // ���� �ٴڿ� ��������
        {
            PlayAudio(dropSound);
        }
    }
}
