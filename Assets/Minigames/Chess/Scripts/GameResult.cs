using System.Collections;
using UnityEngine;

public class GameResult : MonoBehaviour
{
	public static GameResult instance { get; set; }
	private ChessCameraControl chessCamera;

    void Start()
    {
		instance = this;
		chessCamera = Camera.main.GetComponent<ChessCameraControl>();
    }

	public void WinnerDialogAndLoad()
	{
		if (!chessCamera.isFar)
			chessCamera.SwapCameraPosition();
		//BoardManager.Instance.DeleteSerializeBoard();
		if (BoardManager.Instance.isWhiteTurn)
			StartCoroutine(ChesserMonolog.Instance.BlackWinText());
		else
			StartCoroutine(ChesserMonolog.Instance.WhiteWinText());
	}

	/*public void SaveAndLoad()
	{
		if (!chessCamera.isFar)
			chessCamera.SwapCameraPosition();
		BoardManager.Instance.SerializeBoard();
		StartCoroutine(ChesserMonolog.Instance.NeutralText());
	}*/
}
