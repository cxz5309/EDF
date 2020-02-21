using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SettingPopup : MonoBehaviour
{
    public GameObject SettingPanel;
    public UISlider MusicVolumeSlider;
    public UISlider EffectVolumeSlider;
    public UILabel setOKLabel;

    private float tmpMusicVolume;
    private float tmpEffectVolume;
    private bool isLabelAni;

    public void OnSettingButtonClick()
    {
        SettingPanel.SetActive(true);
        AudioManager.instance.SetPause(AudioManager.instance.nowSound, true);
        MusicVolumeSlider.value = AudioManager.instance.musicVolume;
        EffectVolumeSlider.value = AudioManager.instance.effectVolume;
        Time.timeScale = 0;
    }

    public void OnCloseButtonClick()
    {
        SettingPanel.SetActive(false);
        AudioManager.instance.SetPause(AudioManager.instance.nowSound, false);
        Time.timeScale = 1;
    }

    public void OnRetryButtonClick()
    {
        GameObject[] allNotes = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] allEffects = GameObject.FindGameObjectsWithTag("Effect");
        GameObject JudgeAnim = GameObject.FindGameObjectWithTag("JudgeAnim");
        GameObject ComboAnim = GameObject.FindGameObjectWithTag("ComboAnim");

        foreach (GameObject a in allNotes)
        {
            a.SetActive(false);
            a.transform.position = new Vector3(-13, 0, 0);
            ObjectPoolContainer.Instance.Return(a);
        }
        foreach (GameObject a in allEffects)
        {
            a.SetActive(false);
            ObjectPoolContainer.Instance.Return(a);
        }
        if(JudgeAnim!=null) JudgeAnim.SetActive(false);
        if (ComboAnim != null) ComboAnim.SetActive(false);

        GameManager.instance.StopNoteCoroutine();
        NoteManager.instance.NoteEnd();
        AudioManager.instance.Stop(AudioManager.instance.nowSound);
        DataSave.instance.gameData.InitGameData();
        AudioManager.instance.InitAudioManager();
        ProcessManager.instance.InitProcess();
        GameManager.instance.FirstStart();
        SettingPanel.SetActive(false);
        Time.timeScale = 1;
        ProcessManager.instance.SetMusicProgress();
        ProcessManager.instance.StopProcessMusicProgress();
    }

    public void OnHomeButtonClick()
    {
        Time.timeScale = 1;
        AudioManager.instance.Stop(AudioManager.instance.nowSound);
        ProcessManager.instance.SetHPSlider();
        ProcessManager.instance.musicProgress = 0;
        ProcessManager.instance.SetMusicProgress();
        ProcessManager.instance.StopProcessMusicProgress();
        SceneManager.LoadScene("MainScene");
    }

    public void OnMusicVolumeSliderChange()
    {
        tmpMusicVolume = MusicVolumeSlider.value;
    }
    public void OnEffectVolumeSliderChange()
    {
        tmpEffectVolume = EffectVolumeSlider.value;
    }

    public void OnSetVolumeButtonClick()
    {
        AudioManager.instance.SetMusicVolume(tmpMusicVolume);
        AudioManager.instance.SetOneVolume(AudioManager.instance.nowSound, tmpMusicVolume);
        AudioManager.instance.SetEffectVolume(tmpEffectVolume);
        DataSave.instance.dbData.SetNowMusicVolume(tmpMusicVolume);
        DataSave.instance.dbData.SetNowEffectVolume(tmpEffectVolume);
        FirebaseParser.instance.SetVolumeDBToFirebase();
        setOKLabel.gameObject.SetActive(true);
        StartCoroutine(coFadeOut());
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
}
