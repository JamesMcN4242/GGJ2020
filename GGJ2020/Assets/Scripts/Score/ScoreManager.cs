using System.Collections.Generic;

public class ScoreManager
{
    private Dictionary<PlayerController, int> m_playerScores = new Dictionary<PlayerController, int>();
    
    public int GetScoreForPlayer(PlayerController player)
    {
        return m_playerScores.ContainsKey(player) ? m_playerScores[player] : 0;
    }

    public void AddOrUpdatePlayerScore(PlayerController player, int scoreToAdd)
    {
        m_playerScores[player] = GetScoreForPlayer(player) + scoreToAdd;
    }
}
