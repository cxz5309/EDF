using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoddingManager : MonoBehaviour
{
    public static LoddingManager instance;

    public bool step1_DatasaveInit;
    public bool step2_Firebase;
    public bool step3_DataLoad;

    public bool isStepProcessing;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        step1_DatasaveInit = true;
        StartCoroutine(coLodingStep());
        StartCoroutine(coWaitForLoding());
    }

    IEnumerator coLodingStep()
    {
        while (true)
        {
            if (!isStepProcessing) {
                if (step1_DatasaveInit)
                {
                    Debug.Log("step1_DatasaveInit");
                    isStepProcessing = true;
                    DataSave.instance.InitAllDataNull();
                    step2_Firebase = true;
                    step1_DatasaveInit = false;
                    isStepProcessing = false;
                }
                if (step2_Firebase)
                {
                    Debug.Log("step2_Firebase");

                    isStepProcessing = true;
                    FirebaseParser.instance.InitFirebaseParser();
                    FirebaseParser.instance.GetFirebaseDB();
                }
                if (step3_DataLoad)
                {
                    Debug.Log("step3_DataLoad");
                    isStepProcessing = true;

                    AudioManager.instance.InitAudioManager();
                    step3_DataLoad = false;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator coWaitForLoding()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("MainScene");
        yield break;
    }

}
