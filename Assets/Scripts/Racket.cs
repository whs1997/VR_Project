using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;
using UnityEngine.XR.OpenXR.Input;

public class Racket : MonoBehaviour
{
    [SerializeField] float power = 1f; // ���� ���� ��
    private Vector3 preBallPos; // ���� ������������ ��ġ

    private RaycastHit hit;

    [SerializeField] Transform resetPos; // ���� �����Ҷ� ������ ��ġ

    private void FixedUpdate()
    {
        RaycastBall();
    }

    private void RaycastBall() // ���� �������� raycast�� ������ �޾ƿ�
    {
        if (preBallPos != null)
        {
            // ���� ���� ��ġ���� ���� ��ġ�� Raycast
            Vector3 hitDir = (preBallPos - transform.position).normalized; // ���� ���� ����

            // distance : �� �����ӵ��� ������ ���� �Ÿ� ����� Ray�� ���� ����
            if (Physics.Raycast(transform.position, hitDir, out hit, Vector3.Distance(preBallPos, transform.position)))
            {
                // ���� ������
                if (hit.transform.gameObject.tag == "Ball")
                {                    
                    Rigidbody ballRigid = hit.transform.GetComponent<Rigidbody>();

                    Vector3 racketVelocity = GetComponent<Rigidbody>().velocity;

                    // ���� ���� ��(������ �ӵ��� ���� ���� ����)
                    Vector3 hitForce = (racketVelocity + hitDir) * power;

                    ballRigid.AddForce(hitForce, ForceMode.Impulse);
                }
            }
        }
        preBallPos = transform.position; // ���� ��ġ ����
    }

    public void ResetRacket()
    {
        gameObject.SetActive(true);
        transform.position = resetPos.position;
        transform.rotation = resetPos.rotation;
    }
}