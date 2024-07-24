using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public string typeFood;
    public int life;
    public int lifeAttempts;
    public float timeToEat;

    private Transform animalTransform;

    private void Start()
    {
        animalTransform = GetComponent<Transform>();
    }

    public void ReceiveFood()
    {
        animalTransform.transform.localScale = new Vector3(2, 2, 2);
    }
}