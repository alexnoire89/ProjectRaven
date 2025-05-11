using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using Unity.Services.CloudSave.Models;
using UnityEngine;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class SaveDataSystem : MonoBehaviour
{
    public static SaveDataSystem Instance;

    private const string HighScoreKey = "highscore";
    private int cloudHighScore = -1;
    private const string LastSaveTimeKey = "last_save_time";

    private int sessionBestScore = 0;

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            await LoadCloudHighScore(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetSessionScore();
    }

    //funcion auxiliar para pasar el valor al get del score y que ya lo cargue
    private async Task LoadCloudHighScore()
    {
        cloudHighScore = await LoadHighScoreFromCloud();
    }

    public async void TrySaveHighScore(int score)
    {
        int bestScore = Mathf.Max(score, cloudHighScore, GetLocalHighScore());

        //Guarda localmente
        PlayerPrefs.SetInt(HighScoreKey, bestScore);
        PlayerPrefs.SetString(LastSaveTimeKey, DateTime.UtcNow.ToString("o")); 
        PlayerPrefs.Save();

        //Guarda en la nube
        //var data = new Dictionary<string, object> { { HighScoreKey, bestScore.ToString() } };
        //await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        var data = new Dictionary<string, object>
        {
        { HighScoreKey, bestScore.ToString() },
        { LastSaveTimeKey, DateTime.UtcNow.ToString("o") }
        };

        await CloudSaveService.Instance.Data.ForceSaveAsync(data);

        cloudHighScore = bestScore;
    }


    public int GetHighScore()
    {
        if (cloudHighScore >= 0)
        {
            return cloudHighScore;
        }
        else
        {
            return GetLocalHighScore();
        }
    }

    private int GetLocalHighScore()
    {
        //Si no lo encuentra devuelve 0
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public string GetLastSaveTime()
    {
        return PlayerPrefs.GetString(LastSaveTimeKey, "Saves Not Found");
    }

    public async Task<int> LoadHighScoreFromCloud()
    {
        var result = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { HighScoreKey });

        if (result.TryGetValue(HighScoreKey, out var scoreValue))
        {
            if (int.TryParse(scoreValue, out int parsedScore))
            {
                return parsedScore;
            }
        }

        return 0;
    }



    public void UpdateSessionBestScore(int currentScore)
    {
        if (currentScore > sessionBestScore)
        {
            sessionBestScore = currentScore;
        }
    }

    public void ResetSessionScore()
    {
        sessionBestScore = 0;
    }

    public int GetSessionBestScore()
    {
        return sessionBestScore;
    }
}
