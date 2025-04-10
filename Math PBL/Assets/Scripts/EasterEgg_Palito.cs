using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class EasterEgg_Palito : MonoBehaviour
{
    public GameObject ElPalito;
    private Rigidbody rb;
    private bool isActive;
    
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    
    // Start is called before the first frame update
    void Awake()
    {
        ElPalito.SetActive(false);
        rb = ElPalito.GetComponent<Rigidbody>();
        defaultPosition = ElPalito.transform.position;
        defaultRotation = ElPalito.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive && Input.GetKeyDown(KeyCode.P))
        {
            ElPalito.SetActive(true);
            isActive = true;
        }

        if (ElPalito.transform.position.y <= -30)
        {
            ElPalito.SetActive(false);
            // isActive = false; // comment or uncomment this to allow El Palito to be respawned after the first time it disappears
            ElPalito.transform.position = defaultPosition;
            ElPalito.transform.rotation = defaultRotation;
        }
    }
    
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("collided with " + col.gameObject.name);
        
        if (col.CompareTag("Palito"))
        {
            rb.AddForce(Vector3.up * Random.Range(10, 20), ForceMode.Impulse);
            Debug.Log("applied force to " + col.gameObject.name);
        }
    }
}
