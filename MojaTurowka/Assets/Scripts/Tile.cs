﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public bool walkable = true; //Czy po danym bloku można chodzić
    public bool current = false; //wybrana jednostka stojaca na danym klocku zmienia to na true
    public bool target = false; //true jeżeli jednostka wybierze to pole jako cel
    public bool selectable = false; // Pola na które gracz może się ruszyć

    public List<Tile> adjacencyList = new List<Tile>(); // lista pól sąsiadujących dla danego pola

    //Needed BFS (breadth first search)
    public bool visited = false; //czy pole było sprawdzane
    public Tile parent = null; //parent dla pola
    public int distance = 0; //jak daleko pole jest od pola na ktorym znajduje sie gracz

    private void Update()
    {   //zakoloruj odpowiednio klocki
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
         } else 
        if (target)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    /**
     * @desc resetuje parametry dla pola
     */
    public void Reset()
    {
        adjacencyList.Clear();

        current = false; 
        target = false; 
        selectable = false; 

        visited = false; 
        Tile parent = null; 
        distance = 0; 
    }

    /**
     * @desc znajdowanie sąsiednich pól
     */
    public void FindNeighbors(float jumpHeight) 
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight);
        CheckTile(-Vector3.forward, jumpHeight);
        CheckTile(Vector3.right, jumpHeight);
        CheckTile(-Vector3.right, jumpHeight);
    }

    public void CheckTile(Vector3 direction, float jumpHeight)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;

                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1)) //sprawdzanie czy nic nie znajduję się na polu
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }
}
