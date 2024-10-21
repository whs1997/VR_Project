using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float power = 2f;
    [SerializeField] float maxSpeed;

    [SerializeField] Transform resetPos;

    [SerializeField] float dragConst = 0.005f;
    [SerializeField] float magnesConst = 0.005f;

    private Vector3 preBallPos;
    private RaycastHit hit;

    private void Update()
    {
        if(rigid.velocity.magnitude > maxSpeed) // 공이 너무 빨라 속도 제한
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }

        if(transform.position.y < -5)
        {
            ResetBall();
        }
    }

    private void FixedUpdate()
    {
        rigid.AddRelativeForce(MagnesForce());
        rigid.AddRelativeForce(AirForce());

        RaycastBall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 탁구채와 충돌하면
        /*
        if (collision.gameObject.tag == "Racket")
        {
            Vector3 collisionDir = collision.contacts[0].normal; // 충돌한 방향
            float impactSpeed = collision.relativeVelocity.magnitude; // 충돌 속도

            Vector3 impactForce = -collisionDir * impactSpeed * power; // 충돌한 반대 방향으로 힘을 가함

            rigid.AddForce(impactForce, ForceMode.Force);
        }
        */
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Net")
        {
            ResetBall();
        }
    }

    private void ResetBall()
    {
        transform.position = resetPos.position;
        transform.rotation = resetPos.rotation;

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    private Vector3 AirForce() // 공기저항값을 리턴
    {
        float velocityMag = rigid.velocity.magnitude;
        Vector3 velocityDir = rigid.velocity.normalized;
        float airDragMag = dragConst * Mathf.Pow(velocityMag, 2f);

        return airDragMag * -velocityDir;
    }

    private Vector3 MagnesForce() // 마그누스힘 값을 리턴
    {
        Vector3 magnesForce = magnesConst * Vector3.Cross(rigid.velocity, rigid.angularVelocity);

        return magnesForce;
    }

    private void RaycastBall()
    {
        if (preBallPos != null)
        {
            // 현재 위치에서 공의 진행방향으로 Raycast
            // 마지막 Parameter : Ray의 길이는 제한적이어야 하므로 한 프레임동안 움직인 공의 거리 계산
            if (Physics.Raycast(transform.position, (preBallPos - transform.position).normalized, out hit, Vector3.Distance(preBallPos, transform.position)))
            {
                // 라켓에 맞으면
                if (hit.transform.gameObject.tag == "Racket")
                {
                    // 공을 쳐서 보내기
                    //Vector3 hitDir = (preBallPos - transform.position).normalized; // 공의 진행 방향
                    //rigid.AddForce(hitDir * power, ForceMode.Force);

                    // 라켓의 속도
                    Rigidbody racketRigid = hit.transform.GetComponent<Rigidbody>();
                    Vector3 racketVelocity = racketRigid.velocity;

                    // 공의 진행방향
                    Vector3 hitDir = (preBallPos - transform.position).normalized; // 공의 진행 방향

                    // 공에 가할 힘(라켓의 속도와 공의 진행 방향)
                    Vector3 hitForce = (racketVelocity + hitDir) * power;

                    rigid.AddForce(hitForce, ForceMode.Impulse);
                }
            }
        }
        preBallPos = transform.position;
    }
}
