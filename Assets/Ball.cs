using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float power = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        // 탁구채와 충돌하면
        if (collision.gameObject.tag == "Racket")
        {
            Vector3 collisionDir = collision.contacts[0].normal; // 충돌한 방향
            float impactSpeed = collision.relativeVelocity.magnitude; // 충돌 속도

            Vector3 impactForce = -collisionDir * impactSpeed * power; // 충돌한 반대 방향으로 힘을 가함

            rigid.AddForce(impactForce, ForceMode.Impulse);
        }
    }
}
