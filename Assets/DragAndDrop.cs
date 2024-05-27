using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{

    //TODO: Fix the bug where you can clip an object under the ground by rotating another object on top of it
    // This is a product of the "up" item being kinematic while the "down" item isn't. I should probably impliment rotating before I address this in earnest.
    // - The lazy fix is just to increase the upPosition
    // NOTE: This script should be run by a container object that holds all the items in the scene or it fucks up the rotation. Also probably will make spawning items in and sending/recieving messages simpler?
    private GameObject selectedObject;
    private Rigidbody selectedObjectRb; 
    private float upPosition = 2.3f;

    public static ItemManager selectedItemManager;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(selectedObject == null)
            {
                RaycastHit hit = CastRay();

                if(hit.collider != null)
                {
                    if(!hit.collider.CompareTag("Draggable"))
                    {
                        return;
                    }

                    selectedObject = hit.collider.gameObject;
                    selectedObjectRb = selectedObject.GetComponent<Rigidbody>(); // Get Rigidbody component
                    selectedObjectRb.useGravity = false; // Disable gravity
                    

                    // Disable kinematic to allow physics to affect the object
                    selectedObjectRb.isKinematic = true;

                    //BIG breakthrough here! When we select the item in the Drag and Drop script we can access the item's variables. Now we just need to add this to the scanner...
                    selectedItemManager = selectedObject.GetComponent<ItemManager>();
                    Debug.Log(selectedItemManager.price);
                }
            }
            else
            {
                selectedObjectRb.useGravity = true; // Enable gravity
                selectedObjectRb.isKinematic = false;

                selectedObject = null;
                selectedObjectRb = null;
            }
        }

        if(selectedObject != null && selectedObjectRb != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, upPosition, worldPosition.z);

            if(Input.GetMouseButtonDown(1))
            {
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(selectedObject.transform.rotation.eulerAngles.x,
                                                                                 selectedObject.transform.rotation.eulerAngles.y,
                                                                                 selectedObject.transform.rotation.eulerAngles.z + 90f));
            }
        }
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePositionFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePositionNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

        Vector3 worldMousePositionFar = Camera.main.ScreenToWorldPoint(screenMousePositionFar);
        Vector3 worldMousePositionNear = Camera.main.ScreenToWorldPoint(screenMousePositionNear);

        RaycastHit hit;
        Physics.Raycast(worldMousePositionNear, worldMousePositionFar - worldMousePositionNear, out hit);

        return hit;
    }

}