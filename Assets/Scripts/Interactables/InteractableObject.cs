using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public int m_scoreForRebuild = 5;
    public float m_secondsToRebuild = 5f;
    public float m_timeTillBreaksDown = 10f;

    private float m_secondsRepairingFor = 0f;

    private bool m_isRepaired = false;
    private GameObject m_canvasImage = null;
    private RectTransform m_progressBarScale = null;
    private Color m_startingColor;
    private float m_timeTillBreaking = 0.0f;
    
    [SerializeField]
    private GameObject m_sparksEffect = null;

    public void SetUp(GameObject canvas)
    {
        m_canvasImage = canvas;
        m_progressBarScale = m_canvasImage.transform.Find("FillImage") as RectTransform;
        m_canvasImage.SetActive(false);

        m_startingColor = GetComponent<MeshRenderer>().material.color;
    }

    private void TurnOnSparkEffect(bool on)
    {
        if(m_sparksEffect != null)
        {
            m_sparksEffect.SetActive(on);
        }
    }

    public void UpdateInteractable()
    {
        if(IsRepairedAlready())
        {
            m_timeTillBreaking -= Time.deltaTime;
            if(m_timeTillBreaking <= 0f)
            {
                m_isRepaired = false;
                GetComponent<MeshRenderer>().material.color = m_startingColor;
                TurnOnSparkEffect(true);
            }
        }
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
            m_timeTillBreaking = m_timeTillBreaksDown;
            TurnOnSparkEffect(false);
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
