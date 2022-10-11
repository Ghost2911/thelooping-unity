using UnityEngine;

public class ChessChangeTeam : MonoBehaviour
{
	public Sprite blackTeamSprite;
	public void SwapSprite ()
	{
		GetComponent<SpriteRenderer>().sprite = blackTeamSprite;
	}
}
