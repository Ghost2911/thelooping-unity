using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    public Text LogText;
    public GameObject buttons;
    void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(1, 9999);
        PhotonNetwork.GameVersion = "1";
        Log(PhotonNetwork.NickName+" - сгенерированный никнейм");
        Log("...ОЖИДАНИЕ СОЕДИЕНИЯ...");
        PhotonNetwork.ConnectUsingSettings();
        buttons.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        Log("Готов к подключению! Нажмите кнопку СОЗДАТЬ или ПРИСОЕДИНИТЬСЯ!");
        buttons.SetActive(true);
    }

    public override void OnCreatedRoom()
    {
        Log("Создана комната");
    }

    public override void OnJoinedRoom()
    {
        Log("Присоединение к комнате");
        PhotonNetwork.LoadLevel(1);
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void Log(string message)
    {
        LogText.text += "\n" + message;
    }
}
