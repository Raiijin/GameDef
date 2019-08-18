using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TacticsMove
{
    public int health = 100; // Życie postaci
    public int strength = 25; // Siła ataku
    public bool inRange = false; // Czy postać jest w zasięgu ataku.
    public List<Player> enemiesList = new List<Player>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {   //zakoloruj odpowiednio postać
        if (inRange)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
     else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    //Funkcja zabiera życie atakowanej jednostce
    public void Attack(ref Player victim)
    {
        victim.health = -this.strength; 
    }

}
