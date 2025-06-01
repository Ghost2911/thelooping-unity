using System;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SibylDeckCollection
{
    public Dictionary<int, int> collectedCards = new Dictionary<int, int>();
    private const int MaxCardsPerType = 3;
    private const int MaxCardsType = 12;

    [System.Serializable]
    private class SerializedCardCollection
    {
        public List<int> cardIds = new List<int>();
        public List<int> cardCounts = new List<int>();
    }

    public void AddCard(int cardId)
    {
        if (collectedCards.ContainsKey(cardId))
        {
            collectedCards[cardId] = Mathf.Min(collectedCards[cardId] + 1, MaxCardsPerType);
        }
        else
            collectedCards[cardId] = 1;
    }

    public int GetCardCount(int cardId)
    {
        return collectedCards.ContainsKey(cardId) ? collectedCards[cardId] : 0;
    }
    
    public bool GetCardActive(int cardId)
    {
        return collectedCards.ContainsKey(cardId) ? collectedCards[cardId]==3 : false;
    }

    public string ToJson()
    {
        SerializedCardCollection serializable = new SerializedCardCollection();

        foreach (var pair in collectedCards)
        {
            serializable.cardIds.Add(pair.Key);
            serializable.cardCounts.Add(pair.Value);
        }

        return JsonUtility.ToJson(serializable);
    }

    public void FromJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return;
        
        try
        {
            SerializedCardCollection serializable = JsonUtility.FromJson<SerializedCardCollection>(json);
            collectedCards.Clear();
            
            if (serializable != null && 
                serializable.cardIds != null && 
                serializable.cardCounts != null && 
                serializable.cardIds.Count == serializable.cardCounts.Count)
            {
                for (int i = 0; i < serializable.cardIds.Count; i++)
                {
                    collectedCards[serializable.cardIds[i]] = serializable.cardCounts[i];
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse SibylDeckCollection JSON: {e.Message}");
        }
    }
}