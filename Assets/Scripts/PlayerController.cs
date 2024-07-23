using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller Settings")]
    private CharacterController characterController;
    private Animator animator;
    private float turnSmoothVelocity;
    private Vector3 velocity;

    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    public Transform cameraPlayer;
    public float gravity = -9.81f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        // Asegurar que se mantenga en el suelo
        velocity.y = -2f;

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;

        float movX = Input.GetAxisRaw("Horizontal");
        float movZ = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(movX, 0f, movZ).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraPlayer.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Mover al jugador
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }


        animator.SetFloat("Velocity", direction.magnitude);

        // Aplicar el movimiento de gravedad
        characterController.Move(new Vector3(velocity.x, velocity.y, velocity.z) * Time.deltaTime);
    }
}
