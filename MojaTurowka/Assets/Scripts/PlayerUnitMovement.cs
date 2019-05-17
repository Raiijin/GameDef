using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitMovement : TacticsMove
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

        if (!turn)
        {
            return;
        }

        if (!moving)
        {
            FindSelectableTiles();
            CheckMouse();
        }
        else
        {
            Move();
        }
    }

    void CheckMouse() //sprawdz czy znajdziesz klikniecie myszka
    {
        if (Input.GetMouseButtonDown(0)) //if LPM jest wciśnięty 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray,out hit)) //rob cos jeżeli ray natknie się na collider
            {
                //tutaj programujemy reakcje jednostek miedzy soba, podgladanie statystyk, reakcje miedzy nimi 

                if (hit.collider.tag=="Tile") //jeśli collider będzie miał tag tile
                {
                    Tile t = hit.collider.GetComponent<Tile>();
                    t.target = true;
                    
                    if (t.selectable) //jeśli klocek jest selectable, oznacz go jako target (zmien jego kolor)
                    {
                        
                        MoveToTile(t);
                    }
                }
            }

        }
    }
}
