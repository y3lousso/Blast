using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour {

    public GameObject comboPanel;
    public Text comboText;

	// Update is called once per frame
	void Update ()
    {
        if(comboPanel.active && ScoreManager.Instance.combo != 0)
        {
            comboText.text = ScoreManager.Instance.combo.ToString();
        }
        else if (ScoreManager.Instance.combo != 0)
        {
            comboPanel.SetActive(true);
            comboText.text = ScoreManager.Instance.combo.ToString();
        }
        else
        {
            comboPanel.SetActive(false);
        }
	}
}
