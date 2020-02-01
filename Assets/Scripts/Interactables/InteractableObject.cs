using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public int m_scoreForRebuild = 5;
    public float m_secondsToRebuild = 5f;

    private float m_secondsRepairingFor = 0f;
    private GameObject m_canvasImage = null;
    private RectTransform m_progressBarScale = null;

    public void SetCanvasImage(GameObject canvas)
    {
        m_canvasImage = canvas;
        m_progressBarScale = m_canvasImage.transform.Find("FillImage") as RectTransform;
        m_canvasImage.SetActive(false);
    }

    public bool UpdateRepairTiming(float timeTakenOff)
    {
        m_secondsRepairingFor += timeTakenOff;
        bool repaired = m_secondsRepairingFor >= m_secondsToRebuild;

        if(repaired)
        {
            //TODO: Some ceremony boy
        }
        else
        {
            m_progressBarScale.anchorMax = new Vector2(m_secondsRepairingFor/m_secondsToRebuild, 1f);
            m_progressBarScale.offsetMax = new Vector2(0f, m_progressBarScale.offsetMax.y);
        }

        return repaired;
    }

    public void SetInteractedWith(bool interactingWith)
    {
        m_secondsRepairingFor = 0f;
        m_progressBarScale.anchorMax = new Vector2(m_secondsRepairingFor/m_secondsToRebuild, 1f);
        m_canvasImage.SetActive(interactingWith);
    }
}
