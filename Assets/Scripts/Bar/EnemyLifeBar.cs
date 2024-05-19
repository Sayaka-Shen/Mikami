using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLifeBar : MonoBehaviour
{
    [Header("Bar Component")] 
    [SerializeField] private Image imageBarComponent;

    [Header("Enemy Info")] 
    [SerializeField] private EnemyLife enemyLife;
    [SerializeField] private Transform enemyTransform;

    private void Start()
    {
        enemyLife.OnDamageTaken += EnemyLife_OnDamageTaken;

        imageBarComponent.fillAmount = 1;
    }

    private void Update()
    {
        KeepFlip();
    }

    private void EnemyLife_OnDamageTaken(object sender, EnemyLife.OnDamageTakenEventArgs e)
    {
        float healthRatio = (float)e.currentHealthEvent / enemyLife.MaxHealth;
        
        imageBarComponent.fillAmount = healthRatio;
    }
    
    private void KeepFlip()
    {
        Vector3 localScale = enemyTransform.localScale;
        
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
