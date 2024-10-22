using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR.Interaction.Toolkit;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float maxSpeed; // 공의 최대 속도

    [SerializeField] Transform resetPos; // 공이 떨어지거나 게임시작할때 원위치

    [SerializeField] float dragConst = 0.005f; // 공기저항의 상수
    [SerializeField] float magnusConst = 0.005f; // 마그누스효과의 상수 ( 값이 높으면 공이 막 휨 )

    [SerializeField] GameManager gameManager;
    private bool isPlayerSide = false; // 공이 내쪽, 상대쪽에 있는지 확인
    private int bounceCount = 0;

    private void Start()
    {
        rigid.isKinematic = true; // 공을 컨트롤러로 집기 전까지 움직이지 않게 
    }

    private void Update()
    {
        if (rigid.velocity.magnitude > maxSpeed) // 공이 너무빨라지지않게 최대속도 제어
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }

        if (transform.position.y < -5) // 혹시 공이 바닥을 뚫고 떨어지면 리셋
        {
            ResetBall();
        }

        if (transform.position.z < 0)
        {
            isPlayerSide = true; // 공의 z 위치가 0 아래면 내쪽
        }
        else
        {
            isPlayerSide = false; // z가 0 이상이면 상대쪽
        }
    }

    private void FixedUpdate()
    {
        // 공기 저항에 대한 물리 연산은 fixedupdate
        rigid.AddRelativeForce(MagnusForce());
        rigid.AddRelativeForce(AirResist());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Net")
        {
            if (isPlayerSide)
            {
                gameManager.AddEnemyScore(); // 내쪽에서 네트, 바닥에 걸리면 상대가 득점
                ResetBall();
            }
            else
            {
                gameManager.AddPlayerScore(); // 상대쪽에서 걸리면 득점
                ResetBall();
            }
        }

        if (collision.gameObject.tag == "Table")
        {
            if (isPlayerSide) // 공이 내쪽 탁구대에서 튕기면
            {
                bounceCount++;
                if (bounceCount >= 2) // 두번 튕기면
                {
                    gameManager.AddEnemyScore(); // 상대방 득점
                    ResetBall();
                }
            }
            else if (!isPlayerSide)
            {
                bounceCount++;
                if (bounceCount >= 2)
                {
                    gameManager.AddPlayerScore(); // 상대쪽에서 두번 튕기면 내가 득점
                    ResetBall();
                }
            }
        }
    }

    private void ResetBall()
    {
        // 리셋 지점으로 이동
        transform.position = resetPos.position;
        transform.rotation = resetPos.rotation;

        rigid.isKinematic = true; // 컨트롤러로 집기 전까지 움직이지 않음

        // 공의 속도 없애기 
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        bounceCount = 0;
    }
    /*
    public void Grabbed(SelectEnterEventArgs args)
    {
        rigid.isKinematic = false;
        Debug.Log("공 잡음");
    }
    */
    public void Released(SelectExitEventArgs args)
    {
        rigid.isKinematic = false; // 컨트롤러로 잡았다가 놔야 움직임
        // Debug.Log("공 놓음");
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
