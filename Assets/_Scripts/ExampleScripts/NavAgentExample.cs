using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{
   public AIWaypointNetwork WaypointNetwork = null;
   private NavMeshAgent _navAgent = null;
   public int CurrentIndex = 0;
   public bool HasPath = false;
   public bool PathPending = false;

   private void Start()
   {
      _navAgent = GetComponent<NavMeshAgent>();

      if (WaypointNetwork == null) return;
      
      SetNextDestination(false);
   }

   void SetNextDestination(bool increment)
   {
      if (!WaypointNetwork) return;

      int incStep = increment ? 1 : 0;
      Transform nextWaypointTransform = null;

      while (nextWaypointTransform == null)
      {
         int nextWaypoint = (CurrentIndex + incStep >= WaypointNetwork.Waypoints.Count) ? 0 : CurrentIndex + incStep;
         nextWaypointTransform = WaypointNetwork.Waypoints[nextWaypoint];

         if (nextWaypointTransform != null)
         {
            CurrentIndex = nextWaypoint;
            _navAgent.destination = nextWaypointTransform.position;
            return;
         }
      }

      CurrentIndex++;
   }

   private void Update()
   {
      HasPath = _navAgent.hasPath;
      PathPending = _navAgent.pathPending;

      if (!HasPath && !PathPending)
      {
         SetNextDestination(true);
      }
   }
}
