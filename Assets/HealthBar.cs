using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int Health = 0;

    public Image[] HealthIcons = new Image[4];
    
    public Color fullColor = Color.white;
    public Color emptyColor = Color.black;
    
    public Player target;

    void UpdateHealthBar()
    {
        for (int i = 0; i < HealthIcons.Length; i++)
        {
            if (i < Health)
            {
                HealthIcons[i].color = fullColor;
            }
            else
            {
                HealthIcons[i].color = emptyColor;
            }
        }
    }

    void Update()
    {
        if (target != null)
        {
            Health = target.HealthPoints;
        }
        
        UpdateHealthBar();
    }
}
