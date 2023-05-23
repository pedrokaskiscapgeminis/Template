using RockVR.Video;
using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRecordPath : MonoBehaviour, IMetaEvent
{

    GameObject _eventObject;
    public GameObject eventObject { get => _eventObject; set => _eventObject = value; }

    [SerializeField] private VideoCapture videoCapture;

    string _path;
    public void activate(bool host)
    {
        OpenFile();

        
    }


    public void OpenFile()
    {
        Cursor.lockState = CursorLockMode.None;//Unity and standalone

        //Open panel to choose the file and limiting the extensions to choose
        var extensions = new[] { new ExtensionFilter() };


        WriteResult(StandaloneFileBrowser.OpenFolderPanel("Custom directory", "",false));
  

        Cursor.lockState = CursorLockMode.Locked;//Unity and standalone
    }

    public void WriteResult(string[] paths)
    {
        if (paths.Length == 0)
        {
            return;
        }
        Debug.Log("CustomPath: " + paths[0]);
        _path = paths[0];

        videoCapture.customPath = true;
        videoCapture.customPathFolder = _path;




    }


}
