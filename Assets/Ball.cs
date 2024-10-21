using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float maxSpeed;

    [SerializeField] Transform resetPos;

    [SerializeField] float dragConst = 0.005f;
    [SerializeField] float magnusConst = 0.005f;

    private void Update()
    {
        if(rigid.velocity.magnitude > maxSpeed) // 공이 너무빨라지지않게 최대속도 제어
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }

        if(transform.position.y < -5) // 가끔 공이 바닥을 뚫고 내려가면 리셋
        {
            ResetBall();
        }        
    }

    private void FixedUpdate()
    {
        rigid.AddRelativeForce(MagnusForce());
        rigid.AddRelativeForce(AirResist());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Net")
        {
            ResetBall(); // 네트나 바닥에 닿으면 공 리셋
        }
    }

    private void ResetBall()
    {
        // 리셋 지점으로 이동
        transform.position = resetPos.position;
        transform.rotation = resetPos.rotation;

        // 공의 속도 없애기
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    private Vector3 AirResist() // 공기저항값을 리턴
    {
        float velocityMag = rigid.velocity.magnitude;
        Vector3 velocityDir = rigid.velocity.normalized;
        float airDragMag = dragConst * Mathf.Pow(velocityMag, 2f);

        return airDragMag * -velocityDir;
    }

    private Vector3 MagnusForce() // 마그누스힘 값을 리턴
    {
        Vector3 magnusForce = magnusConst * Vector3.Cross(rigid.velocity, rigid.angularVelocity);

        return magnusForce;
    }
}
