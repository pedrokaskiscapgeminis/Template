using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;

using UnityEngine;



public class UserListitem : MonoBehaviour
{
    public int numActor;



   /* public void KickPlayer(string name)
    {
        int[] myNum = { numActor };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others, TargetActors = myNum };
        PhotonNetwork.RaiseEvent(26, "", raiseEventOptions, SendOptions.SendUnreliable);



        Destroy(this.gameObject);
    }*/
}