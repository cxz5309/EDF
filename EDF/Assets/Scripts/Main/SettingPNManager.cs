using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SettingPNManager : MonoBehaviour
{
    public GameObject[] SettingPNObjects;
    public GameObject PopupPanel;

    public bool isTweening;
    public GameObject petPopup;
    public GameObject TouchChk;
    public UILabel petLabel;
    public UISprite petSprite;

    public UISlider MusicVolumeSlider;
    public UISlider EffectVolumeSlider;
    public UILabel setOKLabel;
    public GameObject MuteOn;
    public bool muteOn;
    public bool isLabelAni;

    private float tmpMusicVolume;
    private float tmpEffectVolume;
    private float lastMusicVolume;
    private float lastEffectVolume;

    public void SetPopupPanel()
    {
        if (!isTweening)
        {
            isTweening = true;
            PopupPanel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            PopupPanel.SetActive(true);
            PopupPanel.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack).OnComplete(()=>
            {
                isTweening = false;
            });
        }
    }

    public void OnCloseButtonClick()
    {
        AudioManager.instance.SetMusicVolume(lastMusicVolume);
        AudioManager.instance.SetEffectVolume(lastEffectVolume);
        for (int i = 0; i < 4; i++)
        {
            SettingPNObjects[i].SetActive(false);
            PopupPanel.SetActive(false);
        }
    }

#region 과금버튼
    public void OnBillingButtonClick()
    {
        SetPopupPanel();
        SettingPNObjects[0].SetActive(true);
    }

    public void OnBillingYesButtonClick()
    {
        string BlackCow = "흑우";

        DataSave.instance.dbData.SetHavePet(BlackCow, true);
        FirebaseParser.instance.SetHavePetDBtoFirebase(BlackCow);

        DataSave.instance.dbData.SetHaveAchieve(BlackCow, true);
        FirebaseParser.instance.SetHaveAchieveToFirebase(BlackCow);

        petSprite.spriteName = BlackCow;
        petLabel.text = BlackCow;
        PetPNManager.instance.InitPetInfomation();
        AchievePNManager.instance.InitAchievementPN();
        DoPetPopup();
    }
    
    public void DoPetPopup()
    {
        isTweening = true;
        petPopup.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        petPopup.SetActive(true);
        TouchChk.SetActive(true);
        petPopup.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isTweening = false;
        });
    }

    public void OnPetChkBlockClick()
    {
        if (!isTweening)
        {
            isTweening = true;
            petPopup.transform.DOScale(0f, 0.4f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                petPopup.SetActive(false);
                TouchChk.SetActive(false);
                isTweening = false;
            });
        }
    }
    #endregion
#region 세팅버튼
    public void OnSettingButtonClick()
    {
        SetPopupPanel();
        SettingPNObjects[1].SetActive(true);
        MusicVolumeSlider.value = AudioManager.instance.musicVolume;
        EffectVolumeSlider.value = AudioManager.instance.effectVolume;
        lastMusicVolume = AudioManager.instance.musicVolume;
        lastEffectVolume = AudioManager.instance.effectVolume;
    }

    public void OnMusicVolumeSliderChange()
    {
        tmpMusicVolume = MusicVolumeSlider.value;
        AudioManager.instance.SetOneVolume(AudioManager.instance.nowSound, tmpMusicVolume);
    }

    public void OnEffectVolumeSliderChange()
    {
        tmpEffectVolume = EffectVolumeSlider.value;
    }

    public void OnSetButtonClick()
    {
        if (isLabelAni == false)
        {
            lastMusicVolume = tmpMusicVolume;
            lastEffectVolume = tmpEffectVolume;
            AudioManager.instance.SetMusicVolume(tmpMusicVolume);
            AudioManager.instance.SetEffectVolume(tmpEffectVolume);
            DataSave.instance.dbData.SetNowMusicVolume(tmpMusicVolume);
            DataSave.instance.dbData.SetNowEffectVolume(tmpEffectVolume);
            FirebaseParser.instance.SetVolumeDBToFirebase();
            setOKLabel.gameObject.SetActive(true);
            StartCoroutine(coFadeOut());
        }
    }
    
    IEnumerator coFadeOut()
    {
        if (isLabelAni == false)
        {
            isLabelAni = true;
            Color tmpColor = setOKLabel.color;
            float alpha = 1;
            while (alpha > 0)
            {
                yield return new WaitForSeconds(0.05f);
                alpha -= 0.05f;
                tmpColor.a = alpha;
                setOKLabel.color = tmpColor;
            }
            isLabelAni = false;
            yield break;
        }
    }

    public void OnEffectMute()
    {
        if (!muteOn)
        {
            muteOn = true;
            MuteOn.SetActive(true);
            AudioManager.instance.SetEffectMute();
        }
        else
        {
            muteOn = false;
            MuteOn.SetActive(false);
            AudioManager.instance.SetEffectMute();
        }
    }

    public void OnGameCloseButtonClick()
    {
        Application.Quit();
    }
    #endregion
#region 개발자 버튼
    public void OnDeveloperButtonClick()
    {
        SetPopupPanel();
        SettingPNObjects[2].SetActive(true);
    }
    #endregion
#region 도움버튼
    public void OnHelpButtonClick()
    {
        SetPopupPanel();
        SettingPNObjects[3].SetActive(true);
    }
    
    public void OnOneNoteButtonClick()
    {
        AudioManager.instance.Stop("Stay Around");
        DataSave.instance.selectedData.SetSelectedData("허니", 3);
        SceneManager.LoadScene("GameScene");//게임 시작
    }
    #endregion
}
