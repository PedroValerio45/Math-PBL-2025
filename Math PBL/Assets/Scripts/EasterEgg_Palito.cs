using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EasterEgg_Palito : MonoBehaviour
{
    public GameObject ElPalito;
    private Rigidbody rb;
    private bool isActive;
    
    // Start is called before the first frame update
    void Awake()
    {
        ElPalito.SetActive(false);
        rb = ElPalito.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive && Input.GetKeyDown(KeyCode.P))
        {
            ElPalito.SetActive(true);
            isActive = true;
        }

        if (ElPalito.transform.position.y <= -100)
        {
            ElPalito.SetActive(false);
        }
    }
    
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("collided with " + col.gameObject.name);
        
        if (col.CompareTag("Palito"))
        {
            rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
            Debug.Log("applied force to " + col.gameObject.name);
        }
    }
}
