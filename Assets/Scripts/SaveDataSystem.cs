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

    private const string EncryptionKey = "ravenkey123";

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



    //funcion auxiliar para pasar el valor al get del score y que ya lo cargue
    private async Task LoadCloudHighScore()
    {
        cloudHighScore = await LoadHighScoreFromCloud();
    }

    public async Task<int> LoadHighScoreFromCloud()
    {
        var result = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { HighScoreKey });

        if (result.TryGetValue(HighScoreKey, out var scoreValue))
        {
            return DecryptScore(scoreValue);
        }

        return 0;
    }

    //Cifrar Score
    public async void TrySaveHighScore(int score)
    {
        int bestScore = Mathf.Max(score, cloudHighScore, GetLocalHighScore());

        //Guarda localmente y cifra
        PlayerPrefs.SetString(HighScoreKey, EncryptScore(bestScore));
        PlayerPrefs.SetString(LastSaveTimeKey, DateTime.UtcNow.ToString("o")); 
        PlayerPrefs.Save();

        //Guarda en la nube
        var data = new Dictionary<string, object>
        {
        { HighScoreKey, EncryptScore(bestScore) },
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
        string encrypted = PlayerPrefs.GetString(HighScoreKey, null);
        if (!string.IsNullOrEmpty(encrypted))
            return DecryptScore(encrypted);
        return 0;
    }

    public string GetLastSaveTime()
    {
        return PlayerPrefs.GetString(LastSaveTimeKey, "Saves Not Found");
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


 

    private string EncryptScore(int score)
    {
        string raw = score.ToString();
        char[] key = EncryptionKey.ToCharArray();
        char[] input = raw.ToCharArray();

        for (int i = 0; i < input.Length; i++)
        {
            input[i] = (char)(input[i] ^ key[i % key.Length]);
        }

        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
    }

    private int DecryptScore(string encrypted)
    {
        try
        {
            byte[] data = Convert.FromBase64String(encrypted);
            char[] input = System.Text.Encoding.UTF8.GetChars(data);
            char[] key = EncryptionKey.ToCharArray();

            for (int i = 0; i < input.Length; i++)
            {
                input[i] = (char)(input[i] ^ key[i % key.Length]);
            }

            if (int.TryParse(new string(input), out int score))
            {
                return score;
            }
        }
        catch
        {
            Debug.LogWarning("Score decryption failed.");
        }

        return 0; 
    }
}
