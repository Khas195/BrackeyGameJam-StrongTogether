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
    IUser currentUser = null;
    private void Start()
    {
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
    public override void StartInteraction()
    {
        base.StartInteraction();
        LogHelper.Log("Someone is using " + this.needData);
        isInUse = true;
        curTime = 0;
        this.Focus();
        this.SetActiveInteract(true);
    }
    public void StartInteraction(IUser user)
    {
        base.StartInteraction();
        this.currentUser = user;
    }
    [Button]
    public override void StopInteraction()
    {
        base.StopInteraction();
        LogHelper.Log(this.needData + " is turned off");
        isInUse = false;
        this.currentUser = null;
        this.SetActiveInteract(false);
        this.Defocus();
    }


}
