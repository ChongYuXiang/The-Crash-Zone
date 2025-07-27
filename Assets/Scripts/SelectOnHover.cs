using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnHover : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    private static bool firstSelectionIgnored = false;
    public static bool playNextAudio = true;

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void dontPlayNext()
    {
        playNextAudio = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!firstSelectionIgnored)
        {
            // Skip the first automatic selection
            firstSelectionIgnored = true;
            return;
        }
        if (playNextAudio)
        {
            AudioManager.instance.PlaySFX("ButtonSelect");
        }
        if (!playNextAudio)
        {
            playNextAudio = true;
        }
    }
}
