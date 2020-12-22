using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Infection Infection;
    public float maxHealth;
    public float health;
    public Slider HealthSlider;


    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer MySpriteRenderer = GetComponent<SpriteRenderer>();
        MySpriteRenderer.sprite = Infection.InfectionSprite;
        maxHealth = Infection.MaxHealh;
        health = maxHealth;
        HealthSlider.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthSlider.value = health;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int CellID, float DamageTaken)
    {
        switch (Infection.infectionType) // some Cells are more effective against some kind of infections
        {
            case 0: // bacteria
                if (CellID == 2) // Bacteria vs WhiteBlood;
                {
                    health -= DamageTaken;
                    break;
                } else
                {
                    break;
                }

            case 1: // virus
                break;
        

        }
    }
}
