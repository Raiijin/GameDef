using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    
    public bool current = false; //wybrana jednostka stojaca na danym klocku zmienia to na true
    public bool target = false; //target = true w przypadku gdy klocek jest w zasiegu ruchu wybranej jednostki
    public bool selectable = false; // status pozwalajacy na interakcje z klockiem

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

}
