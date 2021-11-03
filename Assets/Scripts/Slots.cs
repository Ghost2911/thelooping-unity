using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
public class Slots : MonoBehaviour {
 
    public Reel[] reel;
    bool startSpin;
    List<int> results = new List<int>();
    public UnityEvent SpinEndEvent;
    public float timeShowResult = 2f;

    void Start ()
    {
        startSpin = false;
    }

    void OnMouseDown()
    {
        if (!startSpin)
        {
            startSpin = true;
            results.Clear();
            StartCoroutine(Spinning());
        }
    }

        IEnumerator Spinning()
    {
        foreach (Reel spinner in reel)
        {
            //Tells Each Reel To Start Spnning
            spinner.spin = true;
        }
 
        for(int i = 0; i < reel.Length; i++)
        {
            //Allow The Reels To Spin For A Random Amout Of Time Then Stop Them
            yield return new WaitForSeconds(Random.Range(1, 3));
            reel[i].spin = false;

            int rollResult = reel[i].RandomPosition(results);
            results.Add(rollResult);
        }
        yield return new WaitForSeconds(timeShowResult);
        SpinEndEvent.Invoke();
        //Allows The Machine To Be Started Again
        startSpin = false;
    }
}