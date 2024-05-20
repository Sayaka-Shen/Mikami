using System;
using UnityEngine;
using UnityEngine.UI;

public class BugBar : MonoBehaviour
{
    [Header("Bar Component")] 
    [SerializeField] private Image imageBarComponent;
    private Animator animator;

    [Header("Entity Component")] 
    [SerializeField] private HeroMovement heroMovement;
    [SerializeField] private HeroAttack heroAttack;
    [SerializeField] private Transform characterTransform;

    [Header("Teleporter")] 
    [SerializeField] private Transform teleporterArene;

    public Transform TeleporteArene
    {
        get { return teleporterArene; }
    }

    [Header("Enemy Spawner")] 
    [SerializeField] private SpawnEnemy enemySpawner;
    
    private void Start()
    {
        heroMovement.OnBugging += HeroMovement_OnBugging;
        animator = heroMovement.GetComponent<Animator>();
        imageBarComponent.fillAmount = 0f;
    }

    private void Update()
    {
        KeepFlip();
    }

    private void HeroMovement_OnBugging(object sender, HeroMovement.OnBuggingEventArgs e)
    {
        if (!heroAttack.AttackMode)
        {
            imageBarComponent.fillAmount = e.buggingNumberEvent;
        
            if(Mathf.Approximately(imageBarComponent.fillAmount, 1f))
            {
                enemySpawner.UpdateLastTeleportPosition(characterTransform.position);
                animator.SetTrigger("IsChainging");
                Vector3 teleportOffset = new Vector3(-0.2f, 0.2f, 0);
                characterTransform.position = teleporterArene.position + teleportOffset;

                heroAttack.AttackMode = true;
            }
        }
    }

    private void KeepFlip()
    {
        Vector3 localScale = characterTransform.localScale;
        
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
