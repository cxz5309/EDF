using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public UILabel MusicTitleText;
    public UILabel MusicArtistText;

    private GameObject Pet = null;

    private void Awake()
    {
        instance = this;
    }

    public void InitStage(Transform player)
    {
        CreateTitleText();
        PetCreate(player);
    }
    
    public void CreateTitleText()
    {
        MusicTitleText.text = DataSave.instance.parsedData.musicTitle;
        MusicArtistText.text = DataSave.instance.parsedData.musicArtist;
    }

    public void PetCreate(Transform player)
    {
        Pet = Resources.Load("Prefabs/PetPrefabs/" + DataSave.instance.dbData.nowPet) as GameObject;
        ControlManager.instance.pet = Instantiate(Pet, player);
        ControlManager.instance.pet.transform.localPosition = new Vector3(4, 13, 0);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
