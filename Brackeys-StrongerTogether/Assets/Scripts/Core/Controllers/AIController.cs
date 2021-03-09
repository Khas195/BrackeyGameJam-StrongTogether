using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour, IObserver
{
    [SerializeField]
    Transform pivot = null;
    [SerializeField]
    Character2D character = null;
    [SerializeField]
    int designatedZone = 0;
    List<Vector3> waypoints = new List<Vector3>();
    int currentWaypointIndex = 0;
    Furniture currentInteractable = null;
    Furniture targetInteractable = null;

    [SerializeField] public AINeed AINeed { get; private set; }
    [SerializeField] public AISatisfaction AISatisfaction { get; private set; }


    private void Awake()
    {
        if(AINeed == null)
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
        if (waypoints.Count > 0 && currentWaypointIndex < waypoints.Count)
        {
            if (Vector2.Distance(pivot.transform.position, waypoints[currentWaypointIndex]) <= 0.1f)
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
            character.Move(0, 0);
        }
    }

    public void GoTo(Vector2 point)
    {
        waypoints.Clear();
        PathRequestManager.GetInstance().RequestPath(pivot.transform.position, point, OnPathFinished);
    }

    public void OnPathFinished(Vector3[] waypoints, bool pathfindSuccess)
    {
        if (pathfindSuccess)
        {
            this.waypoints.AddRange(waypoints);
            currentWaypointIndex = 1;
            if (currentInteractable != null)
            {
                currentInteractable.Defocus();
                currentInteractable.SetActiveInteract(false);
            }
            currentInteractable = targetInteractable;
            currentInteractable.Focus();
            currentInteractable.SetActiveInteract(true);
        }
        else
        {
            Debug.Log("Could not found path");
            if (targetInteractable != null)
            {
                targetInteractable.Defocus();
                targetInteractable.SetActiveInteract(false);
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
        if (targetInteractable != null)
        {
            targetInteractable.Defocus();
            currentInteractable.SetActiveInteract(false);
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
