using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stocktaking : MonoBehaviour
{
    public string[] foods;
    public GameObject[] foodsStockList;
    public static bool isTakeFood = false;

    private GameObject currentFood;
    private PlayerController playerController;

    private AnimalController currentAnimal;
    private int keyPressDeliverFood = 10;
    private bool isDeliverToFood = false;

    private void Start()
    {
        foods = new string[4];
        playerController = GetComponent<PlayerController>();
        
        foreach (GameObject foodStock in foodsStockList)
        {
            DisableChildsStock(foodStock);
        }
    }

    private void Update()
    {
        TakeFood();
    }

    public bool GetFull()
    {
        return foods.All(f => !string.IsNullOrEmpty(f));
    }

    //Desabilita toda la comida dentro del espacio
    public void DisableChildsStock(GameObject obj)
    {
        Transform[] childs = obj.GetComponentsInChildren<Transform>();

        if (childs.Length > 0)
        {
            foreach (Transform child in childs)
            {
                if (child != obj.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }  
    }

    //Añade la comida al inventario
    public void AddFood(string nameFood)
    {
        for (int i = 0; i < foods.Length; i++)
        {
            if (foods[i] == null)
            {
                foods[i] = nameFood;
                foodsStockList[i].SetActive(false);
                break;
            }
        }
    }

    //Ejecuta la logica para seleccionar la comida del estante
    private void TakeFood()
    {
        if (!GetFull() && currentFood != null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Selecciono la comida: " + currentFood.name);
            StartCoroutine(playerController.CoroutineAnim("ItsPicking"));
            AddFood(currentFood.name);
            currentFood = null;
        }
    }

    //Selecciona la comida
    private void SelectFood()
    {
        if (currentAnimal != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                keyPressDeliverFood = 0;
                Debug.Log(keyPressDeliverFood);
                DeliverFoodToTheAnimal(keyPressDeliverFood, currentAnimal.typeFood);
                currentAnimal = null;

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                keyPressDeliverFood = 1;
                Debug.Log(keyPressDeliverFood);
                DeliverFoodToTheAnimal(keyPressDeliverFood, currentAnimal.typeFood);
                currentAnimal = null;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                keyPressDeliverFood = 2;
                Debug.Log(keyPressDeliverFood);
                DeliverFoodToTheAnimal(keyPressDeliverFood, currentAnimal.typeFood);
                currentAnimal = null;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                keyPressDeliverFood = 3;
                Debug.Log(keyPressDeliverFood);
                DeliverFoodToTheAnimal(keyPressDeliverFood, currentAnimal.typeFood);
                currentAnimal = null;
            }
        }

        //StartCoroutine(CorrutineAnim("ItsDropping"));
    }

    //Entrega la comida al animal
    public void DeliverFoodToTheAnimal(int keyFood, string typeFoodAnimal)
    {
        if (keyFood < foods.Length && !string.IsNullOrEmpty(foods[keyFood]))
        {
            if (foods[keyFood] != null && !foods[keyFood].Equals("N/A"))
            {
                if (foods[keyFood].Equals(typeFoodAnimal))
                {
                    Debug.Log("Si tiene la comida");
                    currentAnimal.ReceiveFood();
                }
            }
        }
    }

    /*Colisiones y Triggers*/
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Food"))
        {
            currentFood = hit.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Animal"))
        {
            Debug.Log(other.gameObject.name);
            currentAnimal = other.gameObject.GetComponent<AnimalController>();
            currentAnimal.ReceiveFood();
        }
    }
}