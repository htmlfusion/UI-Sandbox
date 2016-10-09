using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;

public class HologramController : MonoBehaviour {

    public enum ActiveGestureAction
    {
        Manipulating,
        NavigatingY,
        NavigatingX
    }

    ActiveGestureAction currentState = ActiveGestureAction.Manipulating;

    [Tooltip("Hologram object to be wrapped in control functionality")]
    public GameObject hologram;

    [Tooltip("Time in seconds to complete consecutive air taps - Triggers double-tap action")]
    public float doubleTapTolerance = 0.5f;

    private float lastTap = 0;

    CustomManipulation cm;
    CustomRotation cr;

    // Use this for initialization
    void Start ()
    {
        if (hologram != null)
        {
            // wraps a collider that will trigger gestures around the encased hologram
            Vector3 childColSize = hologram.GetComponent<Collider>().bounds.size;
            BoxCollider boxCol = gameObject.GetComponent<BoxCollider>();
            boxCol.size = childColSize;

            // reference scripts for enabling/disabling
            cm = GetComponent<CustomManipulation>();
            cr = GetComponent<CustomRotation>();

            cm.enabled = true;
            cr.enabled = false;
        }
        else
        {
            Debug.Log("Please attach a hologram in the inspector");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnSelect()
    {
        if ((Time.time - lastTap) < doubleTapTolerance)
        {
            if (currentState == ActiveGestureAction.NavigatingX)
            {
                currentState = 0;
            }
            else
            {
                currentState++;
            }
            EnableCurrentGestureAction();
        }
        lastTap = Time.time;
    }

    void EnableCurrentGestureAction()
    {
        if (currentState == ActiveGestureAction.Manipulating)
        {
            cm.enabled = true;
            cr.enabled = false;
        }
        else if (currentState == ActiveGestureAction.NavigatingY)
        {
            cm.enabled = false;
            cr.enabled = true;
            cr.NavigationAxis(CustomRotation.ActiveAxis.y);
        }
        else if (currentState == ActiveGestureAction.NavigatingX)
        {
            cr.NavigationAxis(CustomRotation.ActiveAxis.x);
        }
    }
}
