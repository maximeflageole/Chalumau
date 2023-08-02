using UnityEngine;

public class LookAtOrtographic : MonoBehaviour
{
    private Transform m_transformToLookAt;

    private void Start()
    {
        m_transformToLookAt = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(-m_transformToLookAt.forward, Vector3.up);
        transform.rotation = rotation;
    }
}
