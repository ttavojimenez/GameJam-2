using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimalController : MonoBehaviour
{
    public string typeFood;
    public float timeToEat;
    private int life;
    private int lifeAttempts = 3;
    public bool isHungry = false;
    private bool isAte = false;

    private GameObject animalChild;
    private Transform animalTransform;
    private Animator animator;

    private GameObject spriteHungry;

    private float currentTime;

    // Nueva bandera para evitar que la corutina se inicie repetidamente
    private bool isHungryRoutineRunning = false;

    // Guardar el tama�o inicial y el tama�o objetivo
    private Vector3 initialScale;
    private Vector3 targetScale = new Vector3(1f, 1f, 1f);
    private Vector3 minScale = new Vector3(0.4f, 0.4f, 0.4f);
    private float scaleIncrement;

    private Transform player;

    private void Start()
    {
        if (transform.childCount > 0)
        {
            animalTransform = transform.GetChild(0);
            animalChild = animalTransform.gameObject;

            initialScale = animalChild.transform.localScale;
            animalChild.transform.localScale = minScale;         

            animator = animalChild.GetComponent<Animator>();

            if (transform.childCount > 1)
            {
                spriteHungry = transform.GetChild(1).gameObject;

                if (spriteHungry != null)
                {
                    spriteHungry.SetActive(false);
                }
            }

            scaleIncrement = (initialScale.x - minScale.x) / 6f;

            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (life < 100)
        {
            OrderFood();
        }

        if (player != null)
        {
            animalChild.transform.LookAt(player);
            //spriteHungry.transform.LookAt(player);//
        }
    }

    public void OrderFood()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= timeToEat)
        {
            isHungry = true;
            spriteHungry.SetActive(true);
            // Solo inicia la corutina si no est� ya en ejecuci�n
            if (!isHungryRoutineRunning)
            {
                StartCoroutine(StartRoutineHungry());
            }
        }

        if (lifeAttempts <= 0)
        {
            SceneManager.LoadScene(3);
        }
    }

    public void ReceiveFood()
    {
        if (life < 100 && isHungry)
        {
            life += 17;
            Vector3 newScale = animalChild.transform.localScale + new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);

            if (newScale.x > initialScale.x)
            {
                newScale = initialScale;
            }

            animalChild.transform.localScale = newScale;
            isHungry = false;
            animator.SetBool("AlmostDie", false);
            animator.SetBool("Hungry", false);
            spriteHungry.SetActive(false);
            currentTime = 0;
            // Reinicia la bandera ya que el animal ha sido alimentado
            isHungryRoutineRunning = false;
        }
    }

    private IEnumerator StartRoutineHungry()
    {
        if (lifeAttempts <= 1)
        {
            animator.SetBool("AlmostDie", true);
        }
        else
        {
            animator.SetBool("Hungry", isHungry);
        }

        isHungryRoutineRunning = true;
        yield return new WaitForSeconds(30f);
        if (isHungry)
        {
            lifeAttempts--;
        }
        spriteHungry.SetActive(false);
        currentTime = 0;
        animator.SetBool("AlmostDie", false);
        animator.SetBool("Hungry", false);
        isHungryRoutineRunning = false; // Reinicia la bandera al finalizar la corutina
    }

    public int GetLife()
    {
        return life;
    }
}