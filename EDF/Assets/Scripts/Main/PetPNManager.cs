using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PetPNManager : MonoBehaviour
{
    public static PetPNManager instance;

    public string nowPetName;
    public string[] allPetNames = new string[9];
    public string[] allPetInfos = new string[9];

    public UISprite sprite;
    public UILabel petNameText;
    public UILabel petInfoText;
    public Dictionary<string, string> petDic;
    public UISprite[] petListSprites = new UISprite[9];

    public UILabel isPetUseLabel;
    bool isLabelAni;

    public int a = 0, b = 1, c = 2, d = 3, e = 4, f = 5, g = 6, h = 7, i = 8;//ngui가 오브젝트로밖에 변수입력을 안받아서 넣어놈

    private void Awake()
    {
        instance = this;
    }
    
    public void InitString()
    {
        allPetNames[0] = "충신";
        allPetInfos[0] = "사악한 간신들의 아첨에도 굴하지 않고 \n항상 연두에게 충언을 아끼지 않는 빛의 트수";
        allPetNames[1] = "소닉";
        allPetInfos[1] = "방사능 고슴도치. \n어떤 쓰레기게임에 출연한 이후 사망하였다.";
        allPetNames[2] = "둠의영혼";
        allPetInfos[2] = "연두의 몸 속에 들어있는 또 하나의 자아. \n2018년 크리스마스 경 둠이라는 게임을 하다가 빙의되었다.";
        allPetNames[3] = "까마귀";
        allPetInfos[3] = "연두가 기르고 있는 검은 새.\n가끔 연두가 방송이 힘들때 목소리 더빙으로 출연한다.";
        allPetNames[4] = "치코리타";
        allPetInfos[4] = "그게..누구더라..?";
        allPetNames[5] = "6V엘풍";
        allPetInfos[5] = "6V라는매우 희귀하고 귀중한 능력치의 엘풍. \n그의 활약이 기대된다.";
        allPetNames[6] = "팅글";
        allPetInfos[6] = "젤다의 전설에 등장하는 연두와 꼭 닮은 요정";
        allPetNames[7] = "대적자";
        allPetInfos[7] = "연두의 대적자이다. \n현재는 악당 연두가 너무 강해진 나머지 구글 \n검색 1페이지를 빼앗기고 말았다.";
        allPetNames[8] = "흑우";
        allPetInfos[8] = "연두.";
    }

    public void InitPetInfomation()
    {
        InitString();
        
        petDic = new Dictionary<string, string>();
        for (int i = 0; i < allPetNames.Length; i++)
        {
            petDic.Add(allPetNames[i], allPetInfos[i]);
            if((bool)DataSave.instance.dbData.havePet[allPetNames[i]])
            {
                petListSprites[i].spriteName = allPetNames[i];
                petListSprites[i].color = Color.white;
            }
        }
        nowPetName = DataSave.instance.dbData.nowPet;
        sprite.spriteName = nowPetName;
        petNameText.text = nowPetName;
        petInfoText.text = petDic[nowPetName];
    }

    public void OnPetSelButtonClick(int _petNum)
    {
        if ((bool)DataSave.instance.dbData.havePet[allPetNames[_petNum]])
        {
            nowPetName = allPetNames[_petNum];
            sprite.spriteName = allPetNames[_petNum];
            petNameText.text = allPetNames[_petNum];
            petInfoText.text = petDic[allPetNames[_petNum]];
        }
    }

    public void OnPetUseButtonClick()
    {
        if (isLabelAni == false)
        {
            isPetUseLabel.gameObject.SetActive(true);

            StartCoroutine(coFadeOut());
            DataSave.instance.dbData.SetNowPet(nowPetName);
            FirebaseParser.instance.SetNowPetDBtoFirebase();
        }
    }

    IEnumerator coFadeOut()
    {
        if(isLabelAni == false){
            isLabelAni = true;
            Color tmpColor = isPetUseLabel.color;
            float alpha = 1;
            while (alpha > 0)
            {
                yield return new WaitForSeconds(0.05f);
                alpha -= 0.05f;
                tmpColor.a = alpha;
                isPetUseLabel.color = tmpColor;
            }
            isLabelAni = false;
            yield break;
        }
    }
}
