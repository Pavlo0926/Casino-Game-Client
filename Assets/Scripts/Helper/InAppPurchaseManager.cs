using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAppPurchaseManager : MonoBehaviour
{
    public List<StoreProduct> products = new List<StoreProduct>();

    // Start is called before the first frame update
    void Start()
    {
        IAPManager.Instance.InitializeIAPManager(InitializeResultCallback);
    }

    private void InitializeResultCallback(IAPOperationStatus status, string message, List<StoreProduct> products)
    {
        this.products.Clear();
        this.products.AddRange(products);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
