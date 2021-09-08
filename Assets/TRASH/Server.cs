using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    public GameObject[] spawns;
    public FloatingJoystick joystick;
    public Button btnAttack;
    public GameObject revive;

    void Awake()
    {
        CreateCharacter();
    }

    public void CreateCharacter()
    {
        GameObject character = Instantiate(Resources.Load("character") as GameObject,spawns[Random.Range(0,spawns.Length)].transform.position, Quaternion.Euler(0, 0, 0));
        Camera.main.GetComponent<CameraFollow>().target = character.transform;
        character.GetComponent<PlayerInput>().joystick = joystick;
        character.GetComponent<PlayerInput>().btnAttack = btnAttack;
        character.GetComponent<PlayerInput>().revive = revive;
        revive.SetActive(false);
    }

    public void Leave()
    {
        SceneManager.LoadScene(0);
    }

}
