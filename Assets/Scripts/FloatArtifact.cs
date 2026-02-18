using UnityEngine;

public class FloatArtifact : MonoBehaviour
{
    [Header("Настройки левитации")]
    public float floatHeight = 0.3f;        
    public float floatSpeed = 1.5f;          
    public float rotationSpeed = 30f;        

    private Vector3 startPosition;
    private bool rotate = true;               
    public Light artifactLight;
    public float lightPulseSpeed = 2f;
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (rotate)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

        if (artifactLight != null)
        {
            float intensity = 0.8f + Mathf.Sin(Time.time * lightPulseSpeed) * 0.3f;
            artifactLight.intensity = intensity;
        }
    }
}