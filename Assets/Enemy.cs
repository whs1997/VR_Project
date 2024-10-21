using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform ball; // 공의 위치
    [SerializeField] Rigidbody ballRigid;  // 공의 리지드바디

    [SerializeField] float moveSpeed = 3f;  // 상대방 이동 속도
    [SerializeField] float power = 1f;  // 공을 칠때 가할 힘
    [SerializeField] float upper = 0.5f; // 공을 칠때 위로 올릴 힘

    [SerializeField] float attackRange = 0.2f; // 공을 타격할 범위

    // 상대편 진영의 이동 범위 설정
    private Vector3 minBound = new Vector3(-1.2f, 0.5f, 1.5f);
    private Vector3 maxBound = new Vector3(1.2f, 2f, 2f);

    private Vector3 targetPosition;  // 상대방이 이동할 목표 위치

    private void Update()
    {
        // 공의 이동을 따라 상대방도 이동
        PredictBall();
        MoveToTarget();
    }

    private void PredictBall()
    {
        // 공의 속도를 통해 공이 떨어질 위치 계산
        Vector3 ballVelocity = ballRigid.velocity;
        float getTime = (ball.position.y - transform.position.y) / ballVelocity.y;

        // 공이 상대방에게 도달할 x, z 좌표를 계산
        targetPosition = ball.position + ballVelocity * getTime;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBound.x, maxBound.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBound.y, maxBound.y);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minBound.z, maxBound.z);
    }

    private void MoveToTarget()
    {
        // 이동할 방향
        Vector3 direction = (targetPosition - transform.position).normalized;
        // 해당 방향으로 이동
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 공이 공격범위 내로 오면 치기
        if(Vector3.Distance(transform.position, ball.position) < attackRange)
        {
            HitBall();
        }
    }

    private void HitBall()
    {
        // 공을 리턴할 방향
        Vector3 returnDir = (ball.position - transform.position).normalized;
        // 공을 조금 위쪽으로 쳐서 내쪽으로 오게함
        returnDir.y = upper;

        ballRigid.AddForce(returnDir * power, ForceMode.Impulse);
    }
}
