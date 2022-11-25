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
    public AudioClip defaultSoundtrack;
    private AudioSource audioSource;
    private CameraFollow cameraFollow;

    private void Awake()
    {
        instance = this;
        FindAllPlayableCharacters();
        audioSource = GetComponent<AudioSource>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void Start()
    {
        Invoke("CreateCharacter", 1f);
    }

    public void ChangeBackgroundSoundtrack(AudioClip clip)
    {
        if (clip == null)
            clip = defaultSoundtrack;

        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void CreateCharacter()
    {
        int characterNum = (isTutorial)?0:Random.Range(0, characters.Count);

        PlayerInput player = characters[characterNum].GetComponent<PlayerInput>();
        SetCameraTraget(player.transform);
        player.joystick = joystick;
        btnAttack.onClick.AddListener(player.AttackButtonClick);
        btnFlip.onClick.AddListener(player.FlipButtonClick);
        btnUse.onClick.AddListener(player.UseButtonClick);
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

    public void SetCameraTraget(Transform target)
    {
        cameraFollow.target = target;
    }

    public void Leave()
    {
        SceneManager.LoadScene(0);
    }

}
