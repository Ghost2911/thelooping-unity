using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class HomeArchitectorShop : HomeShop
{
    public List<GameObject> homeUpgrades;
    public Button[] buttons;

    private void Start()
    {
        foreach (GameObject build in homeUpgrades)
        {
            if (PlayerPrefs.GetInt(build.name) != 0)
                homeUpgrades.Remove(build);
            else
                build.SetActive(false);
        }

        for (int num = 0; num < buttons.Length; num++)
            StartCoroutine(UpdateButton(num, false));
    }
 
    public void Buy(int cardNum,HouseUpgradeData upgrade, GameObject build)
    {
        if (Inventory.instance.ChangeCollectableItem(upgrade.costItem.type, -upgrade.costPrice))
        {
            PlayerPrefs.SetInt(build.name, 1);
            PlayerPrefs.Save();
            StartCoroutine(UpdateButton(cardNum, true));
        }
    }

    public void UpdateButtonData(int num)
    {
        if (homeUpgrades.Count != 0)
        {
            HouseUpgradeData upgrade = homeUpgrades[0].GetComponent<HouseUpgrade>().upgradeData;
            buttons[num].GetComponent<ShopCard>().SetCardInfo(upgrade.description, upgrade.icon,
                upgrade.costPrice, upgrade.costItem.icon);
            buttons[num].onClick.RemoveAllListeners();
            buttons[num].onClick.AddListener(() => Buy(num, upgrade, homeUpgrades[0]));
            homeUpgrades.Remove(homeUpgrades[0]);
        }
        else
            buttons[num].gameObject.SetActive(false);
    }

    IEnumerator UpdateButton(int num, bool isAnimated)
    {
        if (isAnimated)
        {
            buttons[num].interactable = false;
            yield return new WaitForSeconds(0.2f);
            buttons[num].interactable = true;
        }
        UpdateButtonData(num);
    }
}
