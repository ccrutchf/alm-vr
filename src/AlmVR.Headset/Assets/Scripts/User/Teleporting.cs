using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporting : MonoBehaviour {

    private bool wasStickHeldLastFrame;
    private Vector3 currentTargetPosition;

    public GameObject Island;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        OVRInput.Update();

        var primaryThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        var lineRender = GetComponent<LineRenderer>();
        if (primaryThumbstick.magnitude > 0.01f)
        {
            lineRender.enabled = true;

            var rTouchLocalPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            var rTouchWorldPosition = transform.TransformPoint(rTouchLocalPosition);

            var rTouchLocalForward = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward;
            var rTouchWorldForward = transform.TransformDirection(rTouchLocalForward);

            var ray = new Ray(rTouchWorldPosition, rTouchWorldForward);
            RaycastHit hit;
            if (Island.GetComponent<Collider>().Raycast(ray, out hit, 100.0f))
            {
                currentTargetPosition = hit.point;

                lineRender.SetPositions(new Vector3[]
                {
                   rTouchWorldPosition,
                   currentTargetPosition
                });
            }

            wasStickHeldLastFrame = true;
        }
        else
        {
            lineRender.enabled = false;

            if (wasStickHeldLastFrame)
            {
                transform.position = currentTargetPosition + new Vector3(0, 1.5f, 0);
            }

            wasStickHeldLastFrame = false;
        }
	}
}
