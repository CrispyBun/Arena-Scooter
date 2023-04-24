using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIScoreDisplay scoreDisplay;

    [SerializeField] private GameObject[] enemyShipsBasic;
    [SerializeField] private GameObject[] enemyShipsGliders;

    private static int score;
    private static bool scoreAdded = false;

    private float newWaveMinDelaySeconds = 5f;
    private float newWaveMaxDelaySeconds = 10f;
    private float newWaveTimer = 0f;
    private int waveDifficulty = 0;
    private int maxEnemiesPerSpawn = 3;
    private int enemyCountIncreaseScorePeriod = 2550;
    private int waveDifficultyIncreaseScorePeriod = 1500;
    private int maxEnemiesTotal = 10;

    private void Update()
    {
        if (scoreAdded)
        {
            scoreDisplay.UpdateScore(score);
            scoreAdded = false;
        }

        newWaveTimer -= Time.deltaTime;
        if (newWaveTimer <= 0f)
        {
            ShipEnemy[] enemies = GameObject.FindObjectsOfType<ShipEnemy>();
            if (enemies.Length < maxEnemiesTotal)
            {
                newWaveTimer = Random.Range(newWaveMinDelaySeconds, newWaveMaxDelaySeconds);
                maxEnemiesPerSpawn = 3 + score / enemyCountIncreaseScorePeriod;
                waveDifficulty = score / waveDifficultyIncreaseScorePeriod;
                SpawnWave();
            }
        }
    }

    public static void AddScore(int addedScore)
    {
        score += addedScore;
        scoreAdded = true;
    }

    private void SpawnWave()
    {
        // i made a different system where i had a list of wave spawn methods from which one was chosen and called randomly
        // but honestly it just feels less messy to have one large method with a switch statement
        int chosenWave = Random.Range(0, waveDifficulty+1);
        int enemyCount = Random.Range(1, maxEnemiesPerSpawn+1);

        chosenWave = Mathf.Min(chosenWave, 4);

        GameObject enemy;
        switch (chosenWave)
        {
            case 0: // Just the first enemy
                SpawnEnemies(enemyShipsBasic[0], enemyCount);
                break;

            case 1: // Random basic enemies
                for (int i = 0; i < enemyCount; i++)
                {
                    enemy = enemyShipsBasic[Random.Range(0, enemyShipsBasic.Length)];
                    SpawnEnemies(enemy);
                }
                break;

            case 2: // Fly through attack
                enemy = enemyShipsGliders[Random.Range(0, enemyShipsGliders.Length)];
                Vector2 arenaBounds = InBoundKeeper.arena.GetBounds();

                enemyCount = Mathf.Max(enemyCount, 2);
                float arenaBoundWiggleRoom = InBoundKeeper.arena.GetSafeBoundWiggleRoom();
                for (int i = 0; i < enemyCount; i++)
                {
                    float verticalPosition = ((float)(i) / (float)(enemyCount - 1) - 0.5f) * arenaBounds.y / 2;
                    Vector3 enemyPosition = new Vector3(-arenaBounds.x / 2 - arenaBoundWiggleRoom / 2, verticalPosition, 0);
                    Instantiate(enemy, enemyPosition, new Quaternion());
                }
                break;

            case 3: // Scattered fly through attack
                enemyCount *= 4;
                enemy = enemyShipsGliders[Random.Range(0, enemyShipsGliders.Length)];
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                for (int i = 0; i < enemyCount; i++)
                {
                    Vector3 enemyPosition = InBoundKeeper.arena.GetRandomPointInOuterSafeBounds();
                    Instantiate(enemy, enemyPosition, Quaternion.Euler(0, 0, Random.value * 360));
                }
                break;

            case 4: // Just the second enemy
                enemyCount /= 2;
                SpawnEnemies(enemyShipsBasic[1], enemyCount);
                break;
        }
    }

    private void SpawnEnemies(GameObject enemy, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 enemyPosition = InBoundKeeper.arena.GetRandomPointInOuterSafeBounds();
            Instantiate(enemy, enemyPosition, new Quaternion());
        }
    }
}
