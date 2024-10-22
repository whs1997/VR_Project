using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform ball;
    [SerializeField] Rigidbody ballRigid;

    [SerializeField] float moveSpeed = 3f;  // ���� �̵� �ӵ�
    [SerializeField] float power = 0.8f;  // ���� ĥ�� ���� ��
    [SerializeField] float upper = 0.4f; // ���� ĥ�� ���� �ø� ��

    [SerializeField] float attackRange = 0.3f; // ���� Ÿ���� ����

    // ������ �̵� ���� ����
    private Vector3 minBound = new Vector3(-1.2f, 0.5f, 1.6f);
    private Vector3 maxBound = new Vector3(1.2f, 2f, 2f);

    private Vector3 targetPos;  // ������ �̵��� ��ǥ ��ġ

    private void Update()
    {
        GetTarget();
        MoveToTarget();
    }

    private void GetTarget() // ���� ��ġ targetPos�� ���
    {
        // ���� �ӵ��� ���� ���� ������ ��ġ ���
        Vector3 ballVelocity = ballRigid.velocity;
        // �ð� = �Ÿ� / �ӷ����� ���� ���� ��ġ�� �����ϴ� �ð��� ���
        float getTime = (ball.position.y - transform.position.y) / ballVelocity.y;

        // �ӵ� * �ð� = �Ÿ��� ���� ���濡�� ������ ��ġ�� ���
        targetPos = ball.position + ballVelocity * getTime;

        // ���� �������� �̵��ϰ� ����
        targetPos.x = Mathf.Clamp(targetPos.x, minBound.x, maxBound.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minBound.y, maxBound.y);
        targetPos.z = Mathf.Clamp(targetPos.z, minBound.z, maxBound.z);
    }

    private void MoveToTarget() // ���� ������ targetPos�� �̵�
    {
        // �̵��� ����
        Vector3 direction = (targetPos - transform.position).normalized;
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
        // ���� ���� �������� ġ����
        returnDir.y = upper;

        ballRigid.AddForce(returnDir * power, ForceMode.Impulse);
    }
}
