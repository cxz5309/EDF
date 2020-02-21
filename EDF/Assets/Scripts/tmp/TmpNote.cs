using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TmpNote : MonoBehaviour
{
    private string tmpMusicTitle;
    private string tmpMusicLevel;

    public static TmpNote instance;
    string title;
    string artist;
    public float bpm;
    public string backgroundSoundName;
    string keySoundName;
    public int fullComboCnt = 0;
    public float beatStartTime;
    public float musicTime;
    public float defaultSpeed;

    float beatIntervalTime;
    float enemySpeed;
    public float enemyReachTime;

    private List<Enemy> upEnemyList = new List<Enemy>();
    private List<Enemy> downEnemyList = new List<Enemy>();

    public Transform[] spawnPoint;

    public int enemyListNowIndex;
    public float checkTime = 0;

    private void Awake()
    {
        instance = this;
    }

    public void InitNote()
    {
        tmpMusicTitle = MakeData.instance.title;
        tmpMusicLevel = MakeData.instance.level;

        Debug.Log("Beats/" + tmpMusicTitle + tmpMusicLevel);
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + tmpMusicTitle + tmpMusicLevel);
        StringReader reader = new StringReader(textAsset.text);
        title = reader.ReadLine();
        artist = reader.ReadLine();
        backgroundSoundName = reader.ReadLine();
        keySoundName = reader.ReadLine();
        beatStartTime = float.Parse(reader.ReadLine());
        musicTime = float.Parse(reader.ReadLine());
        bpm = float.Parse(reader.ReadLine());
        defaultSpeed = float.Parse(reader.ReadLine());
        Debug.Log("곡 정보 읽기 완료");

        beatIntervalTime = 60 / bpm;
        enemySpeed = defaultSpeed * bpm / 60;
        enemyReachTime = (1 / enemySpeed) * 15;


        string line;
        int j = 0;
        int k = 0;
        while (true)
        {
            if ((line = reader.ReadLine()) != null)
            {
                char[] tmp = line.ToCharArray();// 0:노트x 1:노트 2:양쪽노트 4:롱노트토글 5:롱노트 진행
                for (int i = 0; i < line.Length; i++, j++)
                {
                    upEnemyList.Add(new Enemy(0, int.Parse(tmp[i].ToString()), j));
                    BpmAnalyzer.instance.allNotes++;
                }
            }
            else
            {
                break;
            }
            if ((line = reader.ReadLine()) != null)
            {
                char[] tmp = line.ToCharArray();
                for (int i = 0; i < line.Length; i++, k++)
                {
                    downEnemyList.Add(new Enemy(1, int.Parse(tmp[i].ToString()), k));
                    if (tmp[i] != '0')
                    {
                        fullComboCnt++;
                    }
                }
            }
            else
            {
                break;
            }
        }
    }

    public void SaveParsedData(string _musicTitle, string _musicArtist, int _fullComboCnt)
    {
        DataSave.instance.parsedData.SetParsedData(_musicTitle, _musicArtist, _fullComboCnt);
    }

    public void NoteStart()
    {
        StartCoroutine("coIntervalMakeNote");
    }

    IEnumerator coIntervalMakeNote()
    {
        yield return new WaitForSeconds(beatStartTime);

        for (enemyListNowIndex = 0; enemyListNowIndex < upEnemyList.Count; enemyListNowIndex++)
        {
            
            while (checkTime < beatIntervalTime)
            {
                if (!BpmAnalyzer.instance.isPitching)
                {
                    checkTime += Time.fixedDeltaTime;
                }
                yield return new WaitForFixedUpdate();
            }
            if (enemyListNowIndex != 0 && enemyListNowIndex % 32 == 0)
            {
                BpmAnalyzer.instance.onPauseButtonClick();
            }
            BpmAnalyzer.instance.ShowNowBeat(enemyListNowIndex);

            if (!BpmAnalyzer.instance.isPitching)
            {
                checkTime -= beatIntervalTime;
                //Debug.Log("checkTime = " + checkTime);
                
                //yield return beatIntervalWait;
                //Debug.Log(Time.time);
                MakeNote(upEnemyList[enemyListNowIndex]);
                MakeNote(downEnemyList[enemyListNowIndex]);
            }
        }
    }

    public void EnemyListIndexChange(int val)
    {
        enemyListNowIndex += val;
    }

    public void NoteEnd()
    {
        StopCoroutine("coIntervalMakeNote");
    }

    public void MakeNote(Enemy enemy)
    {
        GameObject obj;

        switch (enemy.type)
        {
            case 0:
                break;
            case 1:
                obj = ObjectPoolContainer.Instance.Pop("TmpRed");//타입에 맞는 몬스터 오브젝트풀에서 생성
                obj.transform.position = spawnPoint[enemy.upOrdown].position;
                obj.GetComponent<EnemyInfo>().upOrdown = enemy.upOrdown;
                obj.GetComponent<EnemyInfo>().type = enemy.type;//타입 정해주기
                obj.GetComponent<EnemyInfo>().speed = enemySpeed;//도달 시간
                obj.SetActive(true);
                obj.GetComponent<EnemyInfo>().goleft();
                break;
            case 2:
                if (enemy.upOrdown == 0)
                {
                    obj = ObjectPoolContainer.Instance.Pop("TmpBlue");//타입에 맞는 몬스터 오브젝트풀에서 생성
                    obj.transform.position = spawnPoint[2].position;
                    obj.GetComponent<EnemyInfo>().upOrdown = 2;
                    obj.GetComponent<EnemyInfo>().type = enemy.type;//타입 정해주기
                    obj.GetComponent<EnemyInfo>().speed = enemySpeed;//도달 시간
                    obj.SetActive(true);
                    obj.GetComponent<EnemyInfo>().goleft();
                }
                break;
            case 3:
                obj = ObjectPoolContainer.Instance.Pop("TmpGreen");//타입에 맞는 몬스터 오브젝트풀에서 생성
                obj.transform.position = spawnPoint[enemy.upOrdown].position;
                obj.GetComponent<EnemyInfo>().upOrdown = enemy.upOrdown;
                obj.GetComponent<EnemyInfo>().type = enemy.type;//타입 정해주기
                obj.GetComponent<EnemyInfo>().speed = enemySpeed;//도달 시간
                obj.SetActive(true);
                obj.GetComponent<EnemyInfo>().goleft();
                break;
            case 4:
                obj = ObjectPoolContainer.Instance.Pop("TmpPerple");//타입에 맞는 몬스터 오브젝트풀에서 생성
                obj.transform.position = spawnPoint[enemy.upOrdown].position;
                obj.GetComponent<EnemyInfo>().upOrdown = enemy.upOrdown;
                obj.GetComponent<EnemyInfo>().type = enemy.type;//타입 정해주기
                obj.GetComponent<EnemyInfo>().speed = enemySpeed;//도달 시간
                obj.SetActive(true);
                obj.GetComponent<EnemyInfo>().goleft();
                break;
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
