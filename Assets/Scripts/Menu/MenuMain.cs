using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Listens for the OnClick events for the main menu buttons
/// </summary>
public class MenuMain : MonoBehaviour
{
    Image fadeOutImage;
    bool started = false;
    float sec;

    private void Start()
    {
        fadeOutImage = GameObject.FindGameObjectWithTag("FadeOut").GetComponent<Image>();
        fadeOutImage.enabled = false;
    }

    private void Update()
    {
        if (started)
        {
            sec += Time.deltaTime * 0.5f;
            Color colorLerp = Color.Lerp(fadeOutImage.color, Color.black, sec * 0.005f);
            fadeOutImage.color = colorLerp;
        }
        if (sec >= 1) SceneManager.LoadScene("Pool");
    }

    /// <summary>
    /// Handles the on click event from the play button
    /// </summary>
    public void HandlePlayButtonOnClickEvent()
    {
        fadeOutImage.enabled = true;
        started = true;
    }

    /// <summary>
    /// Handles the on click event from the quit button
    /// </summary>
    public void HandleQuitButtonOnClickEvent() { Application.Quit(); }
}
