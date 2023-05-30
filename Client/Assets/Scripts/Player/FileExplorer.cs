using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using UnityEngine;
using SFB;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.Video;
using Fusion;

public class FileExplorer : NetworkBehaviour
{
    /*---------------------VARIABLES-------------------------*/
    public Presentation presentation;
    private Compressor compressor = new Compressor();
    private string _path;//File path
    public JObject json;
    UnityWebRequest GetRequest;
    public TMP_Text  loadingPressCanvas;//shows when presentation is loading
    public float size;
    object[] content;
    [Networked(OnChanged = nameof(OnPresentationChanged))]
    public NetworkString<_16> urls { get; set; }
    public GameManager gameManager;


#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);
#endif
    /*---------------------METHODS-------------------------*/
    // Start is called before the first frame update
    void Start()
    {
        presentation=GameObject.Find("Presentation").GetComponent<Presentation>();//Getting the press from scene
        gameManager = GameManager.FindInstance();
    }
    /// <summary>
    /// Open panel to choose the file and limiting the extensions to choose
    /// </summary>
    [Obsolete]
    public void OpenFile()
    {
        loadingPressCanvas.SetText("Loading");
        Screen.lockCursor = false;//Unity and standalone

        var extensions = new [] {new ExtensionFilter("Powerpoint Files", "pptx", "ppt"),new ExtensionFilter("Excel Files", "xlsx", "xlsm","xlsb"),new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),new ExtensionFilter("Video Files", "mp4","ogv","vp8","webm" ),new ExtensionFilter("All Files", "*" ),};
        
        #if UNITY_WEBGL && !UNITY_EDITOR
            UploadFile(gameObject.name, "OnFileUpload", ".pptx", false);
        #else
            WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false));
        #endif
        Screen.lockCursor = true;//Unity and standalone
    }

    // Called from browser
    public void OnFileUpload(string url) {
        //string to string[]
        List<string> list = new List<string>();
        list.Add(url);
        String[] urls = list.ToArray();
        //output
        WriteResult(urls);
    }

    [System.Obsolete]
    public void WriteResult(string[] paths) 
    {
        if (paths.Length == 0) 
        {
            return;
        }
        _path = "";
        foreach (var p in paths) 
        {
            _path += p + "\n";
            Debug.Log("WriteResultPath: " + _path);
        }

        StartCoroutine(UpdateFile());
    }
    /// <summary>
    /// Transform the path to byte and call Api, eith the extenion and this bytes
    /// </summary>
    /// <returns></returns>
    [System.Obsolete]
    IEnumerator UpdateFile()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
           UnityWebRequest www = UnityWebRequest.Get(_path);
            yield return www.SendWebRequest();
            Debug.Log(www.downloadHandler.text);
            byte[] bytes = www.downloadHandler.data;
        #else
            WWW www = new WWW("file://" + _path);
            yield return www.isDone;
            Debug.Log("www.data : " + www.data);
            byte[] bytes = www.bytes;
        #endif
        Debug.Log(bytes.Length);
#if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log("PresentationWebGL");
            StartCoroutine(PresentationUpload(bytes,"pptx",$"https://v2.convertapi.com/convert/{"pptx"}/to/png?Secret=vCDWYMLnubBCLVDo"));
#else
        //Determine file extension
        var fileExtension = _path.Split('.').Last().Trim();
        Debug.Log($"Extension: {fileExtension}");

        switch (fileExtension){

            //Image
            case "jpg":
            case "png":
            case "jpeg":
                Debug.Log("Image");
                ImageUpload(bytes);
                break;

            //Video
            case "mp4":
            case "ogv":
            case "vp8":
            case "webm":
                Debug.Log("Video");
                VideoUpload(bytes,fileExtension);
                break;

            //Excel
            case "xlsx":
            case "xlsm":
            case "xlsb":
                break;
            
            // Powerpoint
            case "pptx":
            case "ppt":
                Debug.Log("Presentation");
                StartCoroutine(PresentationUpload(bytes,fileExtension,$"https://v2.convertapi.com/convert/{fileExtension}/to/png?Secret=vCDWYMLnubBCLVDo"));
                break;
            default:
                Debug.Log("Format file not allowed");
                break;
        }   
        #endif         
    }

    //Upload the image file and share it with other users
    void ImageUpload(byte[] bytes){
        //We build the content item            
    
        //We send the content to the other users
        //RPC.

        SetImage(bytes);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    public void SetImage(byte[] bytes){
        //Clear videoScreen
        presentation.sprites.Clear();
        presentation.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        presentation.transform.GetChild(0).GetComponent<VideoPlayer>().url="";

        //Create Texture
        Texture2D textu = new Texture2D(1, 1);

        //transform data into texture
        textu.LoadImage(bytes);

        //transform texture into Sprite
        loadingPressCanvas.enabled = false;
        var nuevoSprite = Sprite.Create(textu, new Rect(0.0f, 0.0f, textu.width, textu.height), new Vector2((float) size, (float) (size - 0.07))); 
        presentation.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = nuevoSprite;
    }

    //Upload the image file and share it with other users
    void VideoUpload(byte[] bytes,string fileExtension){
        string filename = _path.Split("\\").Last().Split('.')[0].Trim();


        //Online Video, Will be implemented in the future.
        //We build the content item
        // byte[] Compressed = compressor.Compress(bytes);
        // object[] content = new object[] {filename, fileExtension,Compressed};
    
        // //We send the content to the other users
        // RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        // PhotonNetwork.RaiseEvent(24, content, raiseEventOptions, SendOptions.SendReliable);

        SetVideo(filename,fileExtension,bytes);
    }

    public void SetVideo(string filename, string fileExtension, byte[] bytes){
        
        //Prepare videoScreen
        presentation.sprites.Clear();
        presentation.transform.GetChild(0).GetComponent<VideoPlayer>().url="";
        presentation.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;

        //Create VideoFile
        Debug.Log(_path);
        string PostPath = Application.persistentDataPath + $"/{filename}.{fileExtension}";
        if (!System.IO.File.Exists(PostPath))
            System.IO.File.WriteAllBytes(PostPath, bytes);
        
        //Set VideoClip
        //loadingPressCanvas.enabled = false;
        presentation.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        VideoPlayer videoPlayer = presentation.transform.GetChild(0).GetComponent<VideoPlayer>();
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = PostPath;

    }
    /// <summary>
    /// Upload the file to the api, and obtend the results in JSON
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="fileExtension"></param>
    /// <param name="PostPath"></param>
    /// <returns></returns>
    //Upload the presentation file to the convert api
    IEnumerator PresentationUpload(byte[] bytes,string fileExtension, string PostPath){

        //Clear videoScreen and previous sprite
        presentation.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        presentation.transform.GetChild(0).GetComponent<VideoPlayer>().url="";
        presentation.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;

        //Convert data un Base64
        string s = Convert.ToBase64String(bytes);

        //JSON needed to make a post
        var file = new JObject
        {
            ["Name"] = $"file.{fileExtension}",
            ["Data"] = s
        };
    

        var parameters = new JArray
        {
        new JObject
            {
                ["Name"] = "file",
                ["FileValue"] = file
            },
        new JObject
            {
                ["Name"] = "StoreFile",
                ["Value"] = true
            }
        };

        var jsonObject = new JObject
        {
        ["Parameters"] = parameters
        };

        string jsonString = jsonObject.ToString();
        Debug.Log(jsonString);

        //POST to ConvertApi
        UnityWebRequest request = new UnityWebRequest(PostPath, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        
                    
        json = JObject.Parse(request.downloadHandler.text);
        Debug.Log("Json: " + json);
        //We build the content item
                        
      
        //We send the content to the other users
        GameManager.RPC_DownloadImages(gameManager.GetRunner(), request.downloadHandler.text);


        //Si es nula
        /*
        if (presentation.current >= 1) presentation.current=0;
        if(presentation.sprites != null) presentation.sprites.Clear();
        StartCoroutine(GetRequestFunc());
        */
    }

    public void StartCoroutine()
    {
        presentation.sprites.Clear();
        if (presentation.current >= 1) presentation.current = 0;
        if (presentation.sprites != null) presentation.sprites.Clear();
        StartCoroutine(GetRequestFunc());
    }

    /// <summary>
    /// function to make a get request, Get dta from Json with kay Files
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetRequestFunc()
    {
        foreach (var archiv in json["Files"]) 
        {
            Debug.Log(archiv["Url"].ToString());
            //Get Request
            UnityWebRequest GetRequest = UnityWebRequestTexture.GetTexture(archiv["Url"].ToString());
            yield return GetRequest.SendWebRequest();
            
                
                switch (GetRequest.result)
                {
                    //Error cases
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError( "Error: " + GetRequest.error);
                    break;
                    case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + GetRequest.error);
                    break;
                    //Success case
                    case UnityWebRequest.Result.Success:
                        //We create a texture2D with the response data
                        Texture2D textu = ((DownloadHandlerTexture)GetRequest.downloadHandler).texture;
                        var nuevoSprite = Sprite.Create(textu, new Rect(0.0f, 0.0f, textu.width, textu.height), new Vector2((float) size, (float) (size - 0.07))); 
                        presentation.sprites.Add(nuevoSprite);
                    break;
                }
            //yield return new WaitForSeconds((float)0.3);        
        }

        //function in presentation to check if the list is full
        presentation.OnDirect();     
    }

    public  void downloadImages(string file)
    {
        if(presentation.current >= 1) presentation.current=0;
        if(presentation.sprites != null) presentation.sprites.Clear();
        
        loadingPressCanvas.enabled = false;
        json = JObject.Parse(file);
        Debug.Log("Entro");
        StartCoroutine(GetRequestFunc());
    }

    static void OnPresentationChanged(Changed<FileExplorer> changed)
    {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.urls}");

        changed.Behaviour.OnPresentationChanged();
    }

    //Here we put the name to our local player, we don�t need to do more because our nerworked nickname is also setted so since is networked nickname will be load in every client 
    private void OnPresentationChanged()
    {
        Debug.Log($"Presentation changed uploading");

        //downloadImages(urls);
    }
}

