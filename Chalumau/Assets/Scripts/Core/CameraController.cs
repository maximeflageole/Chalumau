using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float m_cameraCurrentSpeed;

    [SerializeField]
    private float m_cameraNormalSpeed;
    [SerializeField]
    private float m_cameraFastSpeed;
    [SerializeField]
    private Vector2 m_cameraClamp = new Vector2(2.0f, 12.0f);

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_cameraCurrentSpeed = m_cameraFastSpeed * Time.deltaTime * Camera.main.orthographicSize;
        }
        else
        {
            m_cameraCurrentSpeed = m_cameraNormalSpeed * Time.deltaTime * Camera.main.orthographicSize;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.forward * m_cameraCurrentSpeed, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.back * m_cameraCurrentSpeed, Space.World);

        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.left * m_cameraCurrentSpeed, Space.World);

        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.right * m_cameraCurrentSpeed, Space.World);
        }

        var mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll != 0)
        {
            Camera.main.orthographicSize += mouseScroll;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, m_cameraClamp.x, m_cameraClamp.y);
        }
    }
}
