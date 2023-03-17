using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChesserMonolog: MonoBehaviour
{
    public static ChesserMonolog Instance { set; get; }
    public TextMeshProUGUI tmpLeft;
    public TextMeshProUGUI tmpRight;

    public Replic[] startReplics;
    public Replic[] winReplics;
    public Replic[] loseReplics;

    public Animator animatorLeft;
    public Animator animatorRight;

    public ChessCameraControl chessCamera;
    public GameObject waitPanel;

    void Start()
    {
        Instance = this;
        tmpLeft.text = "";
        tmpRight.text = "";
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        foreach (Replic replic in startReplics)
        {
            StartCoroutine(ShowText(replic.text, replic.side, replic.text.Length / 15 + 2, replic.color));
            yield return new WaitForSeconds(replic.text.Length / 15 + 3);
        }

        chessCamera.SwapCameraPosition();
        waitPanel.SetActive(false);
    }

    private IEnumerator ShowText(string text, Side side, float duration, Color color)
    {
        if (side == Side.left)
        {
            tmpLeft.text = text;
            tmpLeft.color = color;
            animatorLeft.SetTrigger("Show");
            yield return new WaitForSeconds(duration);
            animatorLeft.SetTrigger("Hide");
        }
        else
        { 
            tmpRight.text = text;
            tmpRight.color = color;
            animatorRight.SetTrigger("Show"); 
            yield return new WaitForSeconds(duration);
            animatorRight.SetTrigger("Hide");
        }
    }

    public virtual IEnumerator BlackWinText()
    {
        waitPanel.SetActive(true);
        if (!chessCamera.isFar)
            chessCamera.SwapCameraPosition();
        foreach (Replic replic in loseReplics)
        {
            StartCoroutine(ShowText(replic.text, replic.side, replic.text.Length / 15 + 2, replic.color));
            yield return new WaitForSeconds(replic.text.Length / 15 + 3);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public virtual IEnumerator WhiteWinText()
    {
        waitPanel.SetActive(true);
        if (!chessCamera.isFar)
            chessCamera.SwapCameraPosition();
        foreach (Replic replic in winReplics)
        {
            StartCoroutine(ShowText(replic.text, replic.side, replic.text.Length / 15 + 2, replic.color));
            yield return new WaitForSeconds(replic.text.Length / 15 + 3);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
