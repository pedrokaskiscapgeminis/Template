using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UiUsers : MonoBehaviour ,IMetaEvent
{
   
    public ManageDataUI manageDataUI;
    public AchievementsManager achievementsManager;
    public GameObject UICard;
    public VisionData visionData;
    public EditVisionData editVisionData;
    public UserUIInfo currentString;
   
    
    GameManager gameManager;
    CharacterInputHandler characterInputHandler;

    GameObject _eventObject;
    GameObject IMetaEvent.eventObject { get => _eventObject; set => _eventObject = value; }
    // Start is called before the first frame update

    public IEnumerator LoadInitialData()
    {
        // Do some work here...
        yield return null; // Wait one frame

        // Do some more work here...
        yield return new WaitForSeconds(1); // Wait for 1 seconds

     
        if (manageDataUI.data == null)
        {
            manageDataUI.data = new UserUIInfo(gameManager.GetUsername(), gameManager.GetEmail(), "Por defecto", "Por defecto", "Por defecto");
           
            manageDataUI.SaveData(manageDataUI.data);
        }
      
        editVisionData.UserNameTitle.text = gameManager.GetUsername();
        editVisionData.TemasText.text= gameManager.GetEmail();

  

        visionData.UserNameTitle.text = gameManager.GetUsername();
        visionData.TemasText.text = gameManager.GetEmail(); ;
        visionData.OboutText.text = manageDataUI.data.about;
        visionData.HobbiesText.text = manageDataUI.data.hobbies;
        visionData.CVText.text = manageDataUI.data.CV;
        currentString = manageDataUI.data;
      
        // Function is finished
    }
    // Start is called before the first frame update
    public void activate(bool host)
    {
        ///////////////////////
        ///
      
        achievementsManager.currentAchievements = new List<Achievement> { new Achievement("Interactuado por primera vez", true )};

        achievementsManager.SaveData(achievementsManager.currentAchievements);

        /////////////////////////////////
        ///
        characterInputHandler = GameManager.FindInstance().GetCurrentPlayer().gameObject.GetComponent<CharacterInputHandler>();
        manageDataUI = new ManageDataUI();
        manageDataUI.LoadData();
        StartCoroutine(LoadInitialData());

        

        UICard.SetActive(true);
        characterInputHandler.DeactivateALL();


    }
    void Start()
    {
        gameManager = GameManager.FindInstance();
        achievementsManager.LoadData();

    }
    // Update is called once per frame
    void Update()
    {
    }
    public void EditUserData()
    {
        manageDataUI.data = new UserUIInfo(gameManager.GetUsername(), gameManager.GetEmail(), editVisionData.OboutTextInput.text, editVisionData.HobbiesTextInput.text, editVisionData.CVTextInput.text);
        manageDataUI.SaveData(manageDataUI.data);
    }
}
