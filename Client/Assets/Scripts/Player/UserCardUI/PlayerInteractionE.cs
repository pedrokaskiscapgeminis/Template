using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using Newtonsoft.Json;
using UnityEngine.UI;

public class PlayerInteractionE : MonoBehaviour 
{
    GameManager gameManager;
    public UserUIInfo data;
    public GameObject card;
    public VisionData visionData;
    public Image imagen;
    public AchievementsManager achievementsManager;
    public AchivementList achivementList;
    CharacterInputHandler characterInputHandler;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameManager.FindInstance();
        
    }

    public void GetPublicDataFromOtherPlayer(string otherPlayerId, List<string> list)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = otherPlayerId,
            Keys = list 
        };

        PlayFabClientAPI.GetUserData(request, LoadDataIntoCard, OnUpdateUserDataFailure);
       
    }

    private void LoadDataIntoCard(GetUserDataResult result)
    {
        card.SetActive(true);
      
        characterInputHandler = GameManager.FindInstance().GetCurrentPlayer().gameObject.GetComponent<CharacterInputHandler>();
        characterInputHandler.DeactivateALL();

        data = JsonConvert.DeserializeObject<UserUIInfo>(result.Data["userUICard"].Value);
        visionData = card.GetComponent<VisionData>();

        visionData.UserNameTitle.text = data.name;
        visionData.TemasText.text = data.teams;
        visionData.OboutText.text = data.about;
        visionData.HobbiesText.text = data.hobbies;
        visionData.CVText.text = data.CV;

        //Logros

        if (result.Data != null && result.Data.ContainsKey("Achievements"))
        {
            achievementsManager.currentAchievements = JsonConvert.DeserializeObject<List<Achievement>>(result.Data["Achievements"].Value);
            Debug.Log("Achievements retrieved successfully!");
            achivementList.InstanceAchivementItem();
        }

        //Imagen
        //esto se puede reciclar usando el script ImagenData la funcion OnGetUserDataSuccess
        if (result.Data != null && result.Data.ContainsKey("CustomImage"))
        {
            string imageString = result.Data["CustomImage"].Value;
            byte[] imageData = System.Convert.FromBase64String(imageString);

            // Use the image data as desired (e.g., display in Unity, save to file, etc.)
            Debug.Log("Image retrieved successfully!");

            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            imagen.sprite = sprite;
        }
    }

    private void OnUpdateUserDataFailure(PlayFabError error)
    {
        Debug.LogError("Failed to upload image to user: " + error.GenerateErrorReport());
    }

}