using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIScoreDisplay scoreDisplay;
    [SerializeField] private GameObject canvasGameOver;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject enemyShipInvader;
    [SerializeField] private GameObject enemyShipStinger;
    [SerializeField] private GameObject enemyShipScout;
    [SerializeField] private GameObject enemyShipRadar;
    [SerializeField] private GameObject enemyShipSplurter;

    private static int score;
    private static bool scoreAdded = false;

    private static TimeSpan playtime;
    private static DateTime playtimeTrackingPoint;

    private float newWaveMinDelaySeconds = 2f;
    private float newWaveMaxDelaySeconds = 5f;
    private float newWaveTimer = 0_0;
    private int waveDifficulty = 0_0;
    private int maxEnemiesPerSpawn = 1;
    private int enemyCountIncreaseScorePeriod = 2550;
    private int waveDifficultyIncreaseScorePeriod = 2048;
    private int maxEnemiesTotal = 10;

    static GameManager()
    {
        playtime = LoadPlaytime();
    }

    private void Start()
    {
        ResetScore();
        canvasGameOver.SetActive(false);

        StartPlaytimeTracking();
    }

    private void Update()
    {
        if (!player && !canvasGameOver.activeSelf)
        {
            canvasGameOver.SetActive(true);
            SaveHighScore(score);

            StopPlaytimeTracking();
        }

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
                newWaveTimer = UnityEngine.Random.Range(newWaveMinDelaySeconds, newWaveMaxDelaySeconds);
                maxEnemiesPerSpawn = 1 + score / enemyCountIncreaseScorePeriod;
                waveDifficulty = score / waveDifficultyIncreaseScorePeriod;
                SpawnWave();
            }
        }
    }

    public static void ResetScore()
    {
        score = 0;
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
        int chosenWave = UnityEngine.Random.Range(0, waveDifficulty+1);
        int enemyCount = UnityEngine.Random.Range(1, maxEnemiesPerSpawn+1);

        chosenWave = Mathf.Min(chosenWave, 6);

        switch (chosenWave)
        {
            case 0: // Just the first enemy
                SpawnEnemies(enemyShipInvader, enemyCount);
                break;

            case 1: // Some scouts mixed in
                enemyCount = Mathf.Min(enemyCount, 2);
                int invaderCount = enemyCount / 2;
                SpawnEnemies(enemyShipInvader, invaderCount);
                SpawnEnemies(enemyShipScout, enemyCount - invaderCount);
                break;

            case 2: // Fly through attack
                Vector2 arenaBounds = InBoundKeeper.arena.GetBounds();

                enemyCount = Mathf.Max(enemyCount, 2);
                float arenaBoundWiggleRoom = InBoundKeeper.arena.GetSafeBoundWiggleRoom();
                for (int i = 0; i < enemyCount; i++)
                {
                    float verticalPosition = ((float)(i) / (float)(enemyCount - 1) - 0.5f) * arenaBounds.y * 0.9f;
                    Vector3 enemyPosition = new Vector3(-arenaBounds.x / 2 - arenaBoundWiggleRoom / 2, verticalPosition, 0);
                    Instantiate(enemyShipStinger, enemyPosition, new Quaternion());
                }
                break;

            case 3: // Scattered fly through attack
                enemyCount *= 4;
                for (int i = 0; i < enemyCount; i++)
                {
                    Vector3 enemyPosition = InBoundKeeper.arena.GetRandomPointInOuterSafeBounds();
                    Instantiate(enemyShipStinger, enemyPosition, Quaternion.Euler(0, 0, UnityEngine.Random.value * 360));
                }
                break;

            case 4: // Just scouts
                enemyCount /= 3;
                SpawnEnemies(enemyShipScout, enemyCount);
                break;

            case 5: // Radars (upgraded scouts)
                enemyCount /= 3;
                SpawnEnemies(enemyShipRadar, enemyCount);
                break;

            case 6: // Splurters
                enemyCount /= 5;
                SpawnEnemies(enemyShipSplurter, enemyCount);
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
    
    public static void SaveHighScore(int savedScore)
    {
        Directory.CreateDirectory("savedata");

        string filename = "savedata/highscores.txt";

        if (File.Exists(filename))
        {
            StreamReader loadStream = new StreamReader(filename);
            string[] lines = loadStream.ReadToEnd().Split('\n');
            loadStream.Close();

            List<int> highScores = new List<int>();
            for (int i = 0; i < lines.Length-1; i++)
            {
                highScores.Add(Convert.ToInt32(lines[i]));
            }

            int highScoresCount = highScores.Count;
            for (int i = 0; i < highScoresCount; i++)
            {
                if (savedScore > highScores[i])
                {
                    highScores.Insert(i, savedScore);
                    break;
                }
                else if (i == highScores.Count - 1)
                {
                    highScores.Add(savedScore);
                }
            }

            if (highScores.Count > 10) highScores.RemoveRange(highScores.Count-1, highScores.Count-10);

            StreamWriter saveStream = new StreamWriter(filename);
            for (int i = 0; i < highScores.Count; i++)
            {
                saveStream.WriteLine(highScores[i]);
            }
            saveStream.Close();
        }
        else
        {
            StreamWriter saveStream = new StreamWriter(filename);
            saveStream.WriteLine(savedScore);
            saveStream.Close();
        }
    }

    public static string GetHighScores()
    {
        string filename = "savedata/highscores.txt";

        if (File.Exists(filename))
        {
            StreamReader loadStream = new StreamReader(filename);
            string[] lines = new string[10];
            for (int i = 0; i < lines.Length; i++)
            {
                string nextLine = loadStream.ReadLine();
                if (nextLine == null) nextLine = "-----";
                lines[i] = nextLine;
            }
            loadStream.Close();
            return String.Join('\n', lines);


        }
        return "-----\n-----\n-----\n-----\n-----\n-----\n-----\n-----\n-----\n-----";
    }

    public static void StartPlaytimeTracking()
    {
        playtimeTrackingPoint = DateTime.Now;
    }
    public static void StopPlaytimeTracking()
    {
        playtime = playtime.Add(DateTime.Now.Subtract(playtimeTrackingPoint));
        SavePlaytime();
    }
    public static string GetPlayTime()
    {
        return "Playtime: " + playtime.ToString(@"hh\:mm\:ss");
    }

    private static void SavePlaytime()
    {
        Directory.CreateDirectory("savedata");

        string filename = "savedata/playtime.txt";

        StreamWriter saveStream = new StreamWriter(filename);
        saveStream.WriteLine(playtime.ToString());
        saveStream.Close();
    }

    private static TimeSpan LoadPlaytime()
    {
        Directory.CreateDirectory("savedata");

        string filename = "savedata/playtime.txt";


        if (File.Exists(filename))
        {
            StreamReader loadStream = new StreamReader(filename);
            string time = loadStream.ReadToEnd();
            loadStream.Close();

            return TimeSpan.Parse(time);
        }

        return new TimeSpan();
    }
}
