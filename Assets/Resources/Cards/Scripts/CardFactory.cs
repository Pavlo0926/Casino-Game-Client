using UnityEngine;

public static class CardFactory
{
    private readonly static string CardPrefabResourcesPath = "Cards/Prefabs/Card";

    public static GameObject CreateCard(string card, bool isFaceUp, Transform parent)
    {
        var c = GameObject.Instantiate(Resources.Load<GameObject>(CardPrefabResourcesPath));

        c.transform.SetParent(parent, false);
        c.transform.localPosition = Vector3.zero;
        c.transform.localRotation = Quaternion.identity;
        c.transform.localScale = Vector3.one;

        c.GetComponent<Card>().SetCard(card);
        c.GetComponent<Card>().SetIsFaceUp(isFaceUp);

        return c;
    }

    public static GameObject CreateCard(string card, bool isFaceUp, Vector3 position, Transform parent)
    {
        var c = CreateCard(card, isFaceUp, parent);

        c.transform.SetParent(parent, false);
        c.transform.position = new Vector3(position.x, position.y, parent.position.z);
        c.transform.rotation = Quaternion.identity;

        return c;

    }
}