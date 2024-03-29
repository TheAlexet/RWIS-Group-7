using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject rowPrefab;
    public int itemsPerRow = 3;

    [SerializeField] List<GameObject> itemsGameObjects;
    [SerializeField] GameObject buttonGameObjectEmpty;
    [SerializeField] Transform parent;
    GameObject last_row;
    List<GameObject> itemsInstantiated;
    private DatabaseAccess db;
    [SerializeField] private AudioSource buttonSound;

    private void Awake() { db = new DatabaseAccess(); }

    void Start()
    {
        for (int i = 0; i < itemsGameObjects.Count; i++)
        {
            if (i % itemsPerRow == 0)
            {
                last_row = GameObject.Instantiate(rowPrefab, parent);
                last_row.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    last_row.GetComponent<RectTransform>().sizeDelta.x,
                    last_row.GetComponent<RectTransform>().sizeDelta.y / (float)(itemsPerRow - 1)
                );
                last_row.GetComponent<HorizontalLayoutGroup>().spacing = last_row.GetComponent<RectTransform>().sizeDelta.x / itemsPerRow / 10f;
            }
            GameObject item = itemsGameObjects[i];
            GameObject itemButton = GameObject.Instantiate(item, last_row.transform);
            itemButton.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = item.GetComponent<Item>().Data.Name;
            itemButton.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = item.GetComponent<Item>().Data.Acorns.ToString();
        }

        int emptyItems = itemsPerRow - itemsGameObjects.Count % itemsPerRow;
        for (int i = 0; i < emptyItems; i++)
        {
            GameObject itemButton = GameObject.Instantiate(buttonGameObjectEmpty, last_row.transform);
        }
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                if (i * 3f + j + 1f <= itemsGameObjects.Count)
                {
                    Transform grandChild = child.GetChild(j);
                    if(Database.getPurchasedItem(grandChild.GetComponent<Item>().Data.Name) == true)
                    {
                        grandChild.GetComponent<Image>().color = new Color32(195,195,195,170);
                    }
                    else
                    {
                        grandChild.GetComponent<Button>().onClick.AddListener(delegate { BuyItem(grandChild.GetComponent<Item>()); });
                    }
                }
            }
        }
    }

    void BuyItem(Item wantedItem)
    {
        if(wantedItem.Data.Name == "Ticket" && Database.getAcorns() >= wantedItem.Data.Acorns)
        {
            buttonSound.Play();
            Database.IncrLotteryTickets(1);
            Database.IncrAcorns(-wantedItem.Data.Acorns);
        }
        else if(Database.getPurchasedItem(wantedItem.Data.Name) == false && Database.getAcorns() >= wantedItem.Data.Acorns)
        {
            buttonSound.Play();
            Database.setPurchasedItem(wantedItem.Data.Name);
            Database.IncrAcorns(-wantedItem.Data.Acorns);
            wantedItem.transform.GetComponent<Image>().color = new Color32(195,195,195,170);
        }
    }
}
