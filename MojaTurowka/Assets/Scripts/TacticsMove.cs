using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{
    public bool turn = false;

    List<Tile> selectableTiles = new List<Tile>(); //lista pól które gracz może wybrać
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>(); //Droga po której jesdnostka będzie się poruszać
    Tile currentTile;

    public bool moving = false;
    public static bool afterMove = false;
    public int move = 5; //zasięg ruchu
    public float jumpHeight = 2; //wysokość skoku/spadku
    public float moveSpeed = 7; //jak szybko jednostka będzie się poruszać przez pola

    Vector3 velocity = new Vector3(); //jak szybko gracz się porusza;
    Vector3 heading = new Vector3(); //kierunek w którym porusza się jednostka

    float halfHeight = 0; //wskazuje gdzie znajduje się gracz nad polem

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
    }

    /**
     * @desc znajdowanie currentTile i oznaczenie jej
     */
    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    /**
     * @desc zwraca wybrane pole
     */
    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    /**
     * @desc tworzenie listy możliwych pól
     */
    public void ComputeAdjacencyLists()
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight);
        }
    }

    /**
     * @desc szukanie pól na które można się przemieścić
     */
    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists();
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile) //wyznacz sciezke do celu
    {
        path.Clear();
        tile.target = true;
        moving = true; // jak tam dojdziesz zakoncz ruch

        Tile next = tile; // pobieraj nastepny klocek
        Debug.Log(path.Count);
        while (next !=null) //idz oznaczonymi klockami dopoki nie zgubisz parenta
        {
            path.Push(next);
            Debug.Log(path.Count);
            next = next.parent; //parent to klocek nastepny w kolejce (trase mierzy od celu do jednostki, dlatego parent jest pozniejszy niz tile podstawowy)
        }

    }

    public void Move()
    {
        if (path.Count>0) //poruszaj sie tylko wtedy kiedy pozostala jakakolwiek sciezka do celu
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;



            target.y += halfHeight+t.GetComponent<Collider>().bounds.extents.y; //ustaw gracza POWYZEJ klocka - bez tego jednostka bedzie wchodzic w plansze

            if (Vector3.Distance(transform.position,target)>=0.05f) //oblicz odległość jednostki od klocka
            {
                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = heading; //wybierz kierunek w ktorym jednostka ma sie poruszac
                transform.position += velocity * Time.deltaTime; //update position
            }
            else
            {
                //jezeli zblizysz sie wystarczajaco blisko srodka (ponizej 0.05) to wycecntruj pozycje jednostki
                transform.position = target;
                path.Pop(); // wyrzuc ten klocek z llisty Tile
            }
        }
        else
        {
            afterMove = true;
            RemoveSelectableTiles();
            moving = false;          
        }
    }

    protected void RemoveSelectableTiles() // usuwamy wszystkie znaczniki selectable
    {

        if (currentTile!=null)
        {
            currentTile.current = false;
            currentTile = null;
        }
        foreach (Tile tile in selectableTiles) //reset w tabeli
        {
            tile.Reset();
        }

        selectableTiles.Clear(); //zerowanie selectable tiles
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    public void BeginTurn()
    {
        turn = true;
    }

    public void EndTurn()
    {
        turn = false;
    }
}
