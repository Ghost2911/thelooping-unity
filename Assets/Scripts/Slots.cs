using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
public class Slots : MonoBehaviour {
 
    public Reel[] reels;
    bool startSpin;
    public UnityEvent SpinEndEvent;
    public float timeShowResult = 2f;

    void Start ()
    {
        startSpin = false;
    }

    public void SetSpinResults(List<int> results)
    {
        for (int i = 0; i < reels.Length; i++)
            reels[i].spinResult = results[i];
    }

    void OnMouseDown()
    {
        if (!startSpin)
        {
            startSpin = true;
            StartCoroutine(Spinning());
        }
    }

    IEnumerator Spinning()
    {
        foreach (Reel reel in reels)
        {
            //Tells Each Reel To Start Spnning
            reel.StartSpin();
        }
 
        for(int i = 0; i < reels.Length; i++)
        {
            //Allow The Reels To Spin For A Random Amout Of Time Then Stop Them
            yield return new WaitForSeconds(Random.Range(1, 3));
            reels[i].StopSpin();
        }
        yield return new WaitForSeconds(timeShowResult);
        SpinEndEvent.Invoke();
        //Allows The Machine To Be Started Again
        startSpin = false;
    }
}