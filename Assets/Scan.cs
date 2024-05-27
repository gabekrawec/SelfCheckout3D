using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scan : MonoBehaviour
{   
    [SerializeField] private float maxLength = 2.5f;
    [SerializeField] private Transform startPos, endPos; 
    [SerializeField] private LineRenderer laser;

    public TextMeshPro monitorText;

    private AudioSource audioSource;
    private Ray ray;
    public RaycastHit hitInfo;
    

    private bool isScanned = false;
    private bool itemOnScanner = false;
    private double totalCost = 0;
   
    private void Awake() 
    {
        ray.origin = startPos.position;
        ray.direction = startPos.up;
        audioSource = GetComponent<AudioSource>();
    }

    void Update() 
    {       
        if(Physics.Raycast(ray, out hitInfo))
        {
            endPos.position = hitInfo.point;

            if(hitInfo.collider != null && hitInfo.collider.CompareTag("Barcode") && isScanned == false)
            {
                LogScan();
            }

            //Prevents multiple scans per frame
            if(hitInfo.collider != hitInfo.collider.CompareTag("Barcode"))
            {
                isScanned = false;
            }
            //This seems silly right now because it's just a button press, but once you need to enter an item code, you won't be able to just press a button on a scannable item bc it won't have a valid code.
            //The real issue is that since the price is determined by the item that currently has focus, if you put something on the scale then click something else, that other item's price comes up when you press the button.
            if(hitInfo.collider != null)
            {
                //This SHOULD work bc since we have gravity an item on the laser is the same as an item on the scanner
                itemOnScanner = true;
            }
        }
        else
        {
            endPos.position = ray.origin + ray.direction * maxLength;
            itemOnScanner = false;
        }
    }

    void LateUpdate() 
    {
        List<Vector3> pos = new List<Vector3>();
        pos.Add(startPos.transform.position);
        pos.Add(endPos.transform.position);
        laser.startWidth = 0.1f;
        laser.endWidth = 0.1f;
        laser.SetPositions(pos.ToArray());
        laser.useWorldSpace = true;
    }

    public void LogScan()
    {
        audioSource.Play();
        isScanned = true;
        totalCost += DragAndDrop.selectedItemManager.price;
        monitorText.SetText("Total:" + totalCost.ToString("C"));
        
    }

    public void LogNonScannableItem()
    {
        if(itemOnScanner)
        {
            ItemManager item = hitInfo.collider.gameObject.GetComponent<ItemManager>();

            audioSource.Play();
            totalCost += item.price;
            monitorText.SetText("Total:" + totalCost.ToString("C"));
        } 
    }
}