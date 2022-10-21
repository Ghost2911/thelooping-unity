using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChesserMonolog: MonoBehaviour
{
    public static ChesserMonolog Instance { set; get; }
    public TextMeshProUGUI tmpLeft;
    public TextMeshProUGUI tmpRight;

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
        yield return new WaitForSeconds(2f);
        StartCoroutine(ShowLeftText("It looks like it's just getting started", 4f, Color.white));
        yield return new WaitForSeconds(2f);
        StartCoroutine(ShowRightText("Doesn't it?", 3f, Color.yellow));
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(ShowRightText("You can make the first move", 10f, Color.yellow));
        yield return new WaitForSeconds(5f);
        chessCamera.SwapCameraPosition();
        waitPanel.SetActive(false);
    }

    private IEnumerator ShowLeftText(string text, float duration, Color color)
    {
        tmpLeft.text = text;
        tmpLeft.color = color;
        animatorLeft.SetTrigger("Show");
        yield return new WaitForSeconds(duration);
        animatorLeft.SetTrigger("Hide");
    }

    private IEnumerator ShowRightText(string text, float duration, Color color)
    {
        tmpRight.text = text;
        tmpRight.color = color;
        animatorRight.SetTrigger("Show");
        yield return new WaitForSeconds(duration);
        animatorRight.SetTrigger("Hide");
    }

    public virtual IEnumerator BlackWinText()
    {
        waitPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        StartCoroutine(ShowLeftText("YOU LOSE", 4f, Color.white));
        yield return new WaitForSeconds(2f);
        StartCoroutine(ShowRightText("YOU LOSE", 3f, Color.yellow));
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(ShowRightText("YOU LOSE", 10f, Color.yellow));
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public virtual IEnumerator WhiteWinText()
    {
        waitPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        StartCoroutine(ShowLeftText("YOU WIN", 4f, Color.white));
        yield return new WaitForSeconds(2f);
        StartCoroutine(ShowRightText("YOU WIN", 3f, Color.yellow));
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(ShowRightText("YOU WIN", 10f, Color.yellow));
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public virtual IEnumerator NeutralText()
    {
        waitPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        StartCoroutine(ShowLeftText("Neutral", 4f, Color.white));
        yield return new WaitForSeconds(2f);
        StartCoroutine(ShowRightText("Neutral", 3f, Color.yellow));
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(ShowRightText("Neutral", 10f, Color.yellow));
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
