using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance;

    public bool Dialogue;

    // Cinemachine Shake Values
    [SerializeField] float defaultShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    [SerializeField] float defaultShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    float ShakeElapsedTime = 0f;

    // Cinemachine Virtual Camera
    private CinemachineVirtualCamera virtualCamera;

    // Cinemachine Components
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private CinemachineFollowZoom virtualCameraFollowZoom;
    private CinemachineConfiner confiner;

    private void Awake()
    {
        Instance = this;

        // Get virtual camera & components
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        // If virtual camera exists, get camera noise component
        if (virtualCamera != null)
        {
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
            virtualCameraFollowZoom = virtualCamera.GetComponent<CinemachineFollowZoom>();
            confiner = virtualCamera.GetComponent<CinemachineConfiner>();
        }
        else
        {
            Debug.Log("Virtual camera was not found!");
        }
    }

    /// <summary>
    /// Update confiner on the virtual camera
    /// </summary>
    /// <param name="collider"></param>
    public void UpdateConfiner(PolygonCollider2D collider)
    {
        if (confiner == null)
        {
            Debug.Log("Virtual camera confiner not assigned!");
            return;
        }

        confiner.m_BoundingShape2D = collider;
        confiner.InvalidatePathCache();
    }

    /// <summary>
    /// Shakes the camera with the following parameters: Duration, Amplitude, Frequency. -- Duration always needs to be given a value, amplitude and frequency will use default value when left at 0. Defaults: Amp(1.2f), Freq(2.0f).
    /// </summary>
    public void ShakeCamera(float duration, float amplitude = 1.2f, float frequency = 2.0f)
    {
        if (virtualCameraNoise == null)
        {
            Debug.Log("Virtual camera noise component is not assigned!");
            return;
        }

        ShakeElapsedTime = duration / 10;
    }

    private void Update()
    {
        // If the virtual camera is null, avoid update.
        if (virtualCamera == null)
        {
            Debug.Log("Cinemachine virtual camera component not assigned!");
            return;
        }

        //----------------------------------------

        // Set Cinemachine Camera Noise parameters
        if (ShakeElapsedTime > 0)
        {
            // Update Shake Timer
            ShakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            // If Camera Shake effect is over, reset variables
            virtualCameraNoise.m_AmplitudeGain = 0f;
            ShakeElapsedTime = 0f;
        }

        // If player is in a dialogue, zoom in
        if (Dialogue && virtualCameraFollowZoom.m_Width > 1)
        {
            virtualCameraFollowZoom.m_Width -= Time.deltaTime * 10;
        }
        // If player is no longer in dialogue, zoom out
        else if (!Dialogue && virtualCameraFollowZoom.m_Width < 12)
        {
            virtualCameraFollowZoom.m_Width += Time.deltaTime * 10;
        }
    }
}
