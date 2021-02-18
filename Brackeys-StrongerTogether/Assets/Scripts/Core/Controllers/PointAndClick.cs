using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClick : MonoBehaviour
{
    [SerializeField]
    LayerMask interactableMask;
    [SerializeField]
    IInteractable focusInteractable = null;

    // Update is called once per frame
    void Update()
    {
        DetectInteractable();
        if (Input.GetMouseButtonDown(0))
        {
            TryInteract();
        }
    }


    private void DetectInteractable()
    {
        RaycastHit2D hit;
        if (IsSeeingInteractable(out hit))
        {
            HandleInteractable(hit);
        }
        else
        {
            DefocusInteractable();
        }
    }

    private bool IsSeeingInteractable(out RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, interactableMask);
        return hit.collider != null;
    }

    private void TryInteract()
    {
        if (focusInteractable != null)
        {
            focusInteractable.Interact();
        }
    }

    private void HandleInteractable(RaycastHit2D hit)
    {
        var interactable = hit.collider.gameObject.GetComponent<IInteractable>();

        Debug.Log("Hit interactable");
        if (interactable != null)
        {
            FocusInteractable(interactable);
        }
        else
        {
            DefocusInteractable();
        }
    }

    private void FocusInteractable(IInteractable interactable)
    {
        Debug.Log("Focus " + interactable.name);
        if (focusInteractable != interactable)
        {
            interactable.Focus();
            DefocusInteractable();
            focusInteractable = interactable;
        }
    }

    private void DefocusInteractable()
    {
        Debug.Log("Doesn't hit anything");
        if (focusInteractable != null)
        {
            focusInteractable.Defocus();
        }
        focusInteractable = null;
    }

}
