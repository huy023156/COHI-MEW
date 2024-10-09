using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState 
{ 
    waitingPlayerToOpenTable,
    playing
}

public class GameManager : Singleton<GameManager>
{
    private GameState currentState;

    private float difficultyMultiplier = 1;

    protected override void Awake()
    {
        base.Awake();
        currentState = GameState.waitingPlayerToOpenTable;
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.waitingPlayerToOpenTable:
                if (TableManager.Instance.IsAnyTableUnlocked())
                {
                    currentState = GameState.playing;
                }
                break;
            case GameState.playing:
                break;
        }

        HandleDifficultyMultiplier();
    }

    private void HandleDifficultyMultiplier()
    {
        if (Time.time / 60 < 1)
        {
            difficultyMultiplier = 1.0f;
        }
        else if (Time.time / 60 < 7)
        {
            difficultyMultiplier = (int)Time.time / 60 * .1f + 1;
        }
        else
        {
            difficultyMultiplier = 1.7f;
        }
    }

    public float GetCurrentDifficultyMultiplier()
    {
        return difficultyMultiplier;
    }

    public GameState GetCurrentState() => currentState;
}