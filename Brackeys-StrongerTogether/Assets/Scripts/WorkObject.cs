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
    private void Start()
    {
        curTime = 0;
    }

    private void Update()
    {
        if (isInUse)
        {
            curTime += Time.deltaTime;
            if (curTime >= progressUpdateInterval && finalProject.IsProjectDone() == false)
            {
                finalProject.IncreaseProgress(workAmount);
                curTime = 0;
            }
        }
    }

    [Button]
    public override void StartInteraction()
    {
        base.StartInteraction();
        LogHelper.Log("Someone is working at " + this);
        isInUse = true;
        curTime = 0;
        this.Focus();
        this.SetActiveInteract(true);
    }
    [Button]
    public override void StopInteraction()
    {
        base.StopInteraction();
        LogHelper.Log("Work Station is turned off" + this);
        isInUse = false;
        this.SetActiveInteract(false);
        this.Defocus();
    }
}
