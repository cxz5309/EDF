using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public string GameDataAchievement(string a, ParsedData parsedData, GameData gameData)
    {
        switch (a)
        {
            default:
                a = null;
                break;
            case "소닉":
                break;
            case "둠의영혼":
                if (parsedData.musicTitle != "Learning is Fun")
                    a = null;
                break;
            case "까마귀":
                if (parsedData.musicTitle != "Daughter")
                    a = null;
                break;
            case "치코리타":
                if (parsedData.musicTitle != "Run Run Rottytops!")
                    a = null;
                break;
            case "6V엘풍":
                if (!(gameData.isGameClear == false))
                    a = null;
                break;
            case "팅글":
                if (!((gameData.scoreRank == "S"||(gameData.scoreRank == "A+")|| (gameData.scoreRank == "S+"))))
                    a = null;
                break;
            case "대적자":
                if (!(gameData.isFullCombo))
                    a = null;
                break;
        }
        switch (a)
        {
            case "치코리타":
            case "소닉":
            case "둠의영혼":
            case "까마귀":
                if (!gameData.isGameClear)
                    a = null;
                break;
        }
        return a;
    }
}