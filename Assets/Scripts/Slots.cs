using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class Slots : MonoBehaviour {
 
    public Reel[] reel;
    bool startSpin;
    List<int> results = new List<int>();
 
    // Use this for initialization
    void Start ()
    {
        startSpin = false;
    }
     
    // Update is called once per frame
    void Update ()
    {
        if (!startSpin)//Prevents Interference If The Reels Are Still Spinning
        {
            if (Input.GetKeyDown(KeyCode.K))//The Input That Starts The Slot Machine
            {
                startSpin = true;
                results.Clear();
                StartCoroutine(Spinning());
            }
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
        //Allows The Machine To Be Started Again
        startSpin = false;
    }
}