using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using System.IO;
using System;


public class UserServerData : OnStateLoadedListener {
    byte[] playerRawData;
    public bool loginSuccess = false;



    public void save_state()
    {
        playerRawData = UserData.get_byte_data();
        ((PlayGamesPlatform)Social.Active).UpdateState(0, playerRawData, this);
    }

    public void OnStateSaved(bool success, int slot)
    {
        if (success)
        {
            //handle success
        }
        else
        {
            //handle fail
        }
    }

    public void load_state()
    {
        ((PlayGamesPlatform)Social.Active).LoadState(0, this);
    }


    public void OnStateLoaded(bool success, int slot, byte[] data)
    {
        if (success)
        {
            // do something with data[]
            playerRawData = data;
            UserData.initialize_player(playerRawData);
            UserData.switch_to_online_mode();
            loginSuccess = true;
        }
        else
        {
            // handle failure
            loginSuccess = false;
            
        }
    }

    public byte[] OnStateConflict(int slot, byte[] local, byte[] server)
    {
        // resolve conflict and return a byte[] representing the
        // resolved state.
        return local;
    }
}
