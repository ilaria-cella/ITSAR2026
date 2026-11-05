using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{

    [SerializeField] CharacterController characterController;
    [SerializeField] Transform cameraTransform;
    private GameControls controls;


    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float sprintMultiplier = 1.5f;
    [SerializeField] float gravity = 10f;

    [SerializeField] float rotationSpeed = 720f;



    // In Awake viene assegnato un nuovo valore di oggetto GameControls a controls (non assegnato)
    private void Awake() => controls = new GameControls();


    // private void Awake
    //{
    //   controls = new GameControls();
    //}

    //dopo aver assegnato a un valore a GameControls ne abilitiamo/attiviamo la mapppa Player

    private void OnEnable() => controls.Player.Enable();

    private void OnDisable() => controls.Player.Disable();

    private void OnDistroy() => controls.Dispose();

    private void Update()
    {
        ApplyGravity();
        Move();
        RotateToDirection();

    }

    void Move()
    {
        // lettura continua: valore tra (-1,-1) e (1,1)
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();
        Debug.Log(input);

        float speed = moveSpeed;
        if (controls.Player.Sprint.IsPressed()) //tenuto premuto
            speed *= sprintMultiplier;

        //Vector3 moveVector = new Vector3(input.x, 0, input.y) * speed;
        Vector3 moveVector = CameraRelative(input) * speed;

        characterController.Move(moveVector * Time.deltaTime);
    }

    void ApplyGravity()
    {
        float verticalVelocity = 0f;
        if (characterController.isGrounded)
        {
            verticalVelocity = 2f;      //resta appoggiato
        }
        else
        {
            verticalVelocity = gravity;

        }
        Vector3 velocity = Vector3.up * verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
    }


    void RotateToDirection()
    {
        //lettura continau: valore tra (-1,-1) e (1,1)
        Vector2 directionRaw = controls.Player.Move.ReadValue<Vector2>();
        Vector3 direction = CameraRelative(directionRaw);
        //new Vector3(directionRaw.x, 0f, directionRaw.y);

        if (direction.sqrMagnitude > 0.001f) //si sta muovendo
        {
            //guardare nella posizione di movimento 
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //girarsi gradualmente
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRotation,
                rotationSpeed * Time.deltaTime);
        }
    }


    //variable locale


    private Vector3 CameraRelative(Vector2 input)
    {
        if (cameraTransform == null)          // nessuna camera
            return new Vector3(input.x, 0f, input.y);
        //Vector3 forward = Vector3.ProjectOnPlane(CameraTransform.forward, Vector.up);
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;                // appiattito sul terreno
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0f;
        right.Normalize();

        // ClampMagnitude: in diagonale non si va più veloci
        return Vector3.ClampMagnitude(
            forward * input.y + right * input.x, 1f);
    }




}
