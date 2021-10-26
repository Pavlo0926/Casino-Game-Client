using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipStoreController : MonoBehaviour
{

    [SerializeField] IAPHelper[] inApps;

    public static UIPopup CreateShopDialog()
    {
        var popup = UIPopup.GetPopup("ChipStore");
        popup.GetComponent<ChipStoreController>().Initialize();


        return popup;
    }

    public void Initialize()
    {
        var iapManager = HomeController.Instance.GetComponent<InAppPurchaseManager>();

        ProductNameToChipSlot(ShopProductNames.ExtraCredits200M, inApps[0]);
        ProductNameToChipSlot(ShopProductNames.ExtraCredits60M, inApps[1]);
        ProductNameToChipSlot(ShopProductNames.ExtraCredits20M, inApps[2]);

        ProductNameToChipSlot(ShopProductNames.ExtraCredits6M, inApps[3]);
        ProductNameToChipSlot(ShopProductNames.ExtraCredits2M, inApps[4]);
        ProductNameToChipSlot(ShopProductNames.ExtraCredits200K, inApps[5]);


        // for (var i = 0; i < iapManager.products.Count && i < inApps.Length; i++)
        // {
        //     Debug.Log("Adding Product: " + iapManager.products[i].localizedTitle);
        //     inApps[i].promoText.text = iapManager.products[i].localizedTitle;
        //     inApps[i].priceText.text = iapManager.products[i].localizedPriceString;
        // }
        // foreach (var product in iapManager.products)
        // {

        // }
        // foreach(IAPHelper iap in inApps)
        // {
        //     if(iap.hasPromo)
        //     {
        //         iap.promoBanner.SetActive(true);
        //         iap.promoText.text = "Available";
        //     }
        //     else
        //     {
        //         iap.promoBanner.SetActive(false);
        //     }
        // }
    }

    private void ProductNameToChipSlot(ShopProductNames productName, IAPHelper slot)
    {
        slot.promoText.text = IAPManager.Instance.GetLocalizedTitle(productName);
        slot.priceText.text = IAPManager.Instance.GetLocalizedPriceString(productName);
        slot.promoBanner.SetActive(true);
    }
}
