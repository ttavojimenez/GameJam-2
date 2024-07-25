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
        SelectFood();
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
                    Debug.Log(child.name);
                    child.gameObject.SetActive(false);
                }
            }
        }  
    }

    //Desabilita toda la comida dentro del espacio
    public void EnableOneChildStock(GameObject obj, int index)
    {
        Transform[] childs = obj.GetComponentsInChildren<Transform>(true);

        if (index >= 0 && index < childs.Length)
        {
            childs[index].gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("�ndice fuera de rango: " + index);
        }

    }

    //A�ade la comida al inventario
    public void AddFood(string nameFood)
    {
        for (int i = 0; i < foods.Length; i++)
        {
            if (foods[i] == null)
            {
                foods[i] = nameFood;

                if (nameFood.Equals("Banana"))
                {
                    EnableOneChildStock(foodsStockList[i], 1);
                    break;
                }
                else if (nameFood.Equals("Apple"))
                {
                    EnableOneChildStock(foodsStockList[i], 2);
                    break;
                }
                else if (nameFood.Equals("Watermelon"))
                {
                    EnableOneChildStock(foodsStockList[i], 3);
                    break;
                }
                else if (nameFood.Equals("PowerUp"))
                {
                    EnableOneChildStock(foodsStockList[i], 4);
                    break;
                }

                break;
            }
        }
    }

    //Ejecuta la logica para seleccionar la comida del estante
    private void TakeFood()
    {
        if (!GetFull() && currentFood != null && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(playerController.CoroutineAnim("ItsPicking"));
            AddFood(currentFood.tag);
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
    }

    private void DeleteFood(int keyFood)
    {
        foods[keyFood] = null;
        DisableChildsStock(foodsStockList[keyFood]);
    }

    //Entrega la comida al animal
    public void DeliverFoodToTheAnimal(int keyFood, string typeFoodAnimal)
    {
        if (keyFood < foods.Length && !string.IsNullOrEmpty(foods[keyFood]))
        {
            if (foods[keyFood] != null && !foods[keyFood].Equals("N/A"))
            {
                if (foods[keyFood].Equals(typeFoodAnimal) || foods[keyFood].Equals("PowerUp"))
                {
                    if (currentAnimal.isHungry)
                    {
                        currentAnimal.ReceiveFood();
                        StartCoroutine(playerController.CoroutineAnim("ItsDropping"));
                        DeleteFood(keyFood);
                    }
                }
            }
        }
    }

    /*Triggers*/
    private void OnTriggerEnter(Collider other)
    {
        //PowerUp, Watermelon, Apple, Banana
        if (other.gameObject.CompareTag("PowerUp") || other.gameObject.CompareTag("Watermelon") || 
            other.gameObject.CompareTag("Apple") || other.gameObject.CompareTag("Banana"))
        {
            currentFood = other.gameObject;
        }

        if (other.gameObject.CompareTag("Animal"))
        {
            currentAnimal = other.gameObject.GetComponent<AnimalController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentFood = null;
        currentAnimal = null;
    }
}