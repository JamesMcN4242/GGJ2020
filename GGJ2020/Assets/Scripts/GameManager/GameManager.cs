﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int k_maxPlayers = 2;

    [SerializeField]
    private Vector3[] m_playerStartingPositions = new Vector3[]
    {
        new Vector3(2f, 0f, -2f),
        new Vector3(-2f, 0f, -2f)
    };
    private PlayerUpdateManager m_playerManager = null;

    // Start is called before the first frame update
    void Start()
    {
        m_playerManager = new PlayerUpdateManager();

        //TODO: Multiple player choice
        for(int i = 0; i < k_maxPlayers; i++)
        {
            m_playerManager.RegisterNewPlayer(m_playerStartingPositions[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_playerManager.UpdateAllPlayers();
    }
}