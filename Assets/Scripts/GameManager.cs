﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // graffiti
    public Text uiText;

    //states
    enum State { NotStarted, Playing, GameOver, WonGame }

    // current state
    State currState;

    // Enemy Manager
    EnemyManager enemyManager;
    // Start is called before the first frame update

    AudioSource audio;

    void Start()
    {
        // start as not playing
        currState = State.NotStarted;

        // refresh UI
        RefreshUI();

        // find the enemy manager
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        audio = uiText.GetComponent<AudioSource> ();

        // log error if it wasn't found
        if(enemyManager == null)
        {
            Debug.LogError("there needs to be an EnemyManager in the scene");
        }
    }

    void RefreshUI()
    {
        // act according to the state
        switch(currState)
        {
            case State.NotStarted:
                uiText.text = "Shoot here to begin";
                break;

            case State.Playing:
                uiText.text = "Enemies left: " + enemyManager.numEnemies;
                break;

            case State.GameOver:
                uiText.text = "Game Over! Shoot here";
                break;

            case State.WonGame:
                uiText.text = "YOU WON! Shoot here";
                break;
        }  
    }

    public void InitGame()
    {
        //don't initiate the game if the game is already running!
        if (currState == State.Playing) return;

        // set the state
        currState = State.Playing;
        audio.Play();

        // create enemy wave
        enemyManager.CreateEnemyWave();

        // show text on the graffiti
        RefreshUI();
    }

    // game over
    public void GameOver()
    {
        // do nothing if we were already on game over
        if (currState == State.GameOver) return;

        // set the state to game over
        currState = State.GameOver;
        audio.Stop();

        // show text on the graffiti
        RefreshUI();

        // remove all enemies
        enemyManager.KillAll();
    }

    // checks whether we've won, and if we did win, refresh UI
    public void HandleEnemyDead()
    {
        if (currState != State.Playing) return;

        RefreshUI();

        // have we won the game?
        if(enemyManager.numEnemies <= 0)
        {
            // set the state of the game
            currState = State.WonGame;

            // show text on the graffiti
            RefreshUI();

            // remove all enemies
            enemyManager.KillAll();
        }
    }
}
