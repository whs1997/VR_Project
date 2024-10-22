using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR.Interaction.Toolkit;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float maxSpeed; // ���� �ִ� �ӵ�

    [SerializeField] Transform resetPos; // ���� �������ų� ���ӽ����Ҷ� ����ġ

    [SerializeField] float dragConst = 0.005f; // ���������� ���
    [SerializeField] float magnusConst = 0.005f; // ���״���ȿ���� ��� ( ���� ������ ���� �� �� )

    [SerializeField] GameManager gameManager;
    private bool isPlayerSide = false; // ���� ����, ����ʿ� �ִ��� Ȯ��
    private int bounceCount = 0;

    private void Start()
    {
        rigid.isKinematic = true; // ���� ��Ʈ�ѷ��� ���� ������ �������� �ʰ� 
    }

    private void Update()
    {
        if (rigid.velocity.magnitude > maxSpeed) // ���� �ʹ����������ʰ� �ִ�ӵ� ����
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }

        if (transform.position.y < -5) // Ȥ�� ���� �ٴ��� �հ� �������� ����
        {
            ResetBall();
        }

        if (transform.position.z < 0)
        {
            isPlayerSide = true; // ���� z ��ġ�� 0 �Ʒ��� ����
        }
        else
        {
            isPlayerSide = false; // z�� 0 �̻��̸� �����
        }
    }

    private void FixedUpdate()
    {
        // ���� ���׿� ���� ���� ������ fixedupdate
        rigid.AddRelativeForce(MagnusForce());
        rigid.AddRelativeForce(AirResist());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Net")
        {
            if (isPlayerSide)
            {
                gameManager.AddEnemyScore(); // ���ʿ��� ��Ʈ, �ٴڿ� �ɸ��� ��밡 ����
                ResetBall();
            }
            else
            {
                gameManager.AddPlayerScore(); // ����ʿ��� �ɸ��� ����
                ResetBall();
            }
        }

        if (collision.gameObject.tag == "Table")
        {
            if (isPlayerSide) // ���� ���� Ź���뿡�� ƨ���
            {
                bounceCount++;
                if (bounceCount >= 2) // �ι� ƨ���
                {
                    gameManager.AddEnemyScore(); // ���� ����
                    ResetBall();
                }
            }
            else if (!isPlayerSide)
            {
                bounceCount++;
                if (bounceCount >= 2)
                {
                    gameManager.AddPlayerScore(); // ����ʿ��� �ι� ƨ��� ���� ����
                    ResetBall();
                }
            }
        }
    }

    private void ResetBall()
    {
        // ���� �������� �̵�
        transform.position = resetPos.position;
        transform.rotation = resetPos.rotation;

        rigid.isKinematic = true; // ��Ʈ�ѷ��� ���� ������ �������� ����

        // ���� �ӵ� ���ֱ� 
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        bounceCount = 0;
    }
    /*
    public void Grabbed(SelectEnterEventArgs args)
    {
        rigid.isKinematic = false;
        Debug.Log("�� ����");
    }
    */
    public void Released(SelectExitEventArgs args)
    {
        rigid.isKinematic = false; // ��Ʈ�ѷ��� ��Ҵٰ� ���� ������
        // Debug.Log("�� ����");
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
