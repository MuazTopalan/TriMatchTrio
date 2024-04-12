using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public int levelNumber; // Bu seviyenin numarası

    private void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); // Kilidi açılan en yüksek seviye

        // Bu butonun numarası, kilidi açılan en yüksek seviyeden büyükse, bu seviye henüz kilidi açılmamış demektir.
        if (levelNumber > unlockedLevel)
        {
            GetComponent<Button>().interactable = false; // Butonu tıklanamaz hale getir
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level" + levelNumber); // Seviye yükle
    }
}
