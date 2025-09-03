using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float startSize = 20f;
    [SerializeField] private float endSize = 8f;
    [SerializeField] private float matchDuration = 120f;
    
    private Camera cam;
    private float timer = 0f;

    void Awake() => cam = GetComponent<Camera>();

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / matchDuration);
        cam.orthographicSize = Mathf.Lerp(startSize, endSize, t);
    }
}
