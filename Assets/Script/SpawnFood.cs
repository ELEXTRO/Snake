using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{

    public int MaxFoodCount = 3;

    public GameObject Cookie;
    //public GameObject SnakeTail;

    public static List<GameObject> ListOfFood = new List<GameObject>();

    private Vector3 _spawnPosition;

    void Start()
    {
        GenerateFood();
        foreach (GameObject lol in ListOfFood)
        {
            print(lol.transform.position);
        }
    }

    private void GenerateFood()
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < MaxFoodCount; i++)
        {
            /*
            GameObject temp = new GameObject();
            //0.046295
            temp.transform.localScale = new Vector3(0.046295f, 0.046295f, 1f);
            temp.transform.localPosition = new Vector3(-0.5f, 0.5f, -1f);
            temp.AddComponent<SpriteRenderer>();
            temp.AddComponent<BoxCollider2D>();
            temp.GetComponent<SpriteRenderer>().sprite = TailSprite;
            temp.GetComponent<BoxCollider2D>().size = new Vector2(15f, 15f);
            temp.GetComponent<BoxCollider2D>().isTrigger = true;
            temp.transform.SetParent(Cookie.transform);
            */
            _spawnPosition = new Vector3(rand.Next(-15, 16) - 0.5f, rand.Next(-8, 9) - 0.5f, -1f);
            Cookie.transform.position = _spawnPosition;
            Instantiate(Cookie, _spawnPosition, Quaternion.identity);
            ListOfFood.Add(Cookie);
            Debug.Log("pos " + ListOfFood[i].transform.position.ToString());
        }
    }
}
