using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyZone : MonoBehaviour
{
    private List<Transform> moneyTransform;
    private int rewardAmount;
    public bool hasMoney;

    public void SpawnMoney(int rewardAmount)
    {
        MoneyManager.Instance.SpawnMoney(transform, out moneyTransform);
        hasMoney = true;
        this.rewardAmount = rewardAmount;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (hasMoney)
        {
            SoundManager.Instance.PlayCollectMoneySound();
            StartCoroutine(MoveMoneyToPlayer());
            ResourceManager.Instance.AddMoney(rewardAmount);
        }
    }

    private IEnumerator MoveMoneyToPlayer()
    {
        hasMoney = false;

        foreach (Transform money in moneyTransform)
        {
            money.LeanMove(PlayerManager.Instance.GetPlayer().transform.position, 0.05f).destroyOnComplete = true;
            yield return new WaitForSeconds(.05f);
        }
    }
}
