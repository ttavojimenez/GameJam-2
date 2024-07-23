using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stocktaking : MonoBehaviour
{
    public string[] foods;
    public GameObject[] foodsStockList;

    private bool full = false;

    private void Start()
    {
        foods = new string[4];
    }

    private void Update()
    {
        
    }

    public bool GetFull()
    {
        return foods.All(f => !string.IsNullOrEmpty(f));
    }

    public void AddFood(string nameFood)
    {
        for (int i = 0; i < foods.Length; i++)
        {
            if (foods[i] == null)
            {
                foods[i] = nameFood;
                Debug.Log("Nombre: " + foods[i]);
                foodsStockList[i].SetActive(false);
                break;
            }
        }
    }
}
