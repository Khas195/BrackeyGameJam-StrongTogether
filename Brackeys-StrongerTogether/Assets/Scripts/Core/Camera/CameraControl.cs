using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    GameObject cameraObject = null;
    [SerializeField]
    Camera mainCamera = null;
    [SerializeField]
    [ReadOnly]
    Vector3 mouseDownPos = Vector2.zero;
    [SerializeField]
    [ReadOnly]
    bool isMouseDown = false;
    [SerializeField]
    float cameraSpeed = 0;
    [SerializeField]
    float dragSpeed = 0;
    [SerializeField]
    float acceleration = 0.2f;
    [SerializeField]
    float decceleration = 0.2f;

    // Update is called once per frame
    void LateUpdate()
    {
        var direction = Vector3.zero;
        if (Input.GetMouseButton(0))
        {
            if (isMouseDown == false)
            {
                mouseDownPos = mainCamera.ScreenToViewportPoint(Input.mousePosition);
            }
            else
            {
                var newMousPos = mainCamera.ScreenToViewportPoint(Input.mousePosition);
                direction = (newMousPos - mouseDownPos).normalized;
                direction *= -1;

                var newDragPosition = cameraObject.transform.position;
                newDragPosition += dragSpeed * Time.deltaTime * direction;
                cameraObject.transform.position = newDragPosition;
                mouseDownPos = newMousPos;
            }
            isMouseDown = true;
        }


        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {

            direction.x = 1;
        }

        var curPosition = cameraObject.transform.position;
        curPosition += cameraSpeed * Time.deltaTime * direction;
        cameraObject.transform.position = curPosition;
    }
}
