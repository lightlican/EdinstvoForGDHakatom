using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static bool canMove = true;

    [Header("Movement")]
    public float walkSpeed = 5f;
    private CharacterController characterController;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    private float cameraPitch = 0f;
    public Transform cameraTransform;

    [Header("Snow Sounds")]
    public AudioSource snowAudioSource;      
    public AudioClip[] snowStepSounds;       
    public float stepInterval = 0.5f;         

    private float stepTimer = 0f;
    private bool isMoving = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
        if (snowAudioSource == null)
            snowAudioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (!canMove) return;

        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        
        bool isMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

        characterController.SimpleMove(move * walkSpeed);

        
        HandleSnowSound(isMoving);

        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        cameraTransform.localEulerAngles = Vector3.right * cameraPitch;
    }

    void HandleSnowSound(bool moving)
    {
        if (moving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f && snowStepSounds.Length > 0)
            {
                AudioClip clip = snowStepSounds[Random.Range(0, snowStepSounds.Length)];
                snowAudioSource.pitch = Random.Range(0.9f, 1.1f);
                snowAudioSource.PlayOneShot(clip);

                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f; 
        }
    }

    public static void SetMovement(bool enabled)
    {
        canMove = enabled;
    }
}