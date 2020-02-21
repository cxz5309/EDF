using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NoteManager : MonoBehaviour
{    
    public static NoteManager instance;
    string title;
    string artist;
    float bpm;
    public float defaultSpeed;

    public string backgroundSoundName;
    public string keySoundName;
    public float beatStartTime;
    public float musicTime;
    int fullComboCnt = 0;

    float beatIntervalTime;
    float enemySpeed;
    public float enemyReachTime;

    private List<Enemy> upEnemyList = new List<Enemy>();
    private List<Enemy> downEnemyList = new List<Enemy>();

    public Transform []spawnPoint;

    private void Awake()
    {
        instance = this;
    }

    public void InitNote(string musicTxt, int level)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + musicTxt + level.ToString());
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
            if ((line = reader.ReadLine()) != null)
            {
                char[] tmp = line.ToCharArray();
                for (int i = 0; i < line.Length; i++, j++)
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

        SaveParsedData(title, artist, fullComboCnt);
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
        float checkTime = 0;
        
        for (int i = 0; i < upEnemyList.Count; i++)
        {
            while (checkTime < beatIntervalTime)
            {
                checkTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            checkTime -= beatIntervalTime;
            
            MakeNote(upEnemyList[i]);
            MakeNote(downEnemyList[i]);
        }
        GameManager.instance.NoteEnd(beatIntervalTime);
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
                obj = ObjectPoolContainer.Instance.Pop("TmpRed");//상, 하 노트 타입에 맞는 몬스터 오브젝트풀에서 생성
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
                    obj = ObjectPoolContainer.Instance.Pop("TmpBlue");//양쪽 노트 타입에 맞는 몬스터 오브젝트풀에서 생성
                    obj.transform.position = spawnPoint[2].position;
                    Debug.Log(obj.GetComponent<EnemyInfo>().type);
                    obj.GetComponent<EnemyInfo>().upOrdown = 2;
                    obj.GetComponent<EnemyInfo>().type = enemy.type;//타입 정해주기
                    obj.GetComponent<EnemyInfo>().speed = enemySpeed;//도달 시간
                    obj.SetActive(true);
                    obj.GetComponent<EnemyInfo>().goleft();
                }
                break;
            case 3:
                obj = ObjectPoolContainer.Instance.Pop("TmpGreen");//롱노트 끝 타입에 맞는 몬스터 오브젝트풀에서 생성
                obj.transform.position = spawnPoint[enemy.upOrdown].position;
                obj.GetComponent<EnemyInfo>().upOrdown = enemy.upOrdown;
                obj.GetComponent<EnemyInfo>().type = enemy.type;//타입 정해주기
                obj.GetComponent<EnemyInfo>().speed = enemySpeed;//도달 시간
                obj.SetActive(true);
                obj.GetComponent<EnemyInfo>().goleft();
                break;
            case 4:
                obj = ObjectPoolContainer.Instance.Pop("TmpPerple");//롱노트 중간 타입에 맞는 몬스터 오브젝트풀에서 생성
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
