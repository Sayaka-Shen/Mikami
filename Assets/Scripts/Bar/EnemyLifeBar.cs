using UnityEngine;
using UnityEngine.UI;

public class EnemyLifeBar : MonoBehaviour
{
    [Header("Bar Component")] 
    [SerializeField] private Image imageBarComponent;

    [Header("Enemy Info")] 
    [SerializeField] private EnemyLife enemyLife;

    private void Start()
    {
        enemyLife.OnDamageTaken += EnemyLife_OnDamageTaken;

        imageBarComponent.fillAmount = 1;
    }

    private void EnemyLife_OnDamageTaken(object sender, EnemyLife.OnDamageTakenEventArgs e)
    {
        float healthRatio = (float)enemyLife.CurrentHealth / enemyLife.MaxHealth;
        
        imageBarComponent.fillAmount = healthRatio;
    }
}
