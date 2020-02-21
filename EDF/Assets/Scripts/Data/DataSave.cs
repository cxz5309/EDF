using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBData
{
    //게임 시작 -> 로비 씬 게임 플레이어의 재화 및 이전 세팅된 값으로 초기화
    //게임시작시 db에서 받아옴
    public float nowMusicVolume;//볼륨을 받아와서 모든 볼륨세팅의 초기값을 이걸로 맞춰주어야 하고 볼륨 저장 키 누르면 파베에 저장하기
    public float nowEffectVolume;//볼륨을 받아와서 모든 볼륨세팅의 초기값을 이걸로 맞춰주어야 하고 볼륨 저장 키 누르면 파베에 저장하기
    public string nowPet;//현재 펫 받아와서 펫 초기화면 이걸로 맞추기
    public Dictionary<string, object> havePet = new Dictionary<string, object>();
    public Dictionary<string, object> haveAchieve = new Dictionary<string, object>();
    public Dictionary<string, object> musicVestScore = new Dictionary<string, object>();//로비 씬->게임 씬 각 스테이지당 영구저장된 값들 (최고 점수)

    public void SetNowMusicVolume(float _nowMusicVolume)
    {
        this.nowMusicVolume = _nowMusicVolume;
    }
    public void SetNowEffectVolume(float _nowEffectVolume)
    {
        this.nowEffectVolume = _nowEffectVolume;
    }

    public void SetNowPet(string _nowPet)
    {
        this.nowPet = _nowPet;
    }
    
    public void SetHavePet(string _petName, bool _havePet)
    {
        havePet[_petName] = _havePet;
    }

    public void InitAllHavePetData(Dictionary<string, object> FirebaseData)
    {
        havePet = FirebaseData;
    }

    public void InitAllHaveAchieveData(Dictionary<string, object> FirebaseData)
    {
        haveAchieve = FirebaseData;
    }

    public void SetHaveAchieve(string _petName, bool _haveAchieve)
    {
        haveAchieve[_petName] = _haveAchieve;
    }

    public void InitAllMusic_VestScoreData(Dictionary<string, object> FirebaseData)
    {
        musicVestScore = FirebaseData;
    }

    public void SetMusic_VestScore(string artist_music, int vestScore)
    {
        musicVestScore[artist_music] = vestScore;
    }
}

public class SelectedData
{
    //로비 씬->게임 씬 (musicTxt + level로 스테이지 세팅, DB읽기, txt파일 읽기)
    public string musicTxt;//로비창 사용자의 선택결과
    public int level;//로비창 사용자의 선택결과

    public void SetSelectedData(string _musicTxt, int _level)
    {
        this.musicTxt = _musicTxt;
        this.level = _level;
    }
    public void InitSelectedData()
    {
        this.musicTxt = null;
        this.level = 0;
    }
}

public class ParsedData
{
    //게임 씬->리절트 씬 
    //txt읽은 세팅 결과로 나온 것 중 리절트 신에 가져가야 할 것들 노트매니저에서 입력
    public string musicTitle;
    public string musicArtist;
    public int fullComboCnt;

    public void SetParsedData(string _musicTitle, string _musicArtist, int _fullComboCnt)
    {
        this.musicTitle = _musicTitle;
        this.musicArtist = _musicArtist;
        this.fullComboCnt = _fullComboCnt;
    }
}

public class GameData
{
    //게임중에 갱신되는 것들
    public string scoreRank;

    public int score;
    public int maxCombo = 0;
    public int perfectCnt;
    public int goodCnt;
    public int missCnt;

    public bool isGameClear;
    public bool isVestScore;
    public bool isFullCombo;

    public void InitGameData()
    {
        this.isGameClear = false;
        this.score = 0;
        this.scoreRank = "F";
        this.maxCombo = 0;
        this.perfectCnt = 0;
        this.goodCnt = 0;
        this.missCnt = 0;
        this.isVestScore = false;
        this.isFullCombo = false;
    }
}




public class DataSave : MonoBehaviour
{
    public static DataSave instance;

    public GameData gameData;
    public ParsedData parsedData;
    public SelectedData selectedData;
    public DBData dbData;

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
    public void InitAllDataNull()
    {
        gameData = new GameData();
        parsedData = new ParsedData();
        selectedData = new SelectedData();
        dbData = new DBData();
    }
}
