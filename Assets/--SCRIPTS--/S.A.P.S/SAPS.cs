using System;
using System.Collections.Generic;
using UnityEngine;

public class SAPS : MonoBehaviour
{
    public int chainLength = 5;
    private string[] chain;
    private int cursor = 0;
    public List<int> hashcodes;

    public void Start()
    {
        chain = new string[chainLength];
        hashcodes = new List<int>();
    }

    public void AddAction(string actionName)
    {
        chain[cursor++] = actionName;
        if (cursor == chainLength)
        {
            int hash = String.Join("", chain).GetHashCode();
            if (!hashcodes.Contains(hash))
                hashcodes.Add(hash);
            Debug.Log($"Action Hashcode:{hash}");
            cursor = 0;
        }
    }
}

public class ActionChain
{
    DateTime date;
    string[] chain;

    int totalCombination;    
}

public class ExpectedAction
{
    int count; 
}

enum PlayerActionType
{
    Attack,
    Move,
}

