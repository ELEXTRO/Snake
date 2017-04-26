using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerateScript : MonoBehaviour
{

    public GameObject Reverse;
    public GameObject SpeedUp;
    public GameObject SpeedDown;
    public GameObject Cookie;
    public GameObject IceCube;

    private Vector3 _foodSpawnPosition;

    public static List<GameObject> ListOfFood = new List<GameObject>();

    void Start()
    {
        GenerateFood();
    }

    private void GenerateFood()
    {
        System.Random rand = new System.Random();
        RandowmSpawn(rand, Reverse);
        RandowmSpawn(rand, Cookie);
        RandowmSpawn(rand, IceCube);
        RandowmSpawn(rand, SpeedUp);
        RandowmSpawn(rand, SpeedDown);
    }

    private void RandowmSpawn(System.Random rand, GameObject foodGameObject)
    {
        bool checkSpawn = false;

        _foodSpawnPosition = new Vector3(rand.Next(-15, 16) - 0.5f, rand.Next(-8, 9) - 0.5f, -1f);

        foreach (GameObject food in ListOfFood)
        {
            if (food.transform.position == _foodSpawnPosition)
            {
                checkSpawn = true;
                break;
            }
        }

        foreach (GameObject tail in MoveScript.ListOfSnakeElements)
        {
            if (tail.transform.position == _foodSpawnPosition)
            {
                checkSpawn = true;
                break;
            }
        }

        if (checkSpawn)
        {
            RandowmSpawn(rand, foodGameObject);
        }

        GameObject foodClone = Instantiate(foodGameObject, _foodSpawnPosition, Quaternion.identity);
        ListOfFood.Add(foodClone);
    }


    public static void RandowmTeleport(GameObject foodGameObject)
    {
        bool checkSpawn = true;

        Vector3 tempPosition = new Vector3(Random.Range(-15, 16) - 0.5f, Random.Range(-8, 9) - 0.5f, -1f);
        while (checkSpawn)
        {

            checkSpawn = false;

            for (int i = 0; i < ListOfFood.Count; i++)
            {
                if (tempPosition == ListOfFood[i].transform.position)
                {
                    tempPosition = new Vector3(Random.Range(-15, 16) - 0.5f, Random.Range(-8, 9) - 0.5f, -1f);
                    i = 0;
                    checkSpawn = true;
                    break;
                }
            }

            for (int i = 0; i < MoveScript.ListOfSnakeElements.Count; i++)
            {
                if (tempPosition == MoveScript.ListOfSnakeElements[i].transform.position)
                {
                    tempPosition = new Vector3(Random.Range(-15, 16) - 0.5f, Random.Range(-8, 9) - 0.5f, -1f);
                    i = 0;
                    checkSpawn = true;
                    break;
                }
            }
        }

        foodGameObject.transform.position = tempPosition;
    }

    private void OnDestroy()
    {
        ListOfFood.Clear();
    }
}
