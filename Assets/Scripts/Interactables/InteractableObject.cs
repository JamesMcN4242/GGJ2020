using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public int m_scoreForRebuild = 5;
    public float m_secondsToRebuild = 5f;

    private float m_secondsRepairingFor = 0f;

    private bool m_isRepaired = false;
    private GameObject m_canvasImage = null;
    private RectTransform m_progressBarScale = null;
    private Color m_startingColor;

    public void SetUp(GameObject canvas)
    {
        m_canvasImage = canvas;
        m_progressBarScale = m_canvasImage.transform.Find("FillImage") as RectTransform;
        m_canvasImage.SetActive(false);

        m_startingColor = GetComponent<MeshRenderer>().material.color;
    }

    public bool UpdateRepairTiming(float timeTakenOff)
    {
        m_secondsRepairingFor += timeTakenOff;
        m_isRepaired = m_secondsRepairingFor >= m_secondsToRebuild;

        if(m_isRepaired)
        {
            //TODO: Some ceremony boy
            m_canvasImage.SetActive(false);
            GetComponent<MeshRenderer>().material.color = Color.gray;
        }
        else
        {
            m_progressBarScale.anchorMax = new Vector2(m_secondsRepairingFor/m_secondsToRebuild, 1f);
            m_progressBarScale.offsetMax = new Vector2(0f, m_progressBarScale.offsetMax.y);
        }

        return m_isRepaired;
    }

    public bool IsRepairedAlready()
    {
        return m_isRepaired;
    }

    public void SetInteractedWith(bool interactingWith)
    {
        m_secondsRepairingFor = 0f;
        m_progressBarScale.anchorMax = new Vector2(m_secondsRepairingFor/m_secondsToRebuild, 1f);
        m_canvasImage.SetActive(interactingWith);
    }
}
