using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //lista jednostek ktora moze byc poruszana
    static Dictionary<string, List<TacticsMove>> units = new Dictionary<string, List<TacticsMove>>();

    //kolejka tur, ktora wybiera czyj jest ruch
    static Queue<string> TurnKey = new Queue<string>();

    // kolejka aktualnego gracza
    static Queue<TacticsMove> turnTeam = new Queue<TacticsMove>();



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //inicjalizuj teamQueue
    static void InitializeTeamQueue()
    {
        List<TacticsMove> teamList = units[TurnKey.Peek()];

        foreach (TacticsMove unit in teamList)
        {
            turnTeam.Enqueue(unit);

            //StartTurn(); //todo
        }
    }
}
