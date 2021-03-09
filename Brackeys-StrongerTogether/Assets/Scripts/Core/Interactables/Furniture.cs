using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class FurnitureEvent
{
    public static string FURNITURE_INTERACT_EVENT = "FURNITURE_INTERACT";
    public static string TARGET_FURNITURE = "FURNITURE";
    public static string ZONE_ID = "ZONE_ID";

}
public class Furniture : IInteractable
{

    [BoxGroup("Furniture Data")]
    [SerializeField]
    [Required]
    SpriteRenderer highlightState = null;
    [BoxGroup("Furniture Data")]
    [SerializeField]
    [Required]
    Transform interactPlace = null;
    [BoxGroup("Furniture Data")]
    [SerializeField]
    int zoneID = 0;
    [BoxGroup("Furniture Data")]
    [SerializeField]
    [ReadOnly]
    bool lockHighlight = false;


    // Start is called before the first frame update
    void Start()
    {
        var transparentHighlight = highlightState.color;
        transparentHighlight.a = 0;
        highlightState.color = transparentHighlight;
    }


    public override void Defocus()
    {
        if (lockHighlight) return;
        base.Defocus();
        var transparentHighlight = highlightState.color;
        transparentHighlight.a = 0;
        highlightState.color = transparentHighlight;

    }

    public override void Focus()
    {
        if (lockHighlight) return;
        base.Focus();
        var transparentHighlight = highlightState.color;
        transparentHighlight.a = 1;
        highlightState.color = transparentHighlight;

    }
    public override bool Interact()
    {
        if (base.Interact() == false) return false;

        var dataPack = DataPool.GetInstance().RequestInstance();
        dataPack.SetValue(FurnitureEvent.TARGET_FURNITURE, this);
        dataPack.SetValue(FurnitureEvent.ZONE_ID, this.zoneID);
        PostOffice.SendData(dataPack, FurnitureEvent.FURNITURE_INTERACT_EVENT);
        DataPool.GetInstance().ReturnInstance(dataPack);
        return true;
    }
    public virtual void StartInteraction(IFurnitureUser user)
    {

    }
    public virtual void StopInteraction()
    {

    }

    public void SetActiveInteract(bool activeInteract)
    {
        this.lockHighlight = activeInteract;
    }

    public Vector2 GetInteractPlace()
    {
        return interactPlace.position;
    }
}
