using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClick : MonoBehaviour
{
    [SerializeField]
    AIController testBummy = null;
    [SerializeField]
    LayerMask walkableMask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, walkableMask);
            if (hit.collider != null)
            {
                Debug.Log("Hit point: " + hit.point);
                Debug.Log("Mous Pos: " + mousePos);
                testBummy.GoTo(hit.point);
            }
        }
    }
}
