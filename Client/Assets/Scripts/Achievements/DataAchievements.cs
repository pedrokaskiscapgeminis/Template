using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;



public class DataAchievements : MonoBehaviour
{
    public Dictionary<string, string> inputdata;
    public ManageDataUI manageDataUI;
    public GameObject UICard;
    public VisionData visionData;
    public EditVisionData editVisionData;
    public UserUIInfo currentString;
    GameManager gameManager;
    CharacterInputHandler characterInputHandler;



    // Start is called before the first frame update

    public IEnumerator LoadInitialData()
    {
        // Do some work here...
        yield return null; // Wait one frame



        // Do some more work here...
        yield return new WaitForSeconds(1); // Wait for 1 seconds



        inputdata = new Dictionary<string, string>();
        if (manageDataUI.data == null)
        {
            manageDataUI.data = new UserUIInfo(gameManager.GetUsername(), gameManager.GetEmail(), "Por defecto", "Por defecto", "Por defecto");
            manageDataUI.SaveData(manageDataUI.data);
        }

        editVisionData.UserNameTitle.text = gameManager.GetUsername();
        editVisionData.TemasText.text = gameManager.GetEmail();



        visionData.UserNameTitle.text = gameManager.GetUsername();
        visionData.TemasText.text = gameManager.GetEmail(); ;
        visionData.OboutText.text = manageDataUI.data.about;
        visionData.HobbiesText.text = manageDataUI.data.hobbies;
        visionData.CVText.text = manageDataUI.data.CV;
        currentString = manageDataUI.data;
        // Function is finished
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.FindInstance();

        characterInputHandler = GameManager.FindInstance().GetCurrentPlayer().gameObject.GetComponent<CharacterInputHandler>();
        manageDataUI = new ManageDataUI();
        manageDataUI.LoadData();
        StartCoroutine(LoadInitialData());



        UICard.SetActive(true);
        characterInputHandler.DeactivateALL();

    }

}