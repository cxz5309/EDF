using UnityEngine;

public class MakeData : MonoBehaviour
{
    public static MakeData instance;

    public string title { get; set; }
    public string level { get; set; }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
