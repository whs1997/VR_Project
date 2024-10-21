using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;
using UnityEngine.XR.OpenXR.Input;

public class Racket : MonoBehaviour
{
    [SerializeField] float power = 1f; // 공에 가할 힘
    private Vector3 preBallPos; // 공의 이전프레임의 위치
    private RaycastHit hit;

    private void FixedUpdate()
    {
        RaycastBall();
    }

    private void RaycastBall()
    {
        if (preBallPos != null)
        {
            // 공의 이전 위치에서 현재 위치로 Raycast
            Vector3 hitDir = (preBallPos - transform.position).normalized; // 공의 진행 방향

            // distance : 한 프레임동안 움직인 공의 거리 계산해 Ray의 길이 제한
            if (Physics.Raycast(transform.position, hitDir, out hit, Vector3.Distance(preBallPos, transform.position)))
            {
                // 공에 맞으면
                if (hit.transform.gameObject.tag == "Ball")
                {                    
                    Rigidbody ballRigid = hit.transform.GetComponent<Rigidbody>();

                    Vector3 racketVelocity = GetComponent<Rigidbody>().velocity;

                    // 공에 가할 힘(라켓의 속도와 공의 진행 방향)
                    Vector3 hitForce = (racketVelocity + hitDir) * power;

                    ballRigid.AddForce(hitForce, ForceMode.Impulse);
                }
            }
        }
        preBallPos = transform.position; // 공의 위치 저장
    }

}