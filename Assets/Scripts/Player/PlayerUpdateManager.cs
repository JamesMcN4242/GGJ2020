using System.Collections.Generic;
using UnityEngine;

public class PlayerUpdateManager
{
    private const string k_playerPrefabName = "PlayerPrefab{0}";

    private static readonly List<KeyCode[]> k_playerKeyCodes = new List<KeyCode[]>()
    {
         new KeyCode[]{KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.E, KeyCode.Q},
         new KeyCode[]{KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.Period, KeyCode.Comma}
    };

    private List<PlayerController> m_players = new List<PlayerController>();
    private GameObject m_prefabObject = null;

    public void RegisterNewPlayer(Vector3 startPosition, int playerIndex)
    {
        GameObject player = GameObject.Instantiate(GetPrefabObject(playerIndex));
        player.transform.position = startPosition;
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.SetInputKeys(k_playerKeyCodes[m_players.Count]);
    
        m_players.Add(playerController);
    }   

    public List<PlayerController> GetRegisteredPlayers()
    {
        return m_players;
    }
    public void UpdateAllPlayers()
    {
        for(int i = 0; i < m_players.Count; i++)
        {
            m_players[i].UpdatePlayerControls();
        }
    }

    public int ConsumeScoreForPlayer(int index)
    {
        return m_players[index].ConsumeScore();
    }

    private GameObject GetPrefabObject(int playerIndex)
    {
        return Resources.Load<GameObject>(string.Format(k_playerPrefabName, playerIndex+1));
    }
}
