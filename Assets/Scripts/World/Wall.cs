using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //Transform wall;
    //UnityEngine.AI.NavMeshAgent nav;

    // Use this for initialization
    void Start ()
    {
        //wall = GameObject.FindGameObjectWithTag("Wall").transform;
        //nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
	
    void OnMouseDown()
    {
        //nav.SetDestination(wall.position);
    }
}
