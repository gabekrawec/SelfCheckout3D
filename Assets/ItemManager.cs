using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public double price;

    void Start()
    {
        price = UnityEngine.Random.Range(0.99f, 59.99f);
        price = Math.Round(price, 2);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
