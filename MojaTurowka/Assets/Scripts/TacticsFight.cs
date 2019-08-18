using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsFight : Player
{
    private void Update()
    {
        if (TacticsMove.afterMove) //sprawdzanie czy gracz już wykonał ruch
        {
            CheckTargets(transform.position, jumpHeight);
            if (enemiesList.Count != 0) //Jeśli znajdziesz przciwników to czekaj na wybranie celu
            {
                CheckMouseForEnemy();
            }
            else //Jeśli nie znajdziesz przeciwników zakończ ruch
            {
                TacticsMove.afterMove = false;
                EndTurn();
            }
        }
    }
    public void CheckTargets(Vector3 pos, float jumpHeight)
    {
        enemiesList.Clear();
        SearchForTargets(Vector3.forward, jumpHeight);
        SearchForTargets(-Vector3.forward, jumpHeight);
        SearchForTargets(Vector3.right, jumpHeight);
        SearchForTargets(-Vector3.right, jumpHeight);
    }

    // Ta klasa powinna znajdować przeciwników w pobliżu i działa podobnie jak znajdowanie pool #NieDziała
    public void SearchForTargets(Vector3 direction, float jumpHeight)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Player enemy = item.GetComponent<Player>();
            RaycastHit hit;

                if (Physics.Raycast(enemy.transform.position, Vector3.up, out hit, 1)) //sprawdzanie czy przeciwnik jest na polu
                {
                enemy.inRange = true;
                enemiesList.Add(enemy);
                }
        }       
    }

    void CheckMouseForEnemy() //sprawdz czy znajdziesz klikniecie myszka
    {
        if (Input.GetMouseButtonDown(0)) //if LPM jest wciśnięty 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) //rob cos jeżeli ray natknie się na collider
            {
                //tutaj programujemy reakcje jednostek miedzy soba, podgladanie statystyk, reakcje miedzy nimi 

                if (hit.collider.tag == "Player") //jeśli collider będzie miał tag player
                {
                    Player p = hit.collider.GetComponent<Player>();
                    if (p.inRange) //jeśli przeciwnik jest w zasięgu
                    {
                        Player active = GetComponent<Player>();
                        active.Attack(ref p); //Wywołaj funkcje atak
                    }
                }
            }

        }

    }
}
