using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveScript : MonoBehaviour
{

    //public SpriteRenderer _spriteRenderer;

    public Sprite HeadSprite;
    public Sprite TailSprite;

    public GameObject Reverse;
    public GameObject SpeedUp;
    public GameObject SpeedDown;
    public GameObject Cookie;
    public GameObject IceCube;
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
    private Vector3 _foodSpawnPosition;
    private Vector2 _foodPosition;
    private Vector2 _headPosition;
    private Vector2 _tailPosition;


    private Quaternion _headDirection;

    private bool _vertical = true;
    private bool _horizontal = true;
    private bool _checkDirection;

    private List<GameObject> _listOfElements = new List<GameObject>();
    private List<GameObject> _listOfFood = new List<GameObject>();

    void Start()
    {
        GenerateSnake(_snakeLeight, SnakeBody.transform.position);
        GenerateFood();
        _speed = MoveTime;
    }

    void FixedUpdate()
    {
        if (_listOfElements.Count < 3)
            Application.LoadLevel(Application.loadedLevel);

        _moveVertikal = Input.GetAxis("Vertical");
        _moveHorizontal = Input.GetAxis("Horizontal");

        if (_moveVertikal > 0 && _vertical && _checkDirection)
        {
            //_headDirection = Quaternion.identity;
            _vector = Vector3.up;
            _headDirection = Quaternion.Euler(0f, 0f, 0);
            _vertical = false;
            _horizontal = true;
            _checkDirection = false;
        }

        if (_moveVertikal < 0 && _vertical && _checkDirection)
        {
            //_headDirection = Quaternion.identity;
            _vector = Vector3.down;
            _headDirection = Quaternion.Euler(0f, 0f, 180);
            _vertical = false;
            _horizontal = true;
            _checkDirection = false;
        }

        if (_moveHorizontal > 0 && _horizontal && _checkDirection)
        {
            //_headDirection = Quaternion.identity;
            _vector = Vector3.right;
            _headDirection = Quaternion.Euler(0f, 0f, -90);
            _vertical = true;
            _horizontal = false;
            _checkDirection = false;
        }

        if (_moveHorizontal < 0 && _horizontal && _checkDirection)
        {
            //_headDirection = Quaternion.identity;
            _vector = Vector3.left;
            _headDirection = Quaternion.Euler(0f, 0f, 90);
            _vertical = true;
            _horizontal = false;
            _checkDirection = false;
        }

        _moveVector = _vector;
        _timer += Time.fixedDeltaTime;
        if (_timer >= _speed)
        {
            Move();
            _timer = 0f;
        }
    }

    private void GenerateSnake(int snakeLeight, Vector3 position)
    {
        for (int i = 0; i < snakeLeight; i++)
        {
            GameObject temp = new GameObject();
            //0.046295
            temp.transform.localScale = new Vector3(0.046295f, 0.046295f, 1f);
            temp.transform.localPosition = position;
            temp.AddComponent<SpriteRenderer>();
            temp.GetComponent<SpriteRenderer>().sprite = TailSprite;
            temp.tag = "Tail";
            temp.transform.SetParent(SnakeBody.transform);

            _listOfElements.Add(temp);
        }
    }

    private void GenerateFood()
    {
        /*
        System.Random rand = new System.Random();
        _foodSpawnPosition = new Vector3(rand.Next(-15, 16) - 0.5f, rand.Next(-8, 9) - 0.5f, -1f);
        GameObject cookieClone = Instantiate(Cookie, _foodSpawnPosition, Quaternion.identity);
        _listOfFood.Add(cookieClone);

        _foodSpawnPosition = new Vector3(rand.Next(-15, 16) - 0.5f, rand.Next(-8, 9) - 0.5f, -1f);
        GameObject iceCube = Instantiate(IceCube, _foodSpawnPosition, Quaternion.identity);
        _listOfFood.Add(iceCube);

        _foodSpawnPosition = new Vector3(rand.Next(-15, 16) - 0.5f, rand.Next(-8, 9) - 0.5f, -1f);
        GameObject speedUp = Instantiate(SpeedUp, _foodSpawnPosition, Quaternion.identity);
        _listOfFood.Add(speedUp);

        _foodSpawnPosition = new Vector3(rand.Next(-15, 16) - 0.5f, rand.Next(-8, 9) - 0.5f, -1f);
        GameObject speedDown = Instantiate(SpeedDown, _foodSpawnPosition, Quaternion.identity);
        _listOfFood.Add(speedDown);
        */
        System.Random rand = new System.Random();
        RandowmSpawn(rand, Reverse);
        RandowmSpawn(rand, Cookie);
        RandowmSpawn(rand, IceCube);
        RandowmSpawn(rand, SpeedUp);
        RandowmSpawn(rand, SpeedDown);
    }

    private void RandowmSpawn(System.Random rand, GameObject foodGameObject)
    {
        //System.Random rand = new System.Random();
        bool checkSpawn = false;

        _foodSpawnPosition = new Vector3(rand.Next(-15, 16) - 0.5f, rand.Next(-8, 9) - 0.5f, -1f);

        foreach (GameObject food in _listOfFood)
        {
            if (food.transform.position == _foodSpawnPosition)
            {
                checkSpawn = true;
                break;
            }
        }

        foreach (GameObject tail in _listOfElements)
        {
            if (tail.transform.position == _foodSpawnPosition)
            {
                checkSpawn = true;
                break;
            }
        }

        if (checkSpawn)
            RandowmSpawn(rand, foodGameObject);

        GameObject foodClone = Instantiate(foodGameObject, _foodSpawnPosition, Quaternion.identity);
        _listOfFood.Add(foodClone);
    }

    private void RandowmTeleport(GameObject foodGameObject)
    {
        bool checkSpawn = true;

        Vector3 tempPosition = new Vector3(Random.Range(-15, 16) - 0.5f, Random.Range(-8, 9) - 0.5f, -1f);
        while (checkSpawn)
        {

            checkSpawn = false;

            for (int i = 0; i < _listOfFood.Count; i++)
            {
                if (tempPosition == _listOfFood[i].transform.position)
                {
                    tempPosition = new Vector3(Random.Range(-4, 4) - 0.5f, Random.Range(-4, 4) - 0.5f, -1f);
                    i = 0;
                    checkSpawn = true;
                    break;
                }
            }

            for (int i = 0; i < _listOfElements.Count; i++)
            {
                if (tempPosition == _listOfElements[i].transform.position)
                {
                    tempPosition = new Vector3(Random.Range(-4, 4) - 0.5f, Random.Range(-4, 4) - 0.5f, -1f);
                    i = 0;
                    checkSpawn = true;
                    break;
                }
            }
        }

        foodGameObject.transform.position = tempPosition;
        /*
        foreach (GameObject foodEated in _listOfFood)
        {
            if (tempPosition == foodEated.transform.position)
            {
                checkSpawn = true;
                Debug.Log("Eda Ok");
                break;
            }
        }

        foreach (GameObject tail in _listOfElements)
        {
            if (tempPosition == tail.transform.position)
            {
                checkSpawn = true;
                Debug.Log("Zmeya Ok");
                break;
            }
        }
        
        if (checkSpawn)
            RandowmTeleport(foodGameObject);
        foodGameObject.transform.position = tempPosition;
        */
    }

    private void Move()
    {
        //print(_moveVector);
        _tempElement = _listOfElements[_listOfElements.Count - 1];
        _tempElement.transform.position = _listOfElements[0].transform.position + _moveVector;
        Teleport();
        _tempElement.GetComponent<SpriteRenderer>().sprite = HeadSprite;
        _listOfElements.RemoveAt(_listOfElements.Count - 1);
        _listOfElements[0].GetComponent<SpriteRenderer>().sprite = TailSprite;
        _listOfElements.Insert(0, _tempElement);
        _listOfElements[0].transform.rotation = _headDirection;
        Eat();
        _checkDirection = true;
        //_listOfElements.Last();
        //_listOfElements[0].transform.position = _listOfElements[1].transform.position;  //_listOfElements[1].transform.position +  _moveVector;

    }

    private void Eat()
    {
        System.Random rand = new System.Random();

        _headPosition = _listOfElements[0].transform.position;

        foreach (GameObject food in _listOfFood)
        {
            _foodPosition = food.transform.position;

            if (_foodPosition == _headPosition)
            {
                if (food.tag == "Cookie")
                {
                    RandowmTeleport(food);
                    GenerateSnake(1, _listOfElements[_listOfElements.Count - 1].transform.position);
                    break;
                }

                if (food.tag == "IceCube")
                {
                    RandowmTeleport(food);
                    Destroy(_listOfElements[_listOfElements.Count - 1]);
                    _listOfElements.RemoveAt(_listOfElements.Count - 1);
                    break;
                }

                if (food.tag == "SpeedUp")
                {
                    RandowmTeleport(food);
                    StartCoroutine(SpeedUpFunction());
                    break;
                }

                if (food.tag == "SpeedDown")
                {
                    RandowmTeleport(food);
                    StartCoroutine(SpeedDownFunction());
                    break;
                }

                if (food.tag == "Reverse")
                {
                    RandowmTeleport(food);
                    _listOfElements.Reverse();
                    _vector = _listOfElements[0].transform.position - _listOfElements[1].transform.position;
                    CheckSnakeDirection();
                    return;
                }
            }
        }

        for (int i = 3; i < _listOfElements.Count; i++)
        {
            _tailPosition = _listOfElements[i].transform.position;
            if (_headPosition == _tailPosition)
            {
                Application.LoadLevel(Application.loadedLevel);
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
}
