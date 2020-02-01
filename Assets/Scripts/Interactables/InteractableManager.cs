using System.Collections.Generic;
using UnityEngine;

public class InteractableManager
{
    private GameObject m_fillPrefab = null;
    private List<InteractableObject> m_interactableObjects = new List<InteractableObject>();

    public InteractableManager()
    {
        RetrieveAndSetUpAllObjects();
    }

    private void RetrieveAndSetUpAllObjects()
    {
        var objList = GameObject.FindGameObjectsWithTag("Interactable");
        foreach(var gameObj in objList)
        {
            var interactObj = gameObj.AddComponent<InteractableObject>();
            var canvas = GameObject.Instantiate(GetFillPrefab(),interactObj.transform);
            interactObj.SetCanvasImage(canvas);
        }
    }

    private GameObject GetFillPrefab()
    {
        if(m_fillPrefab == null)
        {
            m_fillPrefab = Resources.Load<GameObject>("FillBar");
        }
        return m_fillPrefab;
    }
}
