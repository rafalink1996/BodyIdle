using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private GameObject Body;
    public Infection Infection;
    public float maxHealth;
    public float health;
    public Slider HealthSlider;

    public bool IsInsideBody;


    // Start is called before the first frame update
    void Start()
    {
        Body = GameObject.Find("Body");
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


        // check if enemy is inside body
        PolygonUtilities BodyPlygonUtilities = Body.GetComponent<PolygonUtilities>();
        Vector2[] BodyShape = BodyPlygonUtilities.polygonPoints;
        IsPointInsidePolygon isPointInsidePolygon = Body.GetComponent<IsPointInsidePolygon>();
        IsInsideBody = isPointInsidePolygon.IsPointInPolygon(transform.position, BodyShape);
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
