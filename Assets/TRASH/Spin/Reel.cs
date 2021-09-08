using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
public class Reel : MonoBehaviour {
 
    //This Variable Will Be Changed By The "Slots" Class To Control When The Reel Spins 
    public bool spin;
    public int spinResult;

    public int step = 150;
    public int offset = -150;
    //Speed That Reel Will Spin
    int speed;
  
    // Use this for initialization
    void Start()
    {
        spin = false;
        speed = 1500;
    }
 
    // Update is called once per frame
    void Update()
    {
        if (spin)
        {
            foreach (Transform image in transform)//This Targets All Children Objects Of The Main Parent Object
            {
                //Direction And Speed Of Movement
                image.transform.Translate(Vector3.down * Time.smoothDeltaTime * speed, Space.World);
 
                //Once The Image Moves Below A Certain Point, Reset Its Position To The Top
                if (image.transform.position.y <= 0) { image.transform.position = new Vector3(image.transform.position.x, image.transform.position.y + 600, image.transform.position.z); }
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

        transform.GetChild(notUsed[resNum]).position = new Vector3(transform.GetChild(notUsed[resNum]).position.x, parts[1] * step + offset + transform.parent.GetComponent<RectTransform>().transform.position.y,
                transform.GetChild(notUsed[resNum]).position.z);
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

            transform.GetChild(i).position = new Vector3(transform.GetChild(i).position.x, parts[rand]*step+offset + transform.parent.GetComponent<RectTransform>().transform.position.y, 
                transform.GetChild(i).position.z);
            parts.RemoveAt(rand);
        }

    
        Debug.Log(spinResult);
        return spinResult;
    }
}