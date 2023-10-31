using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    [Tooltip("Margin from Bottom-Left")]
    public Vector2 minThreshold;
    [Tooltip("Margin from Top-Right")]
    public Vector2 maxThreshold;

    private Camera m_camera;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 camBounds = CalculateCameraBounds();
        Vector2 playerPosition = player.transform.position;
        
        Vector2 camMin = playerPosition - camBounds + maxThreshold;
        Vector2 camMax = playerPosition + camBounds - minThreshold;

        //Debug.Log("camMin: " + camMin.x + ", " + camMin.y);
        //Debug.Log("camMax: " + camMax.x + ", " + camMax.y);

        Vector2 camPos = transform.position;
        camPos = new Vector2(Mathf.Clamp(camPos.x, camMin.x, camMax.x), Mathf.Clamp(camPos.y, camMin.y, camMax.y));

        transform.position = new Vector3(camPos.x, camPos.y, transform.position.z);
    }

    Vector2 CalculateCameraBounds()
    {
        float aspect = (float)Screen.width / (float)Screen.height;
        float camHeight = m_camera.orthographicSize;
        float camWidth = camHeight * aspect;

        return new Vector2(camWidth, camHeight);
    }
}
