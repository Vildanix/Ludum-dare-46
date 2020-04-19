using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusScreenHandler : MonoBehaviour
{
    public GameObject screen;
    public TMPro.TextMeshProUGUI textUI;

    [SerializeField]
    private string eventName = "";

    [SerializeField]
    private bool useForError = false;

    private void Awake()
    {
        if (eventName.Length > 0)
            EventManager.RegisterListener(eventName, ShowScreen);

        EventManager.RegisterListener("Restart", HideScreen);

        if (useForError)
            EventManager.RegisterListenerText("Error", ShowScreenWithCustomText);
    }

    public void ShowScreen()
    {
        screen.SetActive(true);
    }

    public void ShowScreenWithCustomText(string text)
    {
        textUI.SetText(text);
        ShowScreen();
    }

    public void HideScreen()
    {
        screen.SetActive(false);
    }

    public void RestartGame()
    {
        EventManager.DispatchEvent("Restart");
    }
}
