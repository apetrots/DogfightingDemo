using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTracker : MonoBehaviour
{
    public TextMeshProUGUI numberText;

    void Update()
    {
        var ships = FindObjectsOfType<EnemyShip>();

        numberText.text = "" + ships.Length;
    
        if (ships.Length == 0)
        {   
            var gameManager = FindObjectOfType<GameManager>();
            gameManager.GameWin();
        }
    }
}
