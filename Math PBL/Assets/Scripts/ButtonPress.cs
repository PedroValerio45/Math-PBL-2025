using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public GameObject button;
    private Animator anim;
    private bool animIsPlaying;

    void Start()
    {
        anim = button.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !animIsPlaying)
        {
            anim.Play("ButtonCylinder|Cylinder.003Action");
            animIsPlaying = true;
        }
    }

    public void AnimFinish()
    {
        animIsPlaying = false;
    }
}
