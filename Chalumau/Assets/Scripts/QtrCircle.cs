using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QtrCircle : MonoBehaviour
{
    [SerializeField] private Transform m_circle;
    [SerializeField] private Transform m_circleMask;

    [SerializeField] private float m_sizeDiff = 0.2f;
    [SerializeField] private float m_size = 4.0f;

    // Update is called once per frame
    void Update()
    {
        m_size -= Time.deltaTime;
        m_size = Mathf.Max(m_size, 0);

        float maskDimension = Mathf.Max(m_size - m_sizeDiff, 0);
        m_circle.localScale = new Vector3(m_size, m_size, m_size);
        m_circleMask.localScale = new Vector3(maskDimension, maskDimension, maskDimension);
    }
}
