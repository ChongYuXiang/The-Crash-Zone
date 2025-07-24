using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JumpToUIElement : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable elementToSelect;

    private void Reset()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    public void JumpToElement()
    {
        eventSystem.SetSelectedGameObject(elementToSelect.gameObject);
    }
}
