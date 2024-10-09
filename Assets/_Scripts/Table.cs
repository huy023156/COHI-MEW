using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TableState
{
    locked,
    empty,
    hasCustomer,
    serving,
    dirty
}

public class Table : MonoBehaviour
{
    public static event Action<Table> OnAnyTalbeUnlocked;
    public event Action<TableState> OnTableStateChanged;

    [SerializeField] private ProgressBarUI progressBarUI;
    [SerializeField] private Transform lockTransform;
    [SerializeField] private Transform dirtyTransform;
    [SerializeField] private MoneyZone moneyZone;
    [SerializeField] private int unlockPrice = 100;
    [SerializeField] private float cleanTime = 3f;
    private bool isCleaning = false;

    private TableState currentState;
    private Customer customer;
    private Transform itemTransform;

    private float timerMax;
    private float timer;

    private void Awake()
    {
        ChangeState(TableState.locked);
    }

    private void Update()
    {
        switch (currentState)
        {
            case TableState.locked:
                break;
            case TableState.empty:
                break;
            case TableState.hasCustomer:
                break;
            case TableState.serving:
                timer -= Time.deltaTime;
                progressBarUI.SetValue(1 - timer / timerMax);

                if (timer < 0)
                {
                    ChangeState(TableState.dirty);
                    Destroy(itemTransform.gameObject);
                    customer.ChangeState(CustomerState.leaving);
                    moneyZone.SpawnMoney(customer.GetOrder().rewardAmount);
                    customer = null;
                    progressBarUI.Hide();
                }
                break;
            case TableState.dirty:
                if (isCleaning)
                {
                    timer -= Time.deltaTime;
                    progressBarUI.SetValue(1 - timer / timerMax);

                    if (timer < 0)
                    {
                        ChangeState(TableState.empty);
                        isCleaning = false;
                        progressBarUI.Hide();
                        SoundManager.Instance.PlayClearTable();
                    }
                }
                break;
        }
    }

    public void OnMouseDown()
    {
        switch (currentState)
        {
            case TableState.locked:
                if (ResourceManager.Instance.TrySpendMoney(unlockPrice))
                {
                    lockTransform.gameObject.SetActive(false);
                    ChangeState(TableState.empty);
                    OnAnyTalbeUnlocked?.Invoke(this);
                }
                break;
            case TableState.empty:
                break;
            case TableState.hasCustomer:
                PlayerManager.Instance.GetPlayer().GetMovementSystem().Move(transform.position, null);
                break;
            case TableState.dirty:
                PlayerManager.Instance.GetPlayer().GetMovementSystem().Move(transform.position, null);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (currentState)
        {
            case TableState.locked:
                break;
            case TableState.empty:
                break;
            case TableState.hasCustomer:
                if (other.TryGetComponent<Player>(out Player player))
                {
                    if (player.TryTakeItemSO(customer.GetOrder().item, out itemTransform))
                    {
                        itemTransform.parent = transform;
                        itemTransform.localPosition = Vector2.zero;
                        customer.ChangeState(CustomerState.consuming);
                        ChangeState(TableState.serving);

                        timerMax = customer.GetOrder().item.consumeTime;
                        timer = timerMax;
                        progressBarUI.Show();
                        SoundManager.Instance.PlayBonk();
                        player.BatHit();
                    }
                }
                break;
            case TableState.dirty:
                if (other.TryGetComponent<Player>(out player))
                {
                    isCleaning = true;
                    timerMax = cleanTime;
                    timer = cleanTime;
                    progressBarUI.Show();
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (currentState)
        {
            case TableState.dirty:
                if (collision.TryGetComponent<Player>(out Player player))
                {
                    isCleaning = false;
                    progressBarUI.Hide();
                }
                break;
        }
    }

    public bool IsLocked() => currentState == TableState.locked;

    public bool IsEmpty() => customer == null && currentState == TableState.empty;

    public void SetCustomer(Customer customer) => this.customer = customer;

    public void ChangeState(TableState state)
    {
        if (state == TableState.dirty)
        {
            dirtyTransform.gameObject.SetActive(true);
        } else
        {
            dirtyTransform.gameObject.SetActive(false);
        }


        currentState = state;
        OnTableStateChanged?.Invoke(currentState);
    }
}
