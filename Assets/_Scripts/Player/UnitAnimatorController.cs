using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimatorController : MonoBehaviour
{
    private const string MOVE_STRING = "Move";

    [SerializeField] private MovementSystem movementSystem;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movementSystem.OnStartMoving += MovementSystem_OnStartMoving;
        movementSystem.OnStopMoving += MovementSystem_OnStopMoving;
    }

    private void MovementSystem_OnStopMoving()
    {
        animator.SetBool(MOVE_STRING, false);
    }

    private void MovementSystem_OnStartMoving()
    {
        animator.SetBool(MOVE_STRING, true);
    }
}
