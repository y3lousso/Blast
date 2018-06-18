using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {

    [Header("Panels")]
    public GameObject selectModePanel;
    public GameObject selectMusicPanel;
    public GameObject resultsPanel;

    [Header("Results Panel")]
    public Text scoreText;

    // Use this for initialization
    void Start () {
        HideAllPanel();

        if (GameManager.Instance.results.MaxHit == 0)
        {
            selectModePanel.SetActive(true);
        }
        else
        {
            ShowResults(GameManager.Instance.results);
            GameManager.Instance.results = null;
        }

    }
	
    public void Mode_OnSoloClick()
    {
        HideAllPanel();
        selectMusicPanel.SetActive(true);
    }

    public void Mode_OnQuitClick()
    {
        Application.Quit();
    }

    public void Music_OnMusicSelected(AudioData audioData)
    {
        SceneDataManager.Instance.GoToPlayScene(audioData);
    }

    public void Music_OnReturnSelected()
    {
        HideAllPanel();
        selectModePanel.SetActive(true);
    }

    public void Result_OnContinueClick()
    {
        HideAllPanel();
        selectMusicPanel.SetActive(true);
    }

    private void HideAllPanel()
    {
        selectModePanel.SetActive(false);
        selectMusicPanel.SetActive(false);
        resultsPanel.SetActive(false);
    }

    private void ShowResults(Results results)
    {
        scoreText.text = results.CorrectHit + " / " + results.MaxHit;
        resultsPanel.SetActive(true);
    }
}
