using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private Camera cam;
    public GameObject button;
    private Animator anim;
    
    private bool animIsPlaying;
    public static bool buttonIsPressed;
    [SerializeField] private bool cameraChanged;
    private Vector3 defaultCameraPosition;
    private Quaternion defaultCameraRotation;

    void Start()
    {
        anim = button.GetComponent<Animator>();
        cam = Camera.main;
        
        defaultCameraPosition = cam.transform.position;
        defaultCameraRotation = cam.transform.rotation;
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

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!cameraChanged)
            {
                cameraChanged = true;
                cam.transform.position = new Vector3(16.3f, 4.3f, -2.5f);
                cam.transform.rotation = Quaternion.Euler(20, -80, 0);
            }
            else
            {
                cameraChanged = false;
                cam.transform.position = defaultCameraPosition;
                cam.transform.rotation = defaultCameraRotation;
            }
        }
    }

    public void AnimFinish()
    {
        anim.SetBool("isPlaying", false);
        animIsPlaying = false;
    }
}
