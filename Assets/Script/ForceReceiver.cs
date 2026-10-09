using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float drag = 0.3f;

    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocaity;
    public Vector3 Movement => impact + Vector3.up * verticalVelocaity;

    void Update()
    {
        if (verticalVelocaity < 0f && controller.isGrounded)
        {
            verticalVelocaity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocaity += Physics.gravity.y * Time.deltaTime;
        }
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (agent != null)
        {
            if (impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                agent.enabled = true;
            }
        }

    }
    public void Rest()
    {
        impact = Vector3.zero;
        verticalVelocaity = 0f;
    }
    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null)
        {
            agent.enabled = false;
        }

    }

    public void Jump(float jumpForce)
    {
        verticalVelocaity += jumpForce;

    }
}
