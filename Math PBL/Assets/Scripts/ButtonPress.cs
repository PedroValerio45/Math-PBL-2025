using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public GameObject button;
    private Animator anim;
    private bool animIsPlaying;
    public static bool buttonIsPressed;

    void Start()
    {
        anim = button.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && !animIsPlaying)
        {
            anim.SetBool("isPlaying", true);
            animIsPlaying = true;
            buttonIsPressed = true;
            
            Debug.Log(buttonIsPressed);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            anim.SetBool("isPlaying", false);
            buttonIsPressed = false;
            
            Debug.Log(buttonIsPressed);
        }

        // Reset scene because why not
        if (Input.GetKeyDown(KeyCode.R)) { Application.LoadLevel(Application.loadedLevel); }
    }

    public void AnimFinish()
    {
        anim.SetBool("isPlaying", false);
        animIsPlaying = false;
    }
}
