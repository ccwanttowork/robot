using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleAiInput : MonoBehaviour, IInput
{
    // Start is called before the first frame update
    public Action<Vector3> OnMovementDirectionInput { get; set; }
    public Action<Vector2> OnMovementInput { get; set; }



    //����������ƶ�
    //bull������
    bool playerDetectionResult = false;   //��ʼδ�������
    public Transform eyesTransform;                 //�����任�۾�
    public Transform playerTransform;
    public LayerMask playerLayer;             //����ɰ�
    public float visionDistance, stoppingDistance = 1.2f;                      //�����Ӿ�����

    //������λ��
    private void OnDrawGizmos()   //���ӻ������Ӿ�
    {
        Gizmos.color = Color.green;
        if(playerDetectionResult)
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(eyesTransform.position,visionDistance);        //��������������(�۾��任λ�õľ���

    }
    private void Update()
    {
        playerDetectionResult = DetectPlayer();
        if(playerDetectionResult) 
        {
            var directionToPlayer = playerTransform.position - transform.position;   //���㳯����ҷ���
            directionToPlayer = Vector3.Scale(directionToPlayer, Vector3.forward + Vector3.right);                   //Ͷ�䵽xzƽ����
            if(directionToPlayer.magnitude > stoppingDistance)  //�������>ֹͣ����
            {
                directionToPlayer.Normalize();  //��׼��
                OnMovementInput?.Invoke(Vector2.up);
                OnMovementDirectionInput?.Invoke(directionToPlayer);
                return;
            }
        }
        OnMovementInput?.Invoke(Vector2.zero);
        OnMovementDirectionInput?.Invoke(transform.forward);
    }

    private bool DetectPlayer()
    {
        Collider[] hitCollider = Physics.OverlapSphere(eyesTransform.position, visionDistance, playerLayer);
        foreach(var collider in hitCollider) 
        {
            playerTransform= collider.transform;
            return true;
        }
        playerTransform = null;
        return false;
    }
}
