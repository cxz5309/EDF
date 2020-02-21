using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MakeStartScene : MonoBehaviour
{
    public void OnButtonClick()
    {
        MakeData.instance.title = GameObject.Find("title").GetComponent<InputField>().text;
        MakeData.instance.level = GameObject.Find("level").GetComponent<InputField>().text;
        SceneManager.LoadScene("BeatMakeScene");
    }
}
