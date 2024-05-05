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



    //代理向玩家移动
    //bull层检测结果
    bool playerDetectionResult = false;   //初始未看到玩家
    public Transform eyesTransform;                 //公共变换眼睛
    public Transform playerTransform;
    public LayerMask playerLayer;             //玩家蒙版
    public float visionDistance, stoppingDistance = 1.2f;                      //公共视觉距离

    //获得玩家位置
    private void OnDrawGizmos()   //可视化敌人视距
    {
        Gizmos.color = Color.green;
        if(playerDetectionResult)
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(eyesTransform.position,visionDistance);        //绘制是世界球体(眼睛变换位置的距离

    }
    private void Update()
    {
        playerDetectionResult = DetectPlayer();
        if(playerDetectionResult) 
        {
            var directionToPlayer = playerTransform.position - transform.position;   //计算朝向玩家方向
            directionToPlayer = Vector3.Scale(directionToPlayer, Vector3.forward + Vector3.right);                   //投射到xz平面上
            if(directionToPlayer.magnitude > stoppingDistance)  //方向幅度>停止距离
            {
                directionToPlayer.Normalize();  //标准化
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
