using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBounds : MonoBehaviour
{
    [SerializeField] private float borderThickness = 1f;
    
    private Camera cam;
    private BoxCollider2D[] borders = new BoxCollider2D[4];
    private float lastWidth, lastHeight;
    
    void Awake()
    {
        cam = GetComponent<Camera>();

        for (int i = 0; i < 4; i++)
        {
            GameObject border = new GameObject("Border" + i);
            border.transform.parent = transform;
            var col = border.AddComponent<BoxCollider2D>();
            col.isTrigger = false;
            borders[i] = col;
        }
        float height = cam.orthographicSize * 2;
        float width = height * cam.aspect;
        UpdateBorders(width, height);
    }
    
    void LateUpdate()
    {
        float height = cam.orthographicSize * 2;
        float width = height * cam.aspect;
        if (!Mathf.Approximately(height, lastHeight) || !Mathf.Approximately(width, lastWidth))
        {
            UpdateBorders(width, height);
            lastHeight = height;
            lastWidth = width;
        }
    }
    
    void UpdateBorders(float width, float height)
    {
        borders[0].transform.localPosition = new Vector3(0, height / 2 + borderThickness / 2, 0); 
        borders[1].transform.localPosition = new Vector3(0, -height / 2 - borderThickness / 2, 0); 
        borders[2].transform.localPosition = new Vector3(-width / 2 - borderThickness / 2, 0, 0); 
        borders[3].transform.localPosition = new Vector3(width / 2 + borderThickness / 2, 0, 0); 

        borders[0].size = borders[1].size = new Vector2(width, borderThickness);
        borders[2].size = borders[3].size = new Vector2(borderThickness, height);
    }
    
}
