using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour {

    [Header("Canvas")]
    public GameObject canvas;

    [Header("Pointer")]
    [SerializeField] private VRTK.VRTK_UIPointer uiPointer;
    [SerializeField] private VRTK.VRTK_Pointer pointer;


	// Use this for initialization
	void Start ()
    {
        canvas.SetActive(false);
        uiPointer.enabled = false;
        pointer.enabled = false;
    }
    
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Pause()
    {
        GameManager.Instance.Pause();
        canvas.SetActive(true);
        uiPointer.enabled = true;
        pointer.enabled = true;
    }

    private void Unpause()
    {
        GameManager.Instance.Unpause();
        canvas.SetActive(false);
        uiPointer.enabled = false;
        pointer.enabled = false;
    }

    public void OnContinueClicked()
    {
        Unpause();
    }

    public void OnQuitClicked()
    {
        SceneDataManager.Instance.GoToMenuScene();
    }
}
