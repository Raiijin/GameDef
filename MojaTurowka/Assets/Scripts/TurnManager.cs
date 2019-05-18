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
        Debug.Log(turnTeam.Count);
     if (turnTeam.Count==0) //dzieje się na początku gdy jeszcze nie ma pierwszego ruchu | gdy nikt się nie ruszył zacznij pierwszą turę
        {
            InitializeTeamQueue();
        }
    }

    //inicjalizuj teamQueue
    static void InitializeTeamQueue()
    {
        List<TacticsMove> teamList = units[TurnKey.Peek()];

        foreach (TacticsMove unit in teamList)
        {
            turnTeam.Enqueue(unit);

            
        }

        StartTurn(); 
    }

    static void StartTurn()
    {
        if (turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn()
    {
        TacticsMove unit = turnTeam.Dequeue();
        unit.EndTurn();

        if (turnTeam.Count>0)
        {
            StartTurn();
        }
        else
        {
            string team = TurnKey.Dequeue();

            TurnKey.Enqueue(team);
            InitializeTeamQueue();
        }
    }


    //add unit 
    public static void AddUnit(TacticsMove unit)
    {
        List<TacticsMove> list;

        if (!units.ContainsKey(unit.tag))
        {
            list = new List<TacticsMove>();
            units[unit.tag] = list;

            if (!TurnKey.Contains(unit.tag))
            {
                TurnKey.Enqueue(unit.tag);
            }
        }
        else
        {
            list = units[unit.tag];
        }
        list.Add(unit);
    }
}
