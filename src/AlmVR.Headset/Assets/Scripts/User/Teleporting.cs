using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporting : MonoBehaviour {

    private bool wasStickHeldLastFrame;
    private Vector3 currentTargetPosition;
    private Vector2 currentStickPosition;
    private NetworkManager networkManager;

    public GameObject Island;
    public float TeleportThreshold = 0.5f;
    public float NoHoldThreshold = 0.5f;
    public float RotateThreshold = 0.4f;
    public float RotateExponent = 1.5f;
    public float MaxRotateSpeed = 1.25f;

    public GameObject NetworkManagerObject;

	// Use this for initialization
	void Start () {
        networkManager = NetworkManagerObject.GetComponent<NetworkManager>();
	}

    // Update is called once per frame
    void Update () {
        OVRInput.Update();

        var secondaryThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        var lineRender = GetComponent<LineRenderer>();
        if (secondaryThumbstick.y > TeleportThreshold)
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
                currentStickPosition = secondaryThumbstick;

                lineRender.SetPositions(new Vector3[]
                {
                   rTouchWorldPosition,
                   currentTargetPosition
                });
            }
            else
            {
                lineRender.enabled = false;
            }

            wasStickHeldLastFrame = true;
        }
        else
        {
            lineRender.enabled = false;

            if (wasStickHeldLastFrame && secondaryThumbstick.magnitude < NoHoldThreshold)
            {
                transform.position = currentTargetPosition + new Vector3(0, 1.5f, 0);
                networkManager.RaiseEvent(NetworkManager.EventCode.PlayerPositionChanged, transform.position);
            }

            wasStickHeldLastFrame = false;
        }

        if (Mathf.Abs(secondaryThumbstick.x) > RotateThreshold)
        {
            var eulerAngles = transform.rotation.eulerAngles;
            var direction = Mathf.Abs(secondaryThumbstick.x) / secondaryThumbstick.x;
            eulerAngles.y += direction * Mathf.Min(Mathf.Pow(RotateExponent, Mathf.Abs(secondaryThumbstick.x)), MaxRotateSpeed);
            transform.eulerAngles = eulerAngles;

            networkManager.RaiseEvent(NetworkManager.EventCode.PlayerRotationChanged, eulerAngles.y);
        }
    }
}
