using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller Settings")]
    private CharacterController characterController;
    private Animator animator;
    private bool isAnimating = false;
    private float turnSmoothVelocity;
    private Vector3 velocity;
    private bool isRunning;

    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float turnSmoothTime = 0.1f;
    public Transform cameraPlayer;
    public float gravity = -9.81f;

    private void Start()
    {
        // Ocultar el cursor
        Cursor.visible = false;
        // Bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState= CursorLockMode.None;
        }
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

    isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    float currentSpeed = isRunning ? runSpeed : walkSpeed;

    if (direction.magnitude >= 0.1f && !Stocktaking.isTakeFood)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraPlayer.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        // Crear el vector de movimiento
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        
        // Normalizar el vector de movimiento
        moveDir.Normalize();

        // Aplicar la rotación
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        // Mover al jugador
        characterController.Move(moveDir * currentSpeed * Time.deltaTime);
    }

    ActiveAnimationVelocity(isRunning, direction);

    // Aplicar el movimiento de gravedad
    characterController.Move(new Vector3(velocity.x, velocity.y, velocity.z) * Time.deltaTime);
}

    private void ActiveAnimationVelocity(bool isRunning, Vector3 direction)
    {
        float animationSpeed = isRunning ? direction.magnitude * 2 : direction.magnitude;
        animator.SetFloat("Velocity", animationSpeed);
    }


    //Ejecuta los estados de animacion para tomar comida
    public IEnumerator CoroutineAnim(string animName)
    {
        if (isAnimating) yield break;  // Si ya est� en curso, no hacer nada
        isAnimating = true;

        animator.SetBool(animName, true);
        Stocktaking.isTakeFood = true;
        yield return new WaitForSeconds(2f);
        animator.SetBool(animName, false);
        Stocktaking.isTakeFood = false;

        isAnimating = false;
    }
}