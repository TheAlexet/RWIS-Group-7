using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSwitch : MonoBehaviour
{
    [SerializeField] GameObject menusHolder;

    [SerializeField] GameObject levelText;
    [SerializeField] GameObject buttonsHolder;
    public float rescaleSelectedButton = 1.1f;
    public float transitionStep = 0.1f;
    [SerializeField] private AudioSource buttonSound;

    [Header("Debug Values")]
    [SerializeField] RectTransform defaultButtonRect;
    [SerializeField] int prevMenu = 1;
    [SerializeField] int currentMenu = 1;
    [SerializeField] Vector2 defaultButtonScale;

    [SerializeField] private GameObject databaseAccess;

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("Database").Length == 0)
        {
            DontDestroyOnLoad(GameObject.Instantiate(databaseAccess));
        }
    }

    void Start()
    {
        defaultButtonRect = buttonsHolder.transform.GetChild(0).GetComponent<RectTransform>();
        defaultButtonScale = defaultButtonRect.rect.size;
    }

    void Update()
    {
        UpdateScale();
    }

    public void ChangeMenu(int newMenu)
    {
        menusHolder.transform.GetChild(currentMenu).gameObject.SetActive(false);
        menusHolder.transform.GetChild(newMenu).gameObject.SetActive(true);
        prevMenu = currentMenu;
        currentMenu = newMenu;
        buttonSound.Play();
    }

    public void UpdateScale()
    {
        Vector2 curButtonScale = buttonsHolder.transform.GetChild(currentMenu).GetComponent<RectTransform>().sizeDelta;
        Vector2 curScale = Vector2.Lerp(curButtonScale, rescaleSelectedButton * defaultButtonScale, transitionStep);
        buttonsHolder.transform.GetChild(currentMenu).GetComponent<RectTransform>().sizeDelta = curScale;

        if (prevMenu != currentMenu)
        {
            Vector2 prevButtonScale = buttonsHolder.transform.GetChild(prevMenu).GetComponent<RectTransform>().sizeDelta;
            Vector2 prevScale = Vector2.Lerp(prevButtonScale, defaultButtonScale, transitionStep);
            buttonsHolder.transform.GetChild(prevMenu).GetComponent<RectTransform>().sizeDelta = prevScale;
        }

    }
}
