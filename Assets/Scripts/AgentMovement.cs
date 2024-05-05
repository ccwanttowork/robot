using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    CharacterController controller;
    Animator animator;


    public float rotationSpeed, movementSpeed, gravity = 20;   //应用于y轴移动
    Vector3 movementVector=Vector3.zero;    //移动角色的向量
    private float desiredRotationAngle = 0;                            //私人旋转角度

    private float inputVerticalDirection    = 0;

    private void Start()
    {
        controller=GetComponent<CharacterController>();
        animator=GetComponent<Animator>();
    }
    public void HandleMovement(Vector2 input)   //分配给运动输入
    {
        if(controller.isGrounded)   //跳跃或坠落——检查输入
        {
            if(input.y!=0)
            {
                if(input.y>0)
                {
                    inputVerticalDirection=Mathf.CeilToInt(input.y);
                }
                else
                {
                    inputVerticalDirection=Mathf.FloorToInt(input.y);
                }
                movementVector =input.y*transform.forward * movementSpeed;  //向后或向前
            }
            else
            {
                movementVector = Vector3.zero;
                animator.SetFloat("move", 0);
            }
        }
    }
    public void HandleMovementDirection(Vector3 direction)
    {
        desiredRotationAngle=Vector3.Angle(transform.forward, direction);
        var crossProduct=Vector3.Cross(transform.forward, direction).y;
        if(crossProduct<0)
        {
            desiredRotationAngle *=-1;
        }
    }
    private void RotateAgent()
    {
        if(desiredRotationAngle>10||desiredRotationAngle<-10)
        {
            transform.Rotate(Vector3.up*desiredRotationAngle*rotationSpeed*Time.deltaTime);
        }
    }

    private float SetCorrectAnimation(float inputVerticalDirection)
    {
        float currentAnimationSpeed = animator.GetFloat("move");
        if(desiredRotationAngle>10||desiredRotationAngle<-10)
        {
            if(Mathf.Abs(currentAnimationSpeed)<0.2f)
            {
                currentAnimationSpeed +=inputVerticalDirection* Time.deltaTime * 2;
                currentAnimationSpeed = Mathf.Clamp(currentAnimationSpeed, -0.2f, 0.2f);
            }
            animator.SetFloat("move", currentAnimationSpeed);
        }
        else
        {
            if(currentAnimationSpeed<1)
            {
                currentAnimationSpeed +=inputVerticalDirection* Time.deltaTime * 2;
            }
           
            animator.SetFloat("move", Mathf.Clamp(currentAnimationSpeed,-1,1));
        }
        return Mathf.Abs(currentAnimationSpeed);
    }



    private void Update()    //负责移动角色控制器
    {
        if(controller.isGrounded) 
        {
            if(movementVector.magnitude>0)
            {
                var animationSpeedMultiplier = SetCorrectAnimation(inputVerticalDirection);    //动画速度乘数
                RotateAgent();
                movementVector*=animationSpeedMultiplier;
            }
        }
        movementVector.y -= gravity;
        controller.Move(movementVector*Time.deltaTime);
    }
}
