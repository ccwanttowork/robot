using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerInput : MonoBehaviour, IInput
{

    public Action<Vector2> OnMovementInput { get; set; }
    public Action<Vector3> OnMovementDirectionInput { get; set; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   //光屏锁定在屏幕中央
    }
    private void Update()
    {
        GetMovementInput();
        GetMovementDirection();
    }

    private void GetMovementDirection()
    {
        var cameraForewardDirection = Camera.main.transform.forward;  //获得相机指向的前向方向
        Debug.DrawRay(Camera.main.transform.position, cameraForewardDirection * 10, Color.red);
        var directionToMoveIn = Vector3.Scale(cameraForewardDirection, (Vector3.right + Vector3.forward));
        Debug.DrawRay(Camera.main.transform.position, directionToMoveIn * 10, Color.blue);
        OnMovementDirectionInput?.Invoke(directionToMoveIn);
    }

    private void GetMovementInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        OnMovementInput?.Invoke(input);
    }
}
