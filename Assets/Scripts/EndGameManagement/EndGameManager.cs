using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    private const string k_playerPrefabName = "Player{0}EndPodium";

    public Transform m_winnerTransform = null;
    public Transform m_loserTransform = null;
    public GameObject m_leaveButton = null;

    private Light m_light;
    private bool m_tPosingStarted = false;
    private TPoseAnimator m_winnerPlayer;

    // Start is called before the first frame update
    void Start()
    {
        m_light = GetComponent<Light>();
        m_winnerPlayer = GameObject.Instantiate(GetPrefabObject(WinnerTracker.m_winnerPlayer), m_winnerTransform).GetComponent<TPoseAnimator>();
        GameObject.Instantiate(GetPrefabObject(WinnerTracker.m_loserPlayer), m_loserTransform);
    }

    void Update()
    {
        if(m_light.spotAngle < 75f)
        {
            m_light.spotAngle += (Time.deltaTime * 15f);
        }
        else if (m_tPosingStarted == false)
        {
            m_tPosingStarted = true;
            m_winnerPlayer.PlayTPose();
        }
        else
        {
            m_leaveButton.SetActive(m_winnerPlayer.AnimationComplete());
        }
    }

    private GameObject GetPrefabObject(int player)
    {
        return Resources.Load<GameObject>(string.Format(k_playerPrefabName, player));
    }
}
