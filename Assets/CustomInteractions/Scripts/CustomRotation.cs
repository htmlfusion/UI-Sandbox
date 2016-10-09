using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;

public class CustomRotation : MonoBehaviour {

    [Tooltip("How much to scale each axis of hand movement (camera relative) when manipulating the object")]
    public Vector3 handRotationScale = new Vector3(2.0f, 2.0f, 4.0f);  // Default tuning values, expected to be modified per application

    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 5f;

    private Vector3 manipulationPreviousPosition;

    private float rotationFactor;

    private Vector3 initialHandPosition;
    private Vector3 initialObjectPosition;

    private Interpolator targetInterpolator;

    private bool Navigating { get; set; }
    private bool FocusedObject { get; set; }

    private void OnEnable()
    {
        if (GestureManager.Instance != null)
        {
            GestureManager.Instance.NavigationStarted += BeginNavigation;
            GestureManager.Instance.NavigationCompleted += EndNavigation;
            GestureManager.Instance.NavigationCanceled += EndNavigation;
        }
        else
        {
            Debug.LogError(string.Format("GestureNavigator enabled on {0} could not find GestureManager instance, navigation will not function", name));
        }
    }

    private void OnDisable()
    {
        if (GestureManager.Instance)
        {
            GestureManager.Instance.NavigationStarted -= BeginNavigation;
            GestureManager.Instance.NavigationCompleted -= EndNavigation;
            GestureManager.Instance.NavigationCanceled -= EndNavigation;
        }

        Navigating = false;
        FocusedObject = false;
    }

    private void BeginNavigation()
    {
        if (GestureManager.Instance != null && GestureManager.Instance.NavigationInProgress)
        {
            if (GazeManager.Instance.HitInfo.collider.name == gameObject.name)
            {
                Navigating = true;

                //targetInterpolator = gameObject.GetComponent<Interpolator>();

                // In order to ensure that any manipulated objects move with the user, we do all our math relative to the camera,
                // so when we save the initial hand position and object position we first transform them into the camera's coordinate space
                initialHandPosition = Camera.main.transform.InverseTransformPoint(GestureManager.Instance.NavigationHandPosition);
                initialObjectPosition = Camera.main.transform.InverseTransformPoint(transform.position);
            }
        }
    }

    private void EndNavigation()
    {
        Navigating = false;
    }

    void Update()
    {
        if (Navigating)
        {
            PerformRotation();
        }
    }

    private void PerformRotation()
    {
        // First step is to figure out the delta between the initial hand position and the current hand position
        Vector3 localHandPosition = Camera.main.transform.InverseTransformPoint(GestureManager.Instance.NavigationHandPosition);
        Vector3 initialHandToCurrentHandAdjusted = (localHandPosition - initialHandPosition) * RotationSensitivity;

        // When performing a manipulation gesture, the hand generally only translates a relatively small amount.
        // If we move the object only as much as the hand itself moves, users can only make small adjustments before
        // the hand is lost and the gesture completes.  To improve the usability of the gesture we scale each
        // axis of hand movement by some amount (camera relative).  This value can be changed in the editor or
        // at runtime based on the needs of individual movement scenarios.
        //Vector3 scaledLocalHandPositionDelta = Vector3.Scale(initialHandToCurrentHand, handRotationScale);

        // Once we've figured out how much the object should move relative to the camera we apply that to the initial
        // camera relative position.  This ensures that the object remains in the appropriate location relative to the camera
        // and the hand as the camera moves.  The allows users to use both gaze and gesture to move objects.  Once they
        // begin manipulating an object they can rotate their head or walk around and the object will move with them
        // as long as they maintain the gesture, while still allowing adjustment via hand movement.
       // Vector3 localObjectPosition = initialObjectPosition + scaledLocalHandPositionDelta;
        //Vector3 worldObjectPosition = Camera.main.transform.TransformPoint(localObjectPosition);

        // If the object has an interpolator we should use it, otherwise just move the transform directly
        if (targetInterpolator != null)
        {
            //targetInterpolator.SetTargetPosition(worldObjectPosition);
        }
        else
        {
            //transform.position = worldObjectPosition;
            //transform.Rotate(worldObjectPosition
            transform.parent.Rotate(Vector3.up * initialHandToCurrentHandAdjusted.x * -1);
        }

        ///*if (GestureManager.Instance.IsNavigating &&
        //    (!ExpandModel.Instance.IsModelExpanded ||
        //    (ExpandModel.Instance.IsModelExpanded && HandsManager.Instance.FocusedGameObject == gameObject)))*/
        //if (GestureManager.Instance.IsNavigating)
        //{
        /* TODO: DEVELOPER CODING EXERCISE 2.c */
        // 2.c: Calculate rotationFactor based on GestureManager's NavigationPosition.X and multiply by RotationSensitivity.
        // This will help control the amount of rotation.
        //rotationFactor = GestureManager.Instance.NavigationPosition.x * RotationSensitivity;
            // 2.c: transform.Rotate along the Y axis using rotationFactor.
            //transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
        //}
    }

    //void PerformManipulationStart(Vector3 position)
    //{
    //    manipulationPreviousPosition = position;
    //}

    //void PerformManipulationUpdate(Vector3 position)
    //{
    //    if (GestureManager.Instance.IsManipulating)
    //    {
    //        /* TODO: DEVELOPER CODING EXERCISE 4.a */

    //        Vector3 moveVector = Vector3.zero;
    //        // 4.a: Calculate the moveVector as position - manipulationPreviousPosition.
    //        moveVector = position - manipulationPreviousPosition;
    //        // 4.a: Update the manipulationPreviousPosition with the current position.
    //        manipulationPreviousPosition = position;

    //        // 4.a: Increment this transform's position by the moveVector.
    //        transform.position += moveVector;
    //    }
    //}
}
