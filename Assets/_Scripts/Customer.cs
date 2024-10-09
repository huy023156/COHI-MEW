using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public enum CustomerState
{
    walkingToChair,
    waitingForServing,
    consuming,
    leaving
}

public class Customer : MonoBehaviour
{
    [Header("Order info")]
    private Order order;
    private Table table;

    private Vector2 targetPos;
    private bool isWorking;
    private bool isWaiting;

    private CustomerState currentState;

    private MovementSystem movementSystem;

    [SerializeField] private ProgressBarUI progressBarUI;
    [SerializeField] private Transform itemIconTransform;

    private float timer;

    private void Awake()
    {
        movementSystem = GetComponent<MovementSystem>();
        ChangeState(CustomerState.walkingToChair);
    }

    private void Start()
    {
        movementSystem.OnFlipped += MovementSystem_OnFlipped;
        table.OnTableStateChanged += Table_OnTableStateChanged;
    }

    private void Table_OnTableStateChanged(TableState state)
    {
        if (state == TableState.serving)
        {
            progressBarUI.Hide();
        }
    }

    public void SetUp(Order order, Table table)
    {
        this.order = order;
        this.table = table;
        table.ChangeState(TableState.hasCustomer);
        table.SetCustomer(this);

        isWorking = true;
        
        targetPos = FindTargetPositionNearTable();

        movementSystem.Move(targetPos, ChangeToWaitingForServingState);

        Instantiate(order.item.itemPrefab, itemIconTransform);
        itemIconTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isWorking)
        {
            return;
        }

        HandleState();
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case CustomerState.walkingToChair:
                break;
            case CustomerState.waitingForServing:
                if (!isWaiting)
                {
                    return;
                }

                timer -= Time.deltaTime;
                progressBarUI.SetValue(1 - timer / order.timeLimit);

                if (timer < 0)
                {
                    isWaiting = false;
                    ChangeState(CustomerState.leaving);
                    table.ChangeState(TableState.empty);
                    progressBarUI.Hide();
                }
                break;
            case CustomerState.consuming:
                break;
            case CustomerState.leaving:
                movementSystem.Move(CustomerManager.Instance.GetSpawnPosition(), DestroySelf);
                break;
        }
    }

    private void ChangeToWaitingForServingState()
    {
        ChangeState(CustomerState.waitingForServing);
        timer = order.timeLimit;
        isWaiting = true;
        progressBarUI.Show();
        itemIconTransform.gameObject.SetActive(true);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private Vector2 FindTargetPositionNearTable()
    {
        Vector2 targetPosition = table.transform.position;

        Vector2 customerDir;
        if (transform.position.y > table.transform.position.y)
        {
            customerDir = Vector2.down;
        }
        else
        {
            customerDir = Vector2.up;
        }

        float gapCheck = 0.1f;
        while (true)
        {
            targetPosition = targetPosition + customerDir * gapCheck;

            float maxDistance = 1.0f;
            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
            {
                break;
            }
        }

        return targetPosition;
    }

    public void ChangeState(CustomerState state)
    {
        if (state == CustomerState.leaving)
        {
            itemIconTransform.gameObject.SetActive(false);
        }

        currentState = state;
    }

    private void MovementSystem_OnFlipped()
    {
        progressBarUI.transform.Rotate(new Vector3(0, 180, 0));
    }

    public Order GetOrder() => order;
}
