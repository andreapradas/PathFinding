using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav_Patrol : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject waypointParent; //Parent que contiene a los hijos
    private GameObject[] waypoints; //Los hijos WP01...
    private int waypointIndex = 0;
    private int maxWaypoints;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        maxWaypoints = waypointParent.transform.childCount; //Obtener el numero de waypoint que hay
        waypoints = new GameObject[maxWaypoints];
        for(int i = 0; i < maxWaypoints; i++)
        {
            waypoints[i] = waypointParent.transform.GetChild(i).gameObject;
        }
       GoToNextWayPoint(); 
    }
    void Update()
    {
        if (agent.remainingDistance < 0.1) //Si esta llegando al waypoint
        {
            waypointIndex = (waypointIndex + 1) % maxWaypoints; 
            GoToNextWayPoint(); 
        }
    }
    private void GoToNextWayPoint()
    {
        agent.SetDestination(waypoints[waypointIndex].transform.position);
    }
}
