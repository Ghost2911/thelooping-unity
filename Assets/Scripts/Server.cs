using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Server : MonoBehaviour
{
    public FloatingJoystick joystick;
    public Button btnAttack;
    public Button btnFlip;
    public GameObject revive;
    public HealthPresentor healthPresentor;
    public List<GameObject> characters;

    void Start()
    {
        FindAllPlayableCharacters();
    }

    public void CreateCharacter()
    {
        int characterNum = Random.Range(0, characters.Count);

        PlayerInput player = characters[characterNum].GetComponent<PlayerInput>();
        Camera.main.GetComponent<CameraFollow>().target = player.transform;
        player.enabled = true;
        player.joystick = joystick;
        player.btnAttack = btnAttack;
        player.btnFlip = btnFlip;
        player.revive = revive;
        player.stats.HealthChangeEvent.AddListener(healthPresentor.ChangeValue);
        healthPresentor.ChangeValue(player.stats.Health);
        revive.SetActive(false);

        characters.RemoveAt(characterNum);
    }

    public void FindAllPlayableCharacters()
    {
        characters.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        foreach (GameObject playableCharacter in characters)
            playableCharacter.GetComponent<PlayerInput>().enabled = false;
    }

    public void Leave()
    {
        SceneManager.LoadScene(0);
    }

}
