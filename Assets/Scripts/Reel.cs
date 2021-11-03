using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
public class Reel : MonoBehaviour {
 
    //This Variable Will Be Changed By The "Slots" Class To Control When The Reel Spins 
    public bool spin;
    public int spinResult;

    public float offset = -368;
    public float speed = 1500;

    private float step;

    // Use this for initialization
    void Start()
    {
        spin = false;
        step =  transform.GetComponentInChildren<RectTransform>().rect.height;
    }
 
    // Update is called once per frame
    void Update()
    {
        if (spin)
        {
            foreach (Transform image in transform)//This Targets All Children Objects Of The Main Parent Object
            {
                //Direction And Speed Of Movement
                image.transform.Translate(-image.up * Time.smoothDeltaTime * speed, Space.World);
 
                //Once The Image Moves Below A Certain Point, Reset Its Position To The Top
                if (image.transform.localPosition.y <= -step) { image.transform.localPosition = new Vector3(image.transform.localPosition.x, step, 0); }
            }
        }
    }
 
    //Once The Reel Finishes Spinning The Images Will Be Placed In A Random Position
    public int RandomPosition(List<int> alreadyUsed)
    {
        List<int> parts = new List<int>();

        //Auto-generetion with offset
        int n = 0;
        foreach (Transform image in transform)
        {
             parts.Add(n++);
        }

        //transform.GetChild()
        //установить в позицию 0-0 которого нет в alreadyUsed
        //продолжить перебирать остальные

        int[] notUsed = parts.Except(alreadyUsed).ToArray();
        int resNum = Random.Range(0, notUsed.Length);

        spinResult = transform.GetChild(notUsed[resNum]).GetSiblingIndex();

        transform.GetChild(notUsed[resNum]).localPosition = new Vector3(transform.GetChild(notUsed[resNum]).localPosition.x, parts[1] * step + offset + transform.parent.GetComponent<RectTransform>().transform.localPosition.y, 0);
        parts.Remove(parts[1]);

        //остальное распределение

        for (int i = 0; i < transform.childCount; i++)
        {
             int rand = Random.Range(0, parts.Count);

            //где 0 - тот результатный текущий
            if (transform.GetChild(i).GetSiblingIndex()==spinResult)
            {
                continue;
            }

            transform.GetChild(i).localPosition = new Vector3(transform.GetChild(i).localPosition.x, parts[rand]*step+offset + transform.parent.GetComponent<RectTransform>().transform.localPosition.y, 0);
            parts.RemoveAt(rand);
        }

    
        Debug.Log(spinResult);
        return spinResult;
    }
}