using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("here is rb: " + rb);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision!");
        if (other.gameObject.CompareTag("Hotspot"))
        {
            Debug.Log("Hotspot " + other.name + " was triggered");
            if (other.gameObject.GetComponent<AudioSource>())
            {
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }
}
