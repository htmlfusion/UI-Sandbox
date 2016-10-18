using UnityEngine;
using System.Collections;

public class FadeFX : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnEnable()
    {
        // Do a raycast into the world that will only hit the Spatial Mapping mesh.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
            1.5f))
        {
            // Move this object to where the raycast
            // hit the Spatial Mapping mesh.
            // Here is where you might consider adding intelligence
            // to how the object is placed.  For example, consider
            // placing based on the bottom of the object's
            // collider so it sits properly on surfaces.
            this.transform.position = hitInfo.point;

            // Rotate this object to face the user.
            Quaternion toQuat = Camera.main.transform.localRotation;
            toQuat.x = 0;
            toQuat.z = 0;
            this.transform.rotation = toQuat;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Camera.main.transform.position);
            headPosition = Camera.main.transform.position;
            Vector3 pos = ray.GetPoint(1.6f);
            this.transform.position = new Vector3(headPosition.x, headPosition.y, pos.z);
        }

        StartCoroutine(HideObject());
    }

    IEnumerator HideObject()
    {
        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
    }
}
