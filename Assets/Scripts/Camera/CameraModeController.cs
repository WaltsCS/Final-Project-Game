using UnityEngine;

public class CameraModeController : MonoBehaviour
{
    [Header("Camera Scripts")]
    [SerializeField] private PlayerRearToFrontCamera thirdPersonCam;     ///Third-person (rear-to-front) camera script
    [SerializeField] private CameraFollow topDownCam;    ///Top-down camera script

    [Header("Switch Settings")]
    [SerializeField] private string switchGateTag = "FinalGate"; ///Only switch when this gate tag opens

    private void OnEnable()
    {
        GateManager.OnAnyGateOpened += HandleGateOpened;
    }

    private void OnDisable()
    {
        GateManager.OnAnyGateOpened -= HandleGateOpened;
    }

    private void Start()
    {
        ///Start in third-person
        if (thirdPersonCam != null) thirdPersonCam.enabled = true;
        if (topDownCam != null) topDownCam.enabled = false;
    }

    private void HandleGateOpened(string tag)
    {
        ///Only react to the gate you care about
        if (tag != switchGateTag) return;

        ///Swap cameras
        if (thirdPersonCam != null) thirdPersonCam.enabled = false;
        if (topDownCam != null) topDownCam.enabled = true;
    }
}
