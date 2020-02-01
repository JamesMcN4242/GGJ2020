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

    public void UpdateInteractableItems()
    {
        foreach(InteractableObject obj in m_interactableObjects)
        {
            obj.UpdateInteractable();
        }
    }

    private void RetrieveAndSetUpAllObjects()
    {
        var objList = GameObject.FindGameObjectsWithTag("Interactable");
        foreach(var gameObj in objList)
        {
            var interactObj = gameObj.GetComponent<InteractableObject>();
            if(interactObj == null)
            {
               interactObj = gameObj.AddComponent<InteractableObject>();
            }
            m_interactableObjects.Add(interactObj);
            var canvas = GameObject.Instantiate(GetFillPrefab(),interactObj.transform);
            interactObj.SetUp(canvas);
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
