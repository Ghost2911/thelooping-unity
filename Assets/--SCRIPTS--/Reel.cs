using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour 
{
    public int spinResult;

    [HideInInspector]
    public Animator animator;
    private Image _img;

    void Start()
    {
        animator = GetComponent<Animator>();
        _img = GetComponent<Image>();
    }
 
    public void StartSpin()
    {
        animator.Play("Roll");
        _img.sprite = GetSprite("Bosses_Contracts",spinResult.ToString());
    }

    public void StopSpin()
    {
        animator.Play("EndRoll");
    }

    Sprite GetSprite(string imageName, string spriteName)
    {
        Sprite[] all = Resources.LoadAll<Sprite>(imageName);

        foreach (var s in all)
            if (s.name == spriteName)
                return s;
        return null;
    }
}