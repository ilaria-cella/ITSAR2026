using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    private GameControls controls;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] float sprintMultiplier = 1.5f;
    [SerializeField] float gravity = 9.81f;
    float verticalVelocity = 0f;
    private void Awake() => controls = new GameControls();

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();
    private void OnDestroy() => controls.Dispose();

    private void Update()
    {
        // lettura continua: valore tra (-1,-1) e (1,1)
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();
        Debug.Log(input);

        float speed = controls.Player.Sprint.IsPressed() ? moveSpeed * sprintMultiplier : moveSpeed;

        characterController.Move(new Vector3(input.x, 0, input.y) * speed * Time.deltaTime);

        ApplyGravity();
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 velocity = Vector3.up * verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
    }
}