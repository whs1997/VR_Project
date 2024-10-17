using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float power = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        // Ź��ä�� �浹�ϸ�
        if (collision.gameObject.tag == "Racket")
        {
            Vector3 collisionDir = collision.contacts[0].normal; // �浹�� ����
            float impactSpeed = collision.relativeVelocity.magnitude; // �浹 �ӵ�

            Vector3 impactForce = -collisionDir * impactSpeed * power; // �浹�� �ݴ� �������� ���� ����

            rigid.AddForce(impactForce, ForceMode.Impulse);
        }
    }
}
