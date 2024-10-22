using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform ball;
    [SerializeField] Rigidbody ballRigid;

    [SerializeField] float moveSpeed = 3f;  // 상대방 이동 속도
    [SerializeField] float power = 0.8f;  // 공을 칠때 가할 힘
    [SerializeField] float upper = 0.4f; // 공을 칠때 위로 올릴 힘

    [SerializeField] float attackRange = 0.3f; // 공을 타격할 범위

    // 상대방의 이동 범위 설정
    private Vector3 minBound = new Vector3(-1.2f, 0.5f, 1.6f);
    private Vector3 maxBound = new Vector3(1.2f, 2f, 2f);

    private Vector3 targetPos;  // 상대방이 이동할 목표 위치

    private void Update()
    {
        GetTarget();
        MoveToTarget();
    }

    private void GetTarget() // 공의 위치 targetPos를 계산
    {
        // 공의 속도를 통해 공이 떨어질 위치 계산
        Vector3 ballVelocity = ballRigid.velocity;
        // 시간 = 거리 / 속력으로 공이 상대방 위치에 도달하는 시간을 계산
        float getTime = (ball.position.y - transform.position.y) / ballVelocity.y;

        // 속도 * 시간 = 거리로 공이 상대방에게 도달할 위치를 계산
        targetPos = ball.position + ballVelocity * getTime;

        // 범위 내에서만 이동하게 제한
        targetPos.x = Mathf.Clamp(targetPos.x, minBound.x, maxBound.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minBound.y, maxBound.y);
        targetPos.z = Mathf.Clamp(targetPos.z, minBound.z, maxBound.z);
    }

    private void MoveToTarget() // 공이 떨어질 targetPos로 이동
    {
        // 이동할 방향
        Vector3 direction = (targetPos - transform.position).normalized;
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
        // 공을 조금 위쪽으로 치게함
        returnDir.y = upper;

        ballRigid.AddForce(returnDir * power, ForceMode.Impulse);
    }
}
