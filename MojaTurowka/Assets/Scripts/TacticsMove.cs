using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{
    List<Tile> selectableTiles = new List<Tile>(); //lista pól które gracz może wybrać
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>(); //Droga po której jesdnostka będzie się poruszać
    Tile currentTile;
    
    public int move = 5; //zasięg ruchu
    public float jumpHeight = 2; //wysokość skoku/spadku
    public float moveSpeed = 2; //jak szybko jednostka będzie się poruszać przez pola

    Vector3 velocity = new Vector3(); //jak szybko gracz się porusza;
    Vector3 heading = new Vector3(); //kierunek w którym porusza się jednostka

    float halfHeight = 0; //wskazuje gdzie znajduje się gracz nad polem

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

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
}
