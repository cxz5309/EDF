using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolContainer : MonoBehaviour {
    private static ObjectPoolContainer instance;
    public static ObjectPoolContainer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("ObjectPoolContainer");
                instance = obj.AddComponent<ObjectPoolContainer>();
            }
            return instance;
        }
    } // 싱글톤 구현

    Dictionary<string, List<GameObject>> objectPoolDic = new Dictionary<string, List<GameObject>>();

    public void CreateObjectPool (string poolingName, GameObject obj, int createCount)
    {
        List<GameObject> poolList = new List<GameObject>();
        for (int i = 0; i < createCount; i++)
        {
            GameObject clone = Instantiate(obj, this.transform); //현재 오브젝트의 클론 만들기
            clone.name = poolingName;
            poolList.Add(clone); // 생성된 클론들을 리스트에 삽입
        }
        objectPoolDic.Add(poolingName, poolList); // 지정한 수만큼 만들어진 클론의 리스트를 ID가 키값인 딕셔너리에 삽입 
    }

    public GameObject Pop (string poolingName)
    {
        if (objectPoolDic[poolingName].Count == 1)
        {
            GameObject clone = Instantiate(objectPoolDic[poolingName][0], this.transform);
            clone.name = poolingName;
            objectPoolDic[poolingName].Add(clone);
            Debug.LogError("Need More Object Pool :" + poolingName);
        } // 지정값 이상으로 오브젝트를 생성하는 경우 

        GameObject returnObj = objectPoolDic[poolingName][0]; // 딕셔너리에서 리턴할 오브젝트 가져오기
        objectPoolDic[poolingName].RemoveAt(0); //가져온 딕셔너리 첫번째 항목 삭제, 두번째 항목이 첫번째가 됨
        return returnObj;
    }

    public void Return (GameObject obj)
    {
        if (objectPoolDic.ContainsKey(obj.name))
        {
            objectPoolDic[obj.name].Add(obj);
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }


}
