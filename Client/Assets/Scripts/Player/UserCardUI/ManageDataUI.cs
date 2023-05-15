using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;



    public class UserUIInfo
    {
        public string name;
        public string teams;
        public string about;
        public string hobbies;
    public string CV;



        public UserUIInfo(string name, string teams, string about, string hobbies,string CV)
        {
            this.name = name;
            this.teams = teams;
            this.about = about;
            this.hobbies = hobbies;
            this.CV=CV;
        }
    }
    public class ManageDataUI
    {
    public UserUIInfo data;

        // Save User Data
        public void SaveData(UserUIInfo data)
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
            {
                {"userUICard", JsonConvert.SerializeObject(data)}
            }
            };
            PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
        }

        public void OnDataSend(UpdateUserDataResult obj)
        {
            Debug.Log("[PlayFab-ManageData] Data Sent");
        }


        //Load User Data
        public void LoadData()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnCharactersDataReceived, OnError);
        }

        void OnCharactersDataReceived(GetUserDataResult result)
        {
            Debug.Log("[PlayFab-ManageData] Received characters data!");
            if (result.Data != null && result.Data.ContainsKey("userUICard"))
            {
            Debug.Log(result.Data["userUICard"].Value);
                data = JsonConvert.DeserializeObject<UserUIInfo>(result.Data["userUICard"].Value);
            }

        }

        public void OnError(PlayFabError obj)
        {
            Debug.Log("[PlayFab-ManageData] Error");
        }
    }

