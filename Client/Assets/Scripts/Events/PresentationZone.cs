using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PresentationZone : MonoBehaviour
{
    //Camera of the presentation
    public Camera presentationCamera;

    
    //Detect if it in collider
    private void OnTriggerEnter(Collider other) {
       
        if(other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
         
            SC_FPSController player = other.gameObject.GetComponent<SC_FPSController>();
       
            PlayerSpawn playerSpawn = player.playerSpawner;
     
            playerSpawn.setPresentationCamera(presentationCamera);

            MusicManager musicController = GameObject.Find("MusicManager").GetComponent<MusicManager>();
            musicController.ChangeAudioState();
        
            
        }
    }
    //Detect if Exits collider
    private void OnTriggerExit(Collider other) {
     

         if(other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            SC_FPSController player = other.gameObject.GetComponent<SC_FPSController>();
           
            PlayerSpawn playerSpawn =  player.playerSpawner;
            playerSpawn.setPresentationCamera(null);
        
            MusicManager musicController = GameObject.Find("MusicManager").GetComponent<MusicManager>();
            musicController.ChangeAudioState();
            
        }
        
    }
}
