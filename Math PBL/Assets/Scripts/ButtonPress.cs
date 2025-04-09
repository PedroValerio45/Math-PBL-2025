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
            anim.SetBool("isPlaying", true);
            animIsPlaying = true;
        }
    }

    public void AnimFinish()
    {
        anim.SetBool("isPlaying", false);
        animIsPlaying = false;
    }
}
