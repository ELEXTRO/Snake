using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class MoveScript : MonoBehaviour
{
    public Sprite HeadSprite;
    public Sprite TailSprite;

    public GameObject SnakeBody;
    private GameObject _tempElement;
    private GameObject _spawn;

    private int _snakeLeight = 3;

    public float MoveTime = 0.1f;
    private float _speed;
    private float _timer;
    private float _moveVertikal;
    private float _moveHorizontal;

    private Vector3 _moveVector;
    private Vector3 _vector;
    private Vector2 _foodPosition;
    private Vector2 _headPosition;
    private Vector2 _tailPosition;


    private Quaternion _headDirection;

    private bool _vertical = true;
    private bool _horizontal = true;
    private bool _checkDirection;

    public static List<GameObject> ListOfSnakeElements = new List<GameObject>();

    void Start()
    {
        GenerateSnake(_snakeLeight, SnakeBody.transform.position);
        _speed = MoveTime;
    }

    private void GenerateSnake(int snakeLeight, Vector3 position)
    {
        for (int i = 0; i < snakeLeight; i++)
        {
            GameObject temp = new GameObject();
            temp.transform.localScale = new Vector3(0.046295f, 0.046295f, 1f);
            temp.transform.localPosition = position;
            temp.AddComponent<SpriteRenderer>();
            temp.GetComponent<SpriteRenderer>().sprite = TailSprite;
            temp.tag = "Tail";
            temp.transform.SetParent(SnakeBody.transform);

            ListOfSnakeElements.Add(temp);
        }
    }

    void FixedUpdate()
    {
        if (ListOfSnakeElements.Count < 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        SnakeControl();

        _timer += Time.fixedDeltaTime;
        if (_timer >= _speed)
        {
            Move();
            _timer = 0f;
        }
    }

    private void SnakeControl()
    {
        _moveVertikal = Input.GetAxis("Vertical");
        _moveHorizontal = Input.GetAxis("Horizontal");

        if (_moveVertikal > 0 && _vertical && _checkDirection)
        {
            _vector = Vector3.up;
            _headDirection = Quaternion.Euler(0f, 0f, 0);
            _vertical = false;
            _horizontal = true;
            _checkDirection = false;
        }

        if (_moveVertikal < 0 && _vertical && _checkDirection)
        {
            _vector = Vector3.down;
            _headDirection = Quaternion.Euler(0f, 0f, 180);
            _vertical = false;
            _horizontal = true;
            _checkDirection = false;
        }

        if (_moveHorizontal > 0 && _horizontal && _checkDirection)
        {
            _vector = Vector3.right;
            _headDirection = Quaternion.Euler(0f, 0f, -90);
            _vertical = true;
            _horizontal = false;
            _checkDirection = false;
        }

        if (_moveHorizontal < 0 && _horizontal && _checkDirection)
        {
            _vector = Vector3.left;
            _headDirection = Quaternion.Euler(0f, 0f, 90);
            _vertical = true;
            _horizontal = false;
            _checkDirection = false;
        }

        _moveVector = _vector;
    }

    private void Move()
    {
        _tempElement = ListOfSnakeElements[ListOfSnakeElements.Count - 1];
        _tempElement.transform.position = ListOfSnakeElements[0].transform.position + _moveVector;
        Teleport();
        _tempElement.GetComponent<SpriteRenderer>().sprite = HeadSprite;
        ListOfSnakeElements.RemoveAt(ListOfSnakeElements.Count - 1);
        ListOfSnakeElements[0].GetComponent<SpriteRenderer>().sprite = TailSprite;
        ListOfSnakeElements.Insert(0, _tempElement);
        ListOfSnakeElements[0].transform.rotation = _headDirection;
        Eat();
        _checkDirection = true;
    }

    private void Teleport()
    {
        if (_tempElement.transform.position.y > 9)
        {
            _tempElement.transform.position = new Vector3(_tempElement.transform.position.x, -8.5f, _tempElement.transform.position.z);
        }

        if (_tempElement.transform.position.y < -9)
        {
            _tempElement.transform.position = new Vector3(_tempElement.transform.position.x, 8.5f, _tempElement.transform.position.z);
        }

        if (_tempElement.transform.position.x > 16)
        {
            _tempElement.transform.position = new Vector3(-15.5f, _tempElement.transform.position.y, _tempElement.transform.position.z);
        }

        if (_tempElement.transform.position.x < -16)
        {
            _tempElement.transform.position = new Vector3(15.5f, _tempElement.transform.position.y, _tempElement.transform.position.z);
        }
    }

    private void Eat()
    {
        _headPosition = ListOfSnakeElements[0].transform.position;

        foreach (GameObject food in FoodGenerateScript.ListOfFood)
        {
            _foodPosition = food.transform.position;

            if (_foodPosition == _headPosition)
            {
                if (food.tag == "Cookie")
                {
                    FoodGenerateScript.RandowmTeleport(food);
                    GenerateSnake(1, ListOfSnakeElements[ListOfSnakeElements.Count - 1].transform.position);
                    break;
                }

                if (food.tag == "IceCube")
                {
                    FoodGenerateScript.RandowmTeleport(food);
                    Destroy(ListOfSnakeElements[ListOfSnakeElements.Count - 1]);
                    ListOfSnakeElements.RemoveAt(ListOfSnakeElements.Count - 1);
                    break;
                }

                if (food.tag == "SpeedUp")
                {
                    FoodGenerateScript.RandowmTeleport(food);
                    StartCoroutine(SpeedUpFunction());
                    break;
                }

                if (food.tag == "SpeedDown")
                {
                    FoodGenerateScript.RandowmTeleport(food);
                    StartCoroutine(SpeedDownFunction());
                    break;
                }

                if (food.tag == "Reverse")
                {
                    FoodGenerateScript.RandowmTeleport(food);
                    ListOfSnakeElements.Reverse();
                    _vector = ListOfSnakeElements[0].transform.position - ListOfSnakeElements[1].transform.position;
                    CheckSnakeDirection();
                    return;
                }
            }
        }

        for (int i = 3; i < ListOfSnakeElements.Count; i++)
        {
            _tailPosition = ListOfSnakeElements[i].transform.position;
            if (_headPosition == _tailPosition)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void CheckSnakeDirection()
    {
        if (_vector == Vector3.up)
        {
            _headDirection = Quaternion.Euler(0f, 0f, 0);
            _vertical = false;
            _horizontal = true;
            _checkDirection = false;
        }

        if (_vector == Vector3.down)
        {
            _headDirection = Quaternion.Euler(0f, 0f, 180);
            _vertical = false;
            _horizontal = true;
            _checkDirection = false;
        }

        if (_vector == Vector3.right)
        {
            _headDirection = Quaternion.Euler(0f, 0f, -90);
            _vertical = true;
            _horizontal = false;
            _checkDirection = false;
        }

        if (_vector == Vector3.left)
        {
            _headDirection = Quaternion.Euler(0f, 0f, 90);
            _vertical = true;
            _horizontal = false;
            _checkDirection = false;
        }
    }

    IEnumerator SpeedUpFunction()
    {

        _speed /= 2;

        yield return new WaitForSeconds(3);

        _speed *= 2;

        StopCoroutine(SpeedUpFunction());

    }

    IEnumerator SpeedDownFunction()
    {

        _speed *= 2;

        yield return new WaitForSeconds(3);

        _speed /= 2;

        StopCoroutine(SpeedDownFunction());

    }

    private void OnDestroy()
    {
        ListOfSnakeElements.Clear();
    }
}
