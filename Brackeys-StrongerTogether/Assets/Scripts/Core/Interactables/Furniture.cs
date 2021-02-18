using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class FurnitureEvent
{
    public static string FURNITURE_INTERACT_EVENT = "FURNITURE_INTERACT";
    public static string TARGET_FURNITURE = "FURNITURE";
    public static string FURNITURE_INTERACT_PLACE = "INTERACT_PLACE";

}
public class Furniture : IInteractable
{
    [SerializeField]
    [Required]
    SpriteRenderer normalState = null;
    [SerializeField]
    [Required]
    SpriteRenderer highlightState = null;
    [SerializeField]
    [Required]
    Transform interactPlace = null;


    // Start is called before the first frame update
    void Start()
    {
        var transparentHighlight = highlightState.color;
        transparentHighlight.a = 0;
        highlightState.color = transparentHighlight;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Defocus()
    {
        base.Defocus();
        var transparentHighlight = highlightState.color;
        transparentHighlight.a = 0;
        highlightState.color = transparentHighlight;

    }

    public override void Focus()
    {
        base.Focus();
        var transparentHighlight = highlightState.color;
        transparentHighlight.a = 1;
        highlightState.color = transparentHighlight;

    }
    public override bool Interact()
    {
        if (base.Interact() == false) return false;

        var dataPack = DataPool.GetInstance().RequestInstance();
        dataPack.SetValue(FurnitureEvent.FURNITURE_INTERACT_PLACE, this.interactPlace);
        dataPack.SetValue(FurnitureEvent.TARGET_FURNITURE, this);
        PostOffice.SendData(dataPack, FurnitureEvent.FURNITURE_INTERACT_EVENT);
        DataPool.GetInstance().ReturnInstance(dataPack);
        return true;
    }
    private void OnMouseOver()
    {
        Focus();
    }
    private void OnMouseExit()
    {
        Defocus();
    }
}
