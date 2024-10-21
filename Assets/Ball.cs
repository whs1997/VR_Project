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
        if(rigid.velocity.magnitude > maxSpeed) // ���� �ʹ� ���� �ӵ� ����
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
        // Ź��ä�� �浹�ϸ�
        /*
        if (collision.gameObject.tag == "Racket")
        {
            Vector3 collisionDir = collision.contacts[0].normal; // �浹�� ����
            float impactSpeed = collision.relativeVelocity.magnitude; // �浹 �ӵ�

            Vector3 impactForce = -collisionDir * impactSpeed * power; // �浹�� �ݴ� �������� ���� ����

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

    private Vector3 AirForce() // �������װ��� ����
    {
        float velocityMag = rigid.velocity.magnitude;
        Vector3 velocityDir = rigid.velocity.normalized;
        float airDragMag = dragConst * Mathf.Pow(velocityMag, 2f);

        return airDragMag * -velocityDir;
    }

    private Vector3 MagnesForce() // ���״����� ���� ����
    {
        Vector3 magnesForce = magnesConst * Vector3.Cross(rigid.velocity, rigid.angularVelocity);

        return magnesForce;
    }

    private void RaycastBall()
    {
        if (preBallPos != null)
        {
            // ���� ��ġ���� ���� ����������� Raycast
            // ������ Parameter : Ray�� ���̴� �������̾�� �ϹǷ� �� �����ӵ��� ������ ���� �Ÿ� ���
            if (Physics.Raycast(transform.position, (preBallPos - transform.position).normalized, out hit, Vector3.Distance(preBallPos, transform.position)))
            {
                // ���Ͽ� ������
                if (hit.transform.gameObject.tag == "Racket")
                {
                    // ���� �ļ� ������
                    //Vector3 hitDir = (preBallPos - transform.position).normalized; // ���� ���� ����
                    //rigid.AddForce(hitDir * power, ForceMode.Force);

                    // ������ �ӵ�
                    Rigidbody racketRigid = hit.transform.GetComponent<Rigidbody>();
                    Vector3 racketVelocity = racketRigid.velocity;

                    // ���� �������
                    Vector3 hitDir = (preBallPos - transform.position).normalized; // ���� ���� ����

                    // ���� ���� ��(������ �ӵ��� ���� ���� ����)
                    Vector3 hitForce = (racketVelocity + hitDir) * power;

                    rigid.AddForce(hitForce, ForceMode.Impulse);
                }
            }
        }
        preBallPos = transform.position;
    }
}
