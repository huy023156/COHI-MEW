using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private ProgressBarUI progressBarUI;

    private float timerMax;
    private float timer;
    private bool isNearPlayer = false;
    private Player player;

    private void Awake()
    {
    }

    private void Start()
    {
        timer = itemSO.generateTime * GameManager.Instance.GetCurrentDifficultyMultiplier();
    }

    private void Update()
    {
        if (!isNearPlayer)
        {
            return;
        }

        if (!player.CanHoldMoreItem())
        {
            return;
        }

        timer -= Time.deltaTime;
        progressBarUI.SetValue(1 - timer / timerMax);

        if (timer < 0)
        {
            player.AddHoldingItemSO(itemSO);
            isNearPlayer = false;
            progressBarUI.Hide();
            SoundManager.Instance.PlayItemGeneratedSound();
        }
    }

    private void OnMouseDown()
    {
        PlayerManager.Instance.GetPlayer().GetMovementSystem().Move(transform.position, null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player) && player.CanHoldMoreItem())
        {
            isNearPlayer = true;
            timerMax = itemSO.generateTime * GameManager.Instance.GetCurrentDifficultyMultiplier();
            timer = timerMax;
            this.player = player;
            progressBarUI.Show();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            isNearPlayer = false;
            this.player = null;
            progressBarUI.Hide();
        }
    }

}
