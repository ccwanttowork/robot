using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    IInput input;
    AgentMovement movement;

    private void OnEnable()
    {
        input = GetComponent<IInput>();
        movement = GetComponent<AgentMovement>();
        //input.OnMovementDirectionInput += (input) => { Debug.Log("Direction" + input); };//µ÷ÊÔÓï¾ä
        input.OnMovementDirectionInput += movement.HandleMovementDirection;
        //input.OnMovementInput += (input) => { Debug.Log("Movement input" + input); };
        input.OnMovementInput += movement.HandleMovement;
    }
    private void OnDisable()
    {
        input.OnMovementDirectionInput -= movement.HandleMovementDirection;
        input.OnMovementInput -= movement.HandleMovement;
    }

}
