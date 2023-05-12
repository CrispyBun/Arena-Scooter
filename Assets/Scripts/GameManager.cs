using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIScoreDisplay scoreDisplay;
    [SerializeField] private GameObject canvasGameOver;

    private AudioSource newWeaponSound;
    [SerializeField] private GameObject[] playerWeapons;
    private int lastPickedWeapon = -1;

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

    private static int newWeaponScoreInterval = 2000;
    private static int newWeaponScoreProgress = 0_0;

    static GameManager()
    {
        playtime = LoadPlaytime();
    }

    private void Start()
    {
        newWeaponSound = GetComponent<AudioSource>();

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

        if (newWeaponScoreProgress >= newWeaponScoreInterval)
        {
            newWeaponSound.Play();
            newWeaponScoreProgress = 0;
            AssignNewWeapon();
        }
    }

    private void AssignNewWeapon()
    {
        if (player)
        {
            for (int i = 0; i < player.transform.childCount; i++)
            {
                Transform child = player.transform.GetChild(i);
                if (!child.transform.gameObject.CompareTag("Weapon")) continue;

                Destroy(child.gameObject);

                int newWeaponIndex = UnityEngine.Random.Range(0, playerWeapons.Length);
                if (newWeaponIndex == lastPickedWeapon) newWeaponIndex++;
                if (newWeaponIndex >= playerWeapons.Length) newWeaponIndex = 0;

                GameObject newWeapon = Instantiate(playerWeapons[newWeaponIndex]);
                newWeapon.transform.parent = player.transform;
                newWeapon.transform.localPosition = new Vector3(1f, 0f, 0f);
                newWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);

                lastPickedWeapon = newWeaponIndex;
                break;
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

        ProgressNewWeapon(addedScore);
    }

    private static void ProgressNewWeapon(int addedScore)
    {
        newWeaponScoreProgress += addedScore;
    }

    public static float GetNewWeaponProgress()
    {
        return ((float)(newWeaponScoreProgress)) / ((float)newWeaponScoreInterval);
    }

    private void SpawnWave()
    {
        // i made a different system where i had a list of wave spawn methods from which one was chosen and called randomly
        // but honestly it just feels less messy to have one large method with a switch statement
        int chosenWave = UnityEngine.Random.Range(0, Mathf.Min(waveDifficulty +1, 6));
        int enemyCount = UnityEngine.Random.Range(1, maxEnemiesPerSpawn+1);

        switch (chosenWave)
        {
            case 0: // Just the first enemy
                SpawnEnemies(enemyShipInvader, enemyCount);
                break;

            case 1: // Some scouts mixed in
                enemyCount = Mathf.Max(enemyCount, 2);
                int invaderCount = enemyCount / 2;
                SpawnEnemies(enemyShipInvader, invaderCount);
                SpawnEnemies(enemyShipScout, enemyCount - invaderCount);
                break;

            case 2: // Fly through attack
                Vector2 arenaBounds = InBoundKeeper.arena.GetBounds();

                enemyCount = Mathf.Max(enemyCount, 2);
                enemyCount = Mathf.Min(enemyCount, 8);
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

            case 6: // Splurters and basics
                enemyCount = Mathf.Max(enemyCount, 4);
                int splurtCount = enemyCount / 5;
                SpawnEnemies(enemyShipSplurter, splurtCount);
                SpawnEnemies(enemyShipInvader, enemyCount - splurtCount);
                break;
        }
    }

    private void SpawnEnemies(GameObject enemy, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 enemyPosition = InBoundKeeper.arena.GetRandomPointInOuterSafeBounds();
            if (count > 10)
            {
                enemyPosition = InBoundKeeper.arena.GetRandomPointFromSide();
            }    

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
