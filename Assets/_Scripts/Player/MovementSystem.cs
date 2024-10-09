using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MovementSystem : MonoBehaviour
{
    public event Action OnStartMoving;
    public event Action OnStopMoving;
    public event Action OnFlipped;

    [SerializeField] private float moveSpeed = 20f;
    private bool isMoving;
    private List<Vector2Int> targetGridPositionList;
    private int index;
    private Vector2 currentTargetPosition;

    private Action onMoveCompleted;

    private int facingDirection = 1;
    private bool facingRight = true;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = moveSpeed;
    }

    private bool wasMoving;

    private void Update()
    {
        bool isMoving = agent.velocity.sqrMagnitude > 0.1f; 

        if (isMoving && !wasMoving)
        {
            OnStartMoving?.Invoke(); 
        }
        else if (!isMoving && wasMoving)
        {
            OnStopMoving?.Invoke(); 
            onMoveCompleted?.Invoke();
        }

        wasMoving = isMoving;

        HandleFlip();
    }

    public void Move(Vector2 targetPosition, Action onMoveCompleted)
    {
        this.onMoveCompleted = onMoveCompleted;
        currentTargetPosition = targetPosition;

        agent.SetDestination(targetPosition);
    }

   
    private void HandleFlip()
    {
        if (facingRight && currentTargetPosition.x < transform.position.x)
        {
            Flip();
        }

        if (!facingRight && currentTargetPosition.x > transform.position.x)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection = -facingDirection;
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        OnFlipped?.Invoke();
    }
}
