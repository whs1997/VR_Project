using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform ball; // ���� ��ġ
    [SerializeField] Rigidbody ballRigid;  // ���� ������ٵ�

    [SerializeField] float moveSpeed = 3f;  // ���� �̵� �ӵ�
    [SerializeField] float power = 1f;  // ���� ĥ�� ���� ��
    [SerializeField] float upper = 0.5f; // ���� ĥ�� ���� �ø� ��

    [SerializeField] float attackRange = 0.2f; // ���� Ÿ���� ����

    // ����� ������ �̵� ���� ����
    private Vector3 minBound = new Vector3(-1.2f, 0.5f, 1.5f);
    private Vector3 maxBound = new Vector3(1.2f, 2f, 2f);

    private Vector3 targetPosition;  // ������ �̵��� ��ǥ ��ġ

    private void Update()
    {
        // ���� �̵��� ���� ���浵 �̵�
        PredictBall();
        MoveToTarget();
    }

    private void PredictBall()
    {
        // ���� �ӵ��� ���� ���� ������ ��ġ ���
        Vector3 ballVelocity = ballRigid.velocity;
        float getTime = (ball.position.y - transform.position.y) / ballVelocity.y;

        // ���� ���濡�� ������ x, z ��ǥ�� ���
        targetPosition = ball.position + ballVelocity * getTime;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBound.x, maxBound.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBound.y, maxBound.y);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minBound.z, maxBound.z);
    }

    private void MoveToTarget()
    {
        // �̵��� ����
        Vector3 direction = (targetPosition - transform.position).normalized;
        // �ش� �������� �̵�
        transform.position += direction * moveSpeed * Time.deltaTime;

        // ���� ���ݹ��� ���� ���� ġ��
        if(Vector3.Distance(transform.position, ball.position) < attackRange)
        {
            HitBall();
        }
    }

    private void HitBall()
    {
        // ���� ������ ����
        Vector3 returnDir = (ball.position - transform.position).normalized;
        // ���� ���� �������� �ļ� �������� ������
        returnDir.y = upper;

        ballRigid.AddForce(returnDir * power, ForceMode.Impulse);
    }
}
