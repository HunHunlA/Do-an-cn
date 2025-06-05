using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Camera cam;
    public bool followPlayer = true;
    public float zoom = 5f;
    public float speed = 0.1f;
    public float directionY = 10f;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    void FixedUpdate()
    {
        Zoom();
        FollowPlayer();
    }
    void Zoom()
    {
        if (cam.orthographic)
        {
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoom;
        }
        else
        {
            cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoom;
        }
    }
    void FollowPlayer()
    {
        if (followPlayer)
        {
            if (player != null)
            {
                Vector3 newPosition = new Vector3(player.position.x, player.position.y + directionY, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * speed);
            }
        }
    }

}