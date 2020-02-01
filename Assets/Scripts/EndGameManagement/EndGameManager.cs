using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    private const string k_playerPrefabName = "PlayerPrefab{0}";
    // Start is called before the first frame update
    void Start()
    {
        //TODO: Instantiate players
    }

    private GameObject GetPrefabObject(int playerIndex)
    {
        return Resources.Load<GameObject>(string.Format(k_playerPrefabName, playerIndex+1));
    }
}
