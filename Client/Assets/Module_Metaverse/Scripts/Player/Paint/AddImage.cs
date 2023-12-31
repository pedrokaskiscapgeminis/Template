using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using Manager;
public class AddImage : MonoBehaviour
{

    private string _path;//File path
    private float _size = 0.5f;
    public SpriteRenderer spriteRenderer;
    public RPCManager rcpmanager;
    public PhotonManager photonmanager;

    /// <summary>
    /// Open panel to choose the file and limiting the extensions to choose
    /// </summary>

    private void Start()
    {
        photonmanager = PhotonManager.FindInstance();
    }
    public void OnClickOpenFile()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };

        WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false));

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /// <summary>
    /// Writes the path of the file selected
    /// </summary>
    /// <param name="paths"></param>
    [System.Obsolete]
    public void WriteResult(string[] paths)
    {
        if (paths.Length == 0)
        {
            return;
        }
        _path = string.Join("\n", paths);


        StartCoroutine(UpdateFile());
    }

    /// <summary>
    /// Transform the path to byte and call Api, eith the extenion and this bytes
    /// </summary>
    /// <returns></returns>
    [System.Obsolete]
    IEnumerator UpdateFile()
    {

        WWW www = new WWW("file://" + _path);
        yield return www.isDone;
        Debug.Log("www.data : " + www.data);
        byte[] bytes = www.bytes;

        Debug.Log(bytes.Length);


        //Determine file extension
        var fileExtension = _path.Split('.').Last().Trim();
        Debug.Log($"Extension: {fileExtension}");
        switch (fileExtension)
        {

            //Image
            case "jpg":
            case "png":
            case "jpeg":
                Debug.Log("Image");
                FileUpload(bytes);
                RPCManager.RPC_Images(photonmanager.Runner, bytes);
                break;
        }
    }
  
    public void FileUpload(byte[] bytes, string fileExtension = null)
    {
        //Create Texture
        Texture2D textu = new Texture2D(1, 1);

        //transform data into texture
        textu.LoadImage(bytes);

        //transform texture into Sprite

        var nuevoSprite = Sprite.Create(textu, new Rect(0.0f, 0.0f, textu.width, textu.height), new Vector2((float)_size, (float)(_size)));
        spriteRenderer.sprite = nuevoSprite;
        
    }
    public void FileUploadOthers(byte[] bytes)
    {
        //Create Texture
        Texture2D textu = new Texture2D(1, 1);

        //transform data into texture
        textu.LoadImage(bytes);

        //transform texture into Sprite

        var nuevoSprite = Sprite.Create(textu, new Rect(0.0f, 0.0f, textu.width, textu.height), new Vector2((float)_size, (float)(_size)));
        spriteRenderer.sprite = nuevoSprite;
    }
}

