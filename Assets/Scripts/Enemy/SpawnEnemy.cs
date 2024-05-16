    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine.UI;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class SpawnEnemy : MonoBehaviour
    {
        [Header("Enemy Information")] 
        [SerializeField] private Transform enemyPosition;
        
        // Prefab d'ennemi
        [SerializeField] private GameObject enemyPrefabDistance;
        [SerializeField] private GameObject enemyPrefabMeleeRapide;
        [SerializeField] private GameObject enemyPrefabMeleeLent;
        
        private GameObject enemyInstancePrefab;
        private List<GameObject> enemySpawned = new List<GameObject>();

        public enum EnemyType
        {
            MeleeRapide,
            MeleeLent,
            Distance
        }

        private EnemyType nextEnemyType = EnemyType.MeleeRapide;
        
        [Header("Player Information")] 
        [SerializeField] private HeroAttack heroAttack;
        [SerializeField] private Transform heroTransform;
        
        private Vector3 lastTeleportPosition;

        [Header("Timer Spawner")] 
        [SerializeField] private float spawnTimer = 1f;
        private float currentSpawnTimer = 0f;

        [Header("Manage Enemy Quota")] 
        [SerializeField] private Text textQuota;
        private int quota;

        private void Start()
        {
            GenerateQuota();
        }

        private void Update()
        {
            EnemySpawner();
            DisplayQuota();
        }

        private void EnemySpawner()
        {
            if (heroAttack.AttackMode && quota > 0)
            {
                currentSpawnTimer += Time.deltaTime;
                
                if (currentSpawnTimer >= spawnTimer)
                {
                    SpawnEnemyPrefab();
                    currentSpawnTimer = 0f;
                }
            }

            if (enemyInstancePrefab != null)
            {
                if (heroAttack.AttackMode && quota <= 0 && enemyInstancePrefab.GetComponent<EnemyLife>().CurrentHealth <= 0) 
                {
                    StartCoroutine(WaitBeforeGoingBack());
                }   
            }
        }

        private void SpawnEnemyPrefab()
        {
            GameObject enemyPrefab = null;

            switch (nextEnemyType)
            {
                case EnemyType.MeleeRapide:
                    enemyPrefab = enemyPrefabMeleeRapide;
                    nextEnemyType = EnemyType.MeleeLent;
                    break;
                case EnemyType.MeleeLent:
                    enemyPrefab = enemyPrefabMeleeLent;
                    nextEnemyType = EnemyType.Distance;
                    break;
                case EnemyType.Distance:
                    enemyPrefab = enemyPrefabDistance;
                    nextEnemyType = EnemyType.MeleeRapide;
                    break;
            }
            
            if (quota > 0 && enemySpawned.Count <= 5)
            {
                enemyInstancePrefab = Instantiate(enemyPrefab, enemyPosition.position, Quaternion.identity);
                enemySpawned.Add(enemyInstancePrefab);    

                quota--;
            }
            else
            {
                if(enemyInstancePrefab.GetComponent<EnemyLife>().CurrentHealth <= 0)
                {
                    enemySpawned.Clear();
                }
            }
        }

        private void GenerateQuota()
        {
            quota = Random.Range(5, 10);
        }

        private void DisplayQuota()
        {
            textQuota.text = "Quota: " + quota.ToString();
            textQuota.gameObject.SetActive(heroAttack.AttackMode);
        }
        
        public void UpdateLastTeleportPosition(Vector3 position)
        {
            lastTeleportPosition = position;
        }

        IEnumerator WaitBeforeGoingBack()
        {
            yield return new WaitForSeconds(1);
            Debug.Log("Evryone is dead bro");
            heroTransform.position = lastTeleportPosition;
        }
    }
