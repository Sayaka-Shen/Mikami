using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeBar : MonoBehaviour
{
    [Header("Bar Component")] 
    [SerializeField] private Image imageBarComponent;
    
    [Header("Hero Information")]
    [SerializeField] private HeroLife heroLife;
    [SerializeField] private Transform heroTransform;

    private void Start()
    {
        heroLife.OnHeroLifeChanges += HeroLife_OnHeroTakeDamage;
    }

    private void Update()
    {
        KeepFlip();
    }

    private void HeroLife_OnHeroTakeDamage(object sender, HeroLife.OnHeroLifeChangesEventAgrs e)
    {
        float healthRatio = (float)e.currentHealthHeroEvent / heroLife.MaxHealth;
        
        imageBarComponent.fillAmount = healthRatio;
    }
    
    private void KeepFlip()
    {
        Vector3 localScale = heroTransform.localScale;
        
        if (localScale.x == -1)
        {
            imageBarComponent.fillOrigin = 1;
        }

        if (localScale.x == 1)
        {
            imageBarComponent.fillOrigin = 0;
        }
    }
}
