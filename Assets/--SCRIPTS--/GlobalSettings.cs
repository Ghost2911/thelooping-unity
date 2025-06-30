using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using System;

public class GlobalSettings : MonoBehaviour
{
    public PlayerInput player;
    public ColorChanger playerColor;
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
    public ItemPool itemPool;
    public Item itemPrefab;
    public PostProcessVolume postProcessVolume;
    public EntityStats playerEntity = null;

    private void Awake()
    {
        instance = this;
        FindAllPlayableCharacters();
        audioSource = GetComponent<AudioSource>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        itemPool = new ItemPool();
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

    public void SaveBossDeath(GameObject boss)
    {
        PlayerPrefs.SetInt(boss.name, 1);
        PlayerPrefs.Save();
    }

    public void CreateCharacter()
    {
        int characterNum = (isTutorial) ? 0 : UnityEngine.Random.Range(0, characters.Count);

        player = characters[characterNum].GetComponent<PlayerInput>();
        playerEntity = characters[characterNum].GetComponent<EntityStats>();
        playerColor = characters[characterNum].GetComponent<ColorChanger>();
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

    public EntityStats GetCurrentPlayer()
    {
        return playerEntity;
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

    public Item CreateItem(SlotType slotType, Vector3 position)
    {
        itemPrefab.stats = itemPool.GetItemFromCategory(slotType);
        itemPrefab.inMarket = false;
        return Instantiate(itemPrefab, position, new Quaternion(0, 0, 0, 0)); 
    }

    public void Leave()
    {
        SceneManager.LoadScene(0);
    }

}

