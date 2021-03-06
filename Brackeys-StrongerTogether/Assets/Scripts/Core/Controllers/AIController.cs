﻿using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class AIController : MonoBehaviour, IObserver
{
    [SerializeField]
    Transform pivot = null;
    [SerializeField]
    Character2D character = null;
    [SerializeField]
    int designatedZone = 0;
    [SerializeField]
    [ReadOnly]
    bool isMovingThroughWaypoints = false;
    [SerializeField]
    List<Vector3> waypoints = new List<Vector3>();
    int currentWaypointIndex = 0;
    Furniture currentInteractable = null;
    Furniture targetInteractable = null;


    [SerializeField] public AINeed AINeed { get; private set; }
    [SerializeField] public AISatisfaction AISatisfaction { get; private set; }


    private void Awake()
    {
        if (AINeed == null)
        {
            AINeed = GetComponent<AINeed>();
        }

        if (AISatisfaction == null)
        {
            AISatisfaction = GetComponent<AISatisfaction>();
        }

        AINeed.Init(AISatisfaction);
    }

    private void Start()
    {
        PostOffice.Subscribes(this, FurnitureEvent.FURNITURE_INTERACT_EVENT);
    }

    private void FixedUpdate()
    {
        if (HasActivePath())
        {
            if (IsAtCurrentWaypoint())
            {
                currentWaypointIndex++;
            }
            else
            {
                Vector2 moveDir = (waypoints[currentWaypointIndex] - pivot.transform.position);
                moveDir = moveDir.normalized;
                character.Move(moveDir.x, moveDir.y);
            }
        }
        else
        {
            currentWaypointIndex = 0;
            waypoints.Clear();

            if (isMovingThroughWaypoints)
            {
                isMovingThroughWaypoints = false;
                if (currentInteractable != null)
                {
                    this.currentInteractable.StartInteraction(this.AINeed);
                }
            }
        }
    }

    private bool IsAtCurrentWaypoint()
    {
        return Vector2.Distance(pivot.transform.position, waypoints[currentWaypointIndex]) <= 0.1f;
    }

    private bool HasActivePath()
    {
        return waypoints.Count > 0 && currentWaypointIndex < waypoints.Count;
    }

    public void GoTo(Vector2 point)
    {
        waypoints.Clear();
        PathRequestManager.GetInstance().RequestPath(pivot.transform.position, point, OnPathFinished);
    }

    public void OnPathFinished(Vector3[] newWaypoints, bool pathfindSuccess)
    {
        if (pathfindSuccess)
        {
            this.waypoints.AddRange(newWaypoints);
            currentWaypointIndex = 1;
            if (currentInteractable != null)
            {
                currentInteractable.Defocus();
                currentInteractable.SetActiveInteract(false);
                currentInteractable.StopInteraction();
            }
            currentInteractable = targetInteractable;
            currentInteractable.Focus();
            currentInteractable.SetActiveInteract(true);
            isMovingThroughWaypoints = true;
        }
        else
        {
            Debug.Log("Could not found path");
            if (targetInteractable != null)
            {
                targetInteractable.SetActiveInteract(false);
                targetInteractable.Defocus();
                targetInteractable = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (waypoints.Count > 0)
        {
            Gizmos.DrawCube(waypoints[0], Vector3.one * (Grid.GetInstance().GetNodeDiameter() - Grid.GetInstance().GetTileOffset()));
            Gizmos.DrawLine(waypoints[0], this.pivot.transform.position);
        }

        for (int i = 1; i < waypoints.Count; i++)
        {
            Gizmos.DrawCube(waypoints[i], Vector3.one * (Grid.GetInstance().GetNodeDiameter() - Grid.GetInstance().GetTileOffset()));
            Gizmos.DrawLine(waypoints[i - 1], waypoints[i]);
        }
    }

    public void ReceiveData(DataPack pack, string eventName)
    {
        if (eventName.Equals(FurnitureEvent.FURNITURE_INTERACT_EVENT))
        {
            var zoneID = pack.GetValue<int>(FurnitureEvent.ZONE_ID);
            if (zoneID == this.designatedZone)
            {
                var interactObject = pack.GetValue<Furniture>(FurnitureEvent.TARGET_FURNITURE);
                InitiateInteractSequence(interactObject);
            }
        }
    }

    private void InitiateInteractSequence(Furniture furniture)
    {
        if(targetInteractable == furniture)
        {
            return;
        }

        if (targetInteractable != null)
        {
            targetInteractable.Defocus();
            if(currentInteractable!=null)
            {
                currentInteractable.SetActiveInteract(false);
            }
            targetInteractable = null;
        }
        targetInteractable = furniture;
        this.GoTo(furniture.GetInteractPlace());
    }
    private void OnDestroy()
    {
        PostOffice.Unsubscribes(this, FurnitureEvent.FURNITURE_INTERACT_EVENT);
    }

}
