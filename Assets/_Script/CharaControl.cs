using System.Collections;
using UnityEngine;


public class CharaControl : MonoBehaviour
{
    public Transform cam;
    public float speed = 5f;
    public float sensitivity = 2f;
    public float gravity = 9.8f;

    public bool gempa_Start = false;


    private float verticalLookRange = 80f;
    private float rotationX = 0f;
    private CharacterController characterController;
    HideUnderTable HideUnderTable = null;
    HeartManager heart;
    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        heart = GameObject.FindAnyObjectByType<HeartManager>();
        HideUnderTable = GameObject.FindAnyObjectByType<HideUnderTable>();
    }
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        if(gempa_Start) StartCoroutine(ScreenShake(20));
    }

    private void Update()
    {
        if (gameManager == null || gameManager.gameOver) return;
        // Rotate player
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX * sensitivity);

        // Rotate camera
        rotationX -= mouseY * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -verticalLookRange, verticalLookRange);

        cam.localRotation = Quaternion.Euler(rotationX, 0, 0);

        if (HideUnderTable.currentlyHiding) return;
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
    
    public IEnumerator ScreenShake(float ScreenShakeTime, float ScreenShakeScale = 0.1f)
    {
        float time = Time.time + ScreenShakeTime;
        while(Time.time <= time)
        {
            cam.localPosition = new Vector3(Random.Range(-ScreenShakeScale, ScreenShakeScale),
                Random.Range(-ScreenShakeScale, ScreenShakeScale),
                0);
            yield return new WaitForEndOfFrame();
        }
        cam.localPosition = Vector3.zero;
    }


    private void Gravity()
    {
        if (!characterController.isGrounded)
        {
            characterController.Move(Vector3.down * gravity * Time.deltaTime);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            heart.ReduceHeart(1);
        }
    }
}
