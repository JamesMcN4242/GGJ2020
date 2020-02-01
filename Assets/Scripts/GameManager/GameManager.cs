using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int k_maxPlayers = 2;
    [SerializeField]
    private GameUI m_gameUI = null;

    [SerializeField]
    private Vector3[] m_playerStartingPositions = new Vector3[]
    {
        new Vector3(2f, 0f, -2f),
        new Vector3(-2f, 0f, -2f)
    };

    [SerializeField]
    private Color[] m_playerColours = new Color[]
    {
        Color.red,
        Color.blue
    };

    private PlayerUpdateManager m_playerManager = null;
    private ScoreManager m_scoreManager = null;
    private InteractableManager m_interactManager = null;

    [SerializeField]
    private float m_secondsRemaining = 300f;

    // Start is called before the first frame update
    void Start()
    {
        m_playerManager = new PlayerUpdateManager();

        for(int i = 0; i < k_maxPlayers; i++)
        {
            m_playerManager.RegisterNewPlayer(m_playerStartingPositions[i], m_playerColours[i]);
        }

        m_scoreManager = new ScoreManager();
        m_interactManager = new InteractableManager();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_secondsRemaining > 0f)
        {
            m_secondsRemaining -= Time.deltaTime;
            m_gameUI.UpdateGameTimer((int)m_secondsRemaining);
            m_interactManager.UpdateInteractableItems();

            var players = m_playerManager.GetRegisteredPlayers();
            for(int i = 0; i < players.Count; i++)
            {
                m_scoreManager.AddOrUpdatePlayerScore(players[i], m_playerManager.ConsumeScoreForPlayer(i));
                m_gameUI.UpdatePlayerScore(i, m_scoreManager.GetScoreForPlayer(players[i]));
            }

            m_playerManager.UpdateAllPlayers();
        }
    }



}
