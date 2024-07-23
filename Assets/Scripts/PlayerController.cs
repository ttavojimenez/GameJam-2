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

    public Stocktaking stocktaking;

    private GameObject currentFood;
    private bool isTakeFood = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMovement();
        TakeFood();
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

        if (direction.magnitude >= 0.1f && !isTakeFood)
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

    private void TakeFood()
    {
        if (!stocktaking.GetFull() && currentFood != null && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(CorrutineTakeFood());
            stocktaking.AddFood(currentFood.name);
            currentFood = null;
        }
    }

    private IEnumerator CorrutineTakeFood()
    {
        animator.SetBool("ItsPicking", true);
        isTakeFood = true;
        yield return new WaitForSeconds(5f);
        animator.SetBool("ItsPicking", false);
        isTakeFood = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.CompareTag("Food"))
        {
            Debug.Log(hit.gameObject.name);
            currentFood = hit.gameObject;
        }
    }
}
