using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseParser : MonoBehaviour
{
    public static FirebaseParser instance;

    FirebaseDatabase mDatabase;

    DataSnapshot MusicVolumeSnapshot;
    DataSnapshot EffectVolumeSnapshot;
    DataSnapshot NowPetSnapshot;
    DataSnapshot HavePetSnapshot;
    DataSnapshot AchievementSnapshot;
    DataSnapshot Music_VestScoreSnapshot;

    bool volumeOn = false;
    bool effectOn = false;
    bool nowPetOn = false;
    bool havePetOn = false;
    bool achievementOn = false;
    bool music_VestScoreOn = false;

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

    public void InitFirebaseParser()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://api-project-18200989.firebaseio.com/");
        mDatabase = FirebaseDatabase.DefaultInstance;
    }

    public void GetFirebaseDB()
    {
        GetDBNowVolume();
        GetDBNowPet();
        GetDBHavePet();
        GetDBAchievement();
        GetMusic_VestScore();
    }

    #region 파이어베이스 읽기
    public void GetDBNowVolume()
    {
        mDatabase.GetReference("현재 볼륨").Child("음악 볼륨")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("음악 볼륨 찾기 실패!");
                }
                else if (task.IsCompleted)
                {
                    MusicVolumeSnapshot = task.Result;
                    volumeOn = true;
                    InitNowMusicVolume();

                    if (volumeOn && effectOn && nowPetOn && havePetOn && achievementOn)
                    {
                        LoddingManager.instance.step3_DataLoad = true;
                        LoddingManager.instance.step2_Firebase = false;
                        LoddingManager.instance.isStepProcessing = false;
                    }
                }
            });
        mDatabase.GetReference("현재 볼륨").Child("이펙트 볼륨")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("이펙트 볼륨 찾기 실패!");
                }
                else if (task.IsCompleted)
                {
                    EffectVolumeSnapshot = task.Result;
                    effectOn = true;
                    InitNowEffectVolume();

                    if (volumeOn && effectOn && nowPetOn && havePetOn && achievementOn)
                    {
                        LoddingManager.instance.step3_DataLoad = true;
                        LoddingManager.instance.step2_Firebase = false;
                        LoddingManager.instance.isStepProcessing = false;
                    }
                }
            });
    }

    public void InitNowMusicVolume()
    {
        Debug.Log("현재 음악볼륨 찾아옴");
        Debug.Log("음악 볼륨값은 : " + MusicVolumeSnapshot.Value);
        DataSave.instance.dbData.SetNowMusicVolume(float.Parse(MusicVolumeSnapshot.Value.ToString()));
    }

    public void InitNowEffectVolume()
    {

        Debug.Log("현재 이펙트 볼륨 찾아옴");
        Debug.Log("이펙트 볼륨값은 : " + EffectVolumeSnapshot.Value);
        DataSave.instance.dbData.SetNowEffectVolume(float.Parse(EffectVolumeSnapshot.Value.ToString()));
    }
    
    public void GetDBNowPet()
    {
        mDatabase.GetReference("현재 펫")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("현재 펫 찾기 실패!");
                }
                else if (task.IsCompleted)
                {
                    NowPetSnapshot = task.Result;
                    nowPetOn = true;
                    InitNowPetData();
                    if (volumeOn && effectOn && nowPetOn && havePetOn && achievementOn)
                    {
                        LoddingManager.instance.step3_DataLoad = true;
                        LoddingManager.instance.step2_Firebase = false;
                        LoddingManager.instance.isStepProcessing = false;
                    }
                }
            });
    }
    public void GetDBHavePet()
    {
        mDatabase.GetReference("가진 펫")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("가진 펫 찾기 실패!");
                }
                else if (task.IsCompleted)
                {
                    HavePetSnapshot = task.Result;

                    havePetOn = true;
                    InitHavePetData();
                    if (volumeOn && effectOn && nowPetOn && havePetOn && achievementOn)
                    {
                        LoddingManager.instance.step3_DataLoad = true;
                        LoddingManager.instance.step2_Firebase = false;
                        LoddingManager.instance.isStepProcessing = false;
                    }
                }
            });
    }
    public void InitNowPetData()
    {
        Debug.Log("현재 펫 찾아옴");
        Debug.Log("현재 펫은 : " + NowPetSnapshot.Value);
        DataSave.instance.dbData.SetNowPet(NowPetSnapshot.Value.ToString());
    }
    public void InitHavePetData()
    {
        Debug.Log("가진 펫 찾아옴");
        Debug.Log("가진 펫은 : " + HavePetSnapshot.Value);

        DataSave.instance.dbData.InitAllHavePetData((Dictionary<string, object>)HavePetSnapshot.Value);
    }

    public void GetDBAchievement()
    {
        mDatabase.GetReference("달성 업적")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("업적 찾기 실패!");
                }
                else if (task.IsCompleted)
                {
                    AchievementSnapshot = task.Result;

                    achievementOn = true;
                    InitAchievementData();
                    if (volumeOn && effectOn && nowPetOn && havePetOn && achievementOn)
                    {
                        LoddingManager.instance.step3_DataLoad = true;
                        LoddingManager.instance.step2_Firebase = false;
                        LoddingManager.instance.isStepProcessing = false;
                    }
                }
            });
    }

    public void InitAchievementData()
    {
        Debug.Log("달성 업적 찾아옴");
        Debug.Log("가진 업적은 : " + AchievementSnapshot.Value);

        DataSave.instance.dbData.InitAllHaveAchieveData((Dictionary<string, object>)AchievementSnapshot.Value);
    }
    public void GetMusic_VestScore()
    {
        mDatabase.GetReference("노래")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("노래 찾기 실패!");
                }
                else if (task.IsCompleted)
                {
                    Music_VestScoreSnapshot = task.Result;

                    music_VestScoreOn = true;
                    InitMusic_VestScoreData();
                    if (volumeOn && effectOn && nowPetOn && havePetOn && achievementOn)
                    {
                        LoddingManager.instance.step3_DataLoad = true;
                        LoddingManager.instance.step2_Firebase = false;
                        LoddingManager.instance.isStepProcessing = false;
                    }
                }
            });
    }

    public void InitMusic_VestScoreData()
    {
        Debug.Log("노래 최고점수 찾아옴");
        Debug.Log("노래 최고점수는 : " + Music_VestScoreSnapshot.Value);

        DataSave.instance.dbData.InitAllMusic_VestScoreData((Dictionary<string, object>)Music_VestScoreSnapshot.Value);
    }
    #endregion

    #region 파이어베이스 쓰기
    public void SetVolumeDBToFirebase()
    {
        mDatabase.GetReference("현재 볼륨").Child("음악 볼륨").SetValueAsync(DataSave.instance.dbData.nowMusicVolume);
        mDatabase.GetReference("현재 볼륨").Child("이펙트 볼륨").SetValueAsync(DataSave.instance.dbData.nowEffectVolume);
        Debug.Log("현재 음악 볼륨 파이어베이스에 쓰기 완료 : " + DataSave.instance.dbData.nowMusicVolume);
        Debug.Log("현재 이펙트 볼륨 파이어베이스에 쓰기 완료 : " + DataSave.instance.dbData.nowEffectVolume);
    }

    public void SetNowPetDBtoFirebase()
    {
        mDatabase.GetReference("현재 펫").SetValueAsync(DataSave.instance.dbData.nowPet);
        Debug.Log("현재 펫 파이어베이스에 쓰기 완료 : " + DataSave.instance.dbData.nowPet);
    }

    public void SetHavePetDBtoFirebase(string _petName)
    {
        mDatabase.GetReference("가진 펫").Child(_petName).SetValueAsync(DataSave.instance.dbData.havePet[_petName]);
        Debug.Log("가진 펫 파이어베이스에 쓰기 완료 : " + _petName);
    }

    public void SetHaveAchieveToFirebase(string _petName)
    {
        mDatabase.GetReference("달성 업적").Child(_petName).SetValueAsync(DataSave.instance.dbData.haveAchieve[_petName]);
        Debug.Log("달성 업적 파이어베이스에 쓰기 완료 : " + _petName);

    }

    public void SetVestScoreToFirebase(string _artist_music)
    {
        mDatabase.GetReference("노래").Child(_artist_music).SetValueAsync(DataSave.instance.dbData.musicVestScore[_artist_music]);
        Debug.Log("최고 점수 파이어베이스에 쓰기 완료 : " + _artist_music);

    }
    #endregion

    //public void SetDatabase()
    //{
    //    Dictionary<string, object> name = new Dictionary<string, object>();
    //    mDatabaseRef.Child("ID").SetValueAsync("1");
    //    mDatabaseRef.Child("ID").Push().SetValueAsync("kim", "1");
    //    mDatabaseRef.Child("ID").Push().SetValueAsync("lee");
    //    mDatabaseRef.Child("PASSWARD");
    //}
}


