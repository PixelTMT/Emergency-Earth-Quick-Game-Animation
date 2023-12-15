using UnityEngine;


public class CharaControl : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public float gravity = 9.8f;

    private float verticalLookRange = 80f;
    private float rotationX = 0f;
    private CharacterController characterController;

    HideUnderTable HideUnderTable = null;

    private void Awake()
    {
        HideUnderTable = GameObject.FindAnyObjectByType<HideUnderTable>();
    }
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Rotate player
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX * sensitivity);

        // Rotate camera
        rotationX -= mouseY * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -verticalLookRange, verticalLookRange);

        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        if (HideUnderTable.isHiding) return;
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalMovement, 0, verticalMovement);
        movement = transform.TransformDirection(movement);

        // gravity
        Gravity();

        // movement
        Vector3 combinedMovement = movement * speed * Time.deltaTime + Vector3.down * gravity * Time.deltaTime;
        characterController.Move(combinedMovement);


    }

    private void Gravity()
    {
        if (!characterController.isGrounded)
        {
            characterController.Move(Vector3.down * gravity * Time.deltaTime);
        }
    }
}
