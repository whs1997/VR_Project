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
        if(rigid.velocity.magnitude > maxSpeed) // ���� �ʹ����������ʰ� �ִ�ӵ� ����
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }

        if(transform.position.y < -5) // ���� ���� �ٴ��� �հ� �������� ����
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
            ResetBall(); // ��Ʈ�� �ٴڿ� ������ �� ����
        }
    }

    private void ResetBall()
    {
        // ���� �������� �̵�
        transform.position = resetPos.position;
        transform.rotation = resetPos.rotation;

        // ���� �ӵ� ���ֱ�
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    private Vector3 AirResist() // �������װ��� ����
    {
        float velocityMag = rigid.velocity.magnitude;
        Vector3 velocityDir = rigid.velocity.normalized;
        float airDragMag = dragConst * Mathf.Pow(velocityMag, 2f);

        return airDragMag * -velocityDir;
    }

    private Vector3 MagnusForce() // ���״����� ���� ����
    {
        Vector3 magnusForce = magnusConst * Vector3.Cross(rigid.velocity, rigid.angularVelocity);

        return magnusForce;
    }
}
