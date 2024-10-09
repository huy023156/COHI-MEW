using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource collectMoney;
    [SerializeField] private AudioSource bonk;
    [SerializeField] private AudioSource itemGenerated;
    [SerializeField] private AudioSource clearTable;

    public void PlayCollectMoneySound()
    {
        collectMoney.Play();
    }

    public void PlayBonk()
    {
        bonk.Play();
    }

    public void PlayItemGeneratedSound()
    {
        itemGenerated.Play();
    }

    public void PlayClearTable()
    {
        clearTable.Play();
    }
}
