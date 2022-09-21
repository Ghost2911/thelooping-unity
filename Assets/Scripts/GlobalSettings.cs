using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class GlobalSettings : MonoBehaviour
{
    public FloatingJoystick joystick;
    public Button btnAttack;
    public Button btnFlip;
    public Button btnUse;
    public HealthPresentor healthPresentor;
    public List<GameObject> characters;
    public static GlobalSettings instance;
    public bool isTutorial = false;

    private void Awake()
    {
        instance = this;
        FindAllPlayableCharacters();
    }

    private void Start()
    {
        Invoke("CreateCharacter", 1f);
    }

    public void CreateCharacter()
    {
        int characterNum = (isTutorial)?0:Random.Range(0, characters.Count);

        PlayerInput player = characters[characterNum].GetComponent<PlayerInput>();
        Camera.main.GetComponent<CameraFollow>().target = player.transform;
        player.joystick = joystick;
        player.btnAttack = btnAttack;
        player.btnFlip = btnFlip;
        player.btnUse = btnUse;
        player.enabled = true;
        player.stats.HealthChangeEvent.AddListener(healthPresentor.ChangeValue);
        healthPresentor.ChangeValue(player.stats.Health);

        characters.RemoveAt(characterNum);
    }

    public void FindAllPlayableCharacters()
    {
        if (!isTutorial)
            characters.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        foreach (GameObject playableCharacter in characters)
            playableCharacter.GetComponent<PlayerInput>().enabled = false;
    }

    public void Leave()
    {
        SceneManager.LoadScene(0);
    }

}
