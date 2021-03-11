using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
public class WorkObject : Furniture
{
    [BoxGroup("Work Station Data")]
    [SerializeField]
    [Required]
    FinalProject finalProject = null;

    [BoxGroup("Work Station Data")]
    [SerializeField]
    float progressUpdateInterval = 0;

    [BoxGroup("Work Station Data")]
    [SerializeField]
    float workAmount = 0;
    [BoxGroup("Work Station Data")]
    [SerializeField]
    [ReadOnly]
    bool isInUse = false;
    [BoxGroup("Work Station Data")]
    [SerializeField]
    [ReadOnly]
    float curTime = 0;
    IWorker curUser = null;

    protected override void Start()
    {
        base.Start();
        curTime = 0;
    }

    private void Update()
    {
        if (isInUse)
        {
            curTime += Time.deltaTime;
            if (curTime >= progressUpdateInterval && finalProject.IsProjectDone() == false)
            {
                if (curUser.HasActiveNeed())
                {
                    finalProject.IncreaseProgress(workAmount / 2f);
                }
                else
                {
                    finalProject.IncreaseProgress(workAmount);
                }
                curTime = 0;
            }
        }
    }

    [Button]
    public override void StartInteraction(IFurnitureUser user)
    {
        try
        {
            base.StartInteraction(user);
            LogHelper.Log("Someone is working at " + this);
            isInUse = true;
            curTime = 0;
            this.Focus();
            this.SetActiveInteract(true);
            this.curUser = (IWorker)user;

        }
        catch (InvalidCastException e)
        {
            LogHelper.LogError("Past the wrong subtype of IFurnitureUser to WorkObject: " + e);
        }
    }
    [Button]
    public override void StopInteraction()
    {
        base.StopInteraction();
        LogHelper.Log("Work Station is turned off" + this);
        isInUse = false;
        this.SetActiveInteract(false);
        this.Defocus();
        this.curUser = null;
    }
}
