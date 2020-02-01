using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private Text[] m_playerScoreTexts = new Text[2];

    [SerializeField]
    private Text m_timerText = null;

    public void UpdatePlayerScore(int playerIndex, int score)    
    {
        m_playerScoreTexts[playerIndex].text = string.Format("Player {0} Score: {1}", playerIndex + 1, score);
    }

    public void UpdateGameTimer(int secondsRemaining)
    {
        m_timerText.text = string.Format("Time Remaining: {0}s", secondsRemaining);
    }
}
