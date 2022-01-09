using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour {
 
    public int spinResult;

    private Animator _animator;
    private Image _img;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _img = GetComponent<Image>();
    }
 
    public void StartSpin()
    {
        _animator.Play("Roll");
        _img.sprite = GetSprite("Bosses_Contracts",spinResult.ToString());
    }

    public void StopSpin()
    {
        _animator.Play("EndRoll");
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