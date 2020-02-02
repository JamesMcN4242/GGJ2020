using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuMover : MonoBehaviour
{
    public string m_nextScene = "GameScene";

    public void MoveToScene()
    {
        SceneManager.LoadScene(m_nextScene);
    }
}
