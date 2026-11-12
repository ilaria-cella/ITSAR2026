using UnityEngine;

public class PlayerController_RigidBody : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 input;

    private float force = 5f;

    private GameControls controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new GameControls();

    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();
    private void OnDestroy() => controls.Dispose();

    private void Update()       // ogni frame: leggo
    {
        input = controls.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()  // 50 volte al secondo: muovo
    {
        // dinamico: una spinta

        //rb.AddForce(new Vector3(input.x, 0f, input.y) * force); // non metto time.deltatime perchè il fixed update non è ad ogni frame ma intervalli fissi
        Vector3 input3 = new Vector3(input.x, 0f, input.y);
        Vector3 velocity = input3 * force;
        
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        // cinematico: decido io dove si trova (come Projectile)
        // rb.MovePosition(rb.position
        //     + velocity * Time.fixedDeltaTime);
    }

}
