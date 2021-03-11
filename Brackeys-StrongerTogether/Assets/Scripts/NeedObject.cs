using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
public class NeedObject : Furniture
{
    [SerializeField]
    [Required]
    NeedData needData;
    [SerializeField]
    [ReadOnly]
    bool isInUse = false;
    [SerializeField]
    [ReadOnly]
    float curTime = 0;
    [SerializeField]
    ISatisfier currentUser = null;


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
            if (curTime >= needData.SatisfyInterval)
            {
                currentUser.Satisfy(this.needData);
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
            LogHelper.Log("Someone is using " + this.needData);
            isInUse = true;
            curTime = 0;
            this.Focus();
            this.SetActiveInteract(true);
            this.currentUser = (ISatisfier)user;

        }
        catch (InvalidCastException e)
        {
            LogHelper.LogError("Past the wrong subtype of IFurnitureUser to NeedObject: " + e);
        }
    }
    [Button]
    public override void StopInteraction()
    {
        base.StopInteraction();
        if(currentUser!=null) currentUser.StopSatisfying(this.needData);
        LogHelper.Log(this.needData + " is turned off");
        isInUse = false;
        this.currentUser = null;
        this.SetActiveInteract(false);
        this.Defocus();
    }


}
