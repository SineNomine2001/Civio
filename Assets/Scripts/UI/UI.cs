using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public virtual void ToggleVisibility()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.enabled = !canvas.enabled;
    }

    public virtual void Show()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public virtual void Hide()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
