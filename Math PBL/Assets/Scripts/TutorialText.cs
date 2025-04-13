using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialText : MonoBehaviour
{
    public ButtonPress buttonScript;
    public Canvas canvas;

    void Update()
    {
        if (ButtonPress.buttonIsPressed)
        {
            Destroy(canvas.gameObject);
        }
    }
}
