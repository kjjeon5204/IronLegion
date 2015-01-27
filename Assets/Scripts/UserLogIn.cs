using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.IO;




public class MySaveLoadClass : OnStateLoadedListener
{
    public bool saveSuccess;

    public void SaveState()
    {
        ((PlayGamesPlatform)Social.Active).UpdateState(0,
                        UserData.get_byte_data(), this);
    }

    public void OnStateSaved(bool success, int slot)
    {
        //handle success or fail
        saveSuccess = true;
    }

    public void LoadState()
    {
        ((PlayGamesPlatform)Social.Active).LoadState(0, this);
    }


    public bool loadSuccess = false;
    public byte[] playerData;
    public void OnStateLoaded(bool success, int slot, byte[] data)
    {
        if (success)
        {
            loadSuccess = true;
            playerData = data;
        }
    }

    public bool conflictToResolve = false;
    public byte[] OnStateConflict(int slot, byte[] local, byte[] server)
    {
        conflictToResolve = true;
        return server;
    }
}

public class UserLogIn : MonoBehaviour {
    public DownloadObbExample obbDownload;
    UserServerData curUserServerData;
    public GameObject offlineDialog;
    byte[] localData;
    public PlayerMechData playerMechData;
    MySaveLoadClass saveLoadHandlers;

    bool playerLoginSuccess;

    void read_player_local_data()
    {
        string dataPath = Application.persistentDataPath + "/loginData";
        if (File.Exists(dataPath))
        {
            
            using (StreamReader inputFile = File.OpenText(dataPath))
            {
                string tempLoginStatus = inputFile.ReadLine();
                if (tempLoginStatus == "GOOGLEPLUS")
                {
                    UserData.userDataContainer.loginStatus = UserDataContainer.PlayerLoginStatus.GOOGLEPLUS;
                }
                else
                {
                    UserData.userDataContainer.loginStatus = UserDataContainer.PlayerLoginStatus.GUEST;
                }
            }
            playerLoginSuccess = true;
            loginStatus = LoginStatus.CONT_LOGIN_STATE;
        }
        else
        {
            playerLoginSuccess = false;
            loginStatus = LoginStatus.FIRSTLOGIN_WAIT_FOR_INPUT;

        }
    }

    bool saveToServerFailed = false;
    bool loadDataFromServer = true;
    bool loginProcessEnded = false;
    public GameObject loadPlayerDataFailed;
    
    void player_login_sequence()
    {
        startLoginSequence = true;
        statusText.text = "Logging in...";
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                loginProcessEnded = true;
                statusText.text = "Login success!";
            }
            else
            {
                loginProcessEnded = true;
                statusText.text = "Login failed!";
            }
        });
	}


    //Google cloud save/load
    enum LoadingSequenceStatus
    {
        CHECKING_PLAYER_SAVED_DATA,
        SAVE_META_DATA_RETRIEVED,
        OPENING_META_DATA,
        CREATING_NEW_PLAYER,
        SAVING_NEW_PLAYER,
        LOADING_COMPLETE,
        READING_COMPLETE,
        ERROR
    }
    LoadingSequenceStatus curLoadStatus;
    bool loadingProcessActive = false;
    bool loadingFinished = false;
    ISavedGameMetadata tempPlayerMetaData;
    public GameObject loadingCircle;

    //Google cloud save function implementation
    IEnumerator player_load_sequence()
    {
        loadingProcessActive = true; //start loading process
        ISavedGameClient saveGameClientMethod = ((PlayGamesPlatform)Social.Active).SavedGame;
        //Check for all existing load data
        statusText.text = "Retrieving Player Save Data";
        curLoadStatus = LoadingSequenceStatus.CHECKING_PLAYER_SAVED_DATA;
        retrieve_load_data();

        //Wait until data retrieval process completes
        while (curLoadStatus == LoadingSequenceStatus.CHECKING_PLAYER_SAVED_DATA)
        {
            statusText.text = "Retrieving Player Save Data";
            yield return null;
        }

        //Load player data. If player data does not exist, create new player data
        if (curLoadStatus == LoadingSequenceStatus.CREATING_NEW_PLAYER)
        {
            creating_new_player();
            //Create new meta data
            saveGameClientMethod.OpenWithAutomaticConflictResolution("MainUserData",
                DataSource.ReadNetworkOnly, ConflictResolutionStrategy.UseOriginal,
                create_new_meta_data);

            //loop to wait until previous callback method finishes
            while (curLoadStatus == LoadingSequenceStatus.CREATING_NEW_PLAYER)
            {
                statusText.text = "Creating new player data";
                yield return null;
            }

            //Save new player data
            if (curLoadStatus == LoadingSequenceStatus.SAVING_NEW_PLAYER)
            {
                SavedGameMetadataUpdate.Builder metaDataBuilder = new SavedGameMetadataUpdate.Builder();
                metaDataBuilder = metaDataBuilder.WithUpdatedPlayedTime(System.TimeSpan.Zero)
                    .WithUpdatedDescription("Player main save file");

                SavedGameMetadataUpdate updatedMetaData = metaDataBuilder.Build();

                if (tempPlayerMetaData.IsOpen)
                {
                    int tempNum = 5;
                    byte[] temporarySaveData = new byte[3];
                    
                    temporarySaveData[0] = System.Convert.ToByte(tempNum);
                    temporarySaveData[1] = System.Convert.ToByte(tempNum);
                    temporarySaveData[2] = System.Convert.ToByte(tempNum);
                    statusText.text = "Never don't fuck google!";
                    byte[] tempSaveState = UserData.userDataContainer.encode_data_to_byte();
                    statusText.text = "Fuck you Google!";
                    statusText.text = tempSaveState.Length.ToString();
                    saveGameClientMethod.CommitUpdate(tempPlayerMetaData, updatedMetaData, 
                        tempSaveState, new_player_save_game_completed);



                    while (curLoadStatus == LoadingSequenceStatus.SAVING_NEW_PLAYER)
                    {
                        loadingCircle.transform.Rotate(Vector3.forward * 45.0f * Time.deltaTime);
                        yield return null;
                    }
                }
            }
        }
        else if (curLoadStatus == LoadingSequenceStatus.OPENING_META_DATA)
        {
            saveGameClientMethod.OpenWithAutomaticConflictResolution(tempPlayerMetaData.Filename,
                DataSource.ReadNetworkOnly, ConflictResolutionStrategy.UseOriginal, load_user_data);

            while (curLoadStatus == LoadingSequenceStatus.OPENING_META_DATA)
            {
                yield return null;
            }
            
            saveGameClientMethod.ReadBinaryData(tempPlayerMetaData, initialize_load_data);

            while (curLoadStatus == LoadingSequenceStatus.LOADING_COMPLETE)
            {
                yield return null;
            }


        }
        loadingProcessActive = false; //end loading process
        loadingFinished = true;
    }

    public void initialize_load_data(SavedGameRequestStatus readStatus, byte[] binarySaveData)
    {
        if (readStatus == SavedGameRequestStatus.Success)
        {
            curLoadStatus = LoadingSequenceStatus.READING_COMPLETE;
            statusText.text = "Reading player data complete";
            UserData.userDataContainer.decode_byte_to_data(binarySaveData);
        }
        else
        {
            statusText.text = "Error reading player data";
        }
    }

    public void load_user_data(SavedGameRequestStatus loadStatus, ISavedGameMetadata game) 
    {
        if (loadStatus == SavedGameRequestStatus.Success)
        {
            tempPlayerMetaData = game;
            curLoadStatus = LoadingSequenceStatus.LOADING_COMPLETE;
            statusText.text = "Loading complete";
        }
        else
        {
            statusText.text = "Error code 1003";
        }
    }

    //Creates new user data when the player logs on for the first time
    public void create_new_meta_data(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            tempPlayerMetaData = game;
            curLoadStatus = LoadingSequenceStatus.SAVING_NEW_PLAYER;
            statusText.text = "Creating player data...";
        }
        else
        {
            statusText.text = "Failed to Create Player Data! /n (Error code:1001)";
            curLoadStatus = LoadingSequenceStatus.ERROR;
        }
    }

    //Completes the save data process.
    public void new_player_save_game_completed(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            curLoadStatus = LoadingSequenceStatus.LOADING_COMPLETE;
            statusText.text = "Creating Player Data Complete! /n Welcome to Iron Legions!";
        }
        else
        {
            statusText.text = "Failed to Create Player Data! /n (Error code: 1002)";
        }
    }

    void retrieve_load_data()
    {
        ISavedGameClient saveGameMethod = ((PlayGamesPlatform)Social.Active).SavedGame;
        saveGameMethod.FetchAllSavedGames(DataSource.ReadNetworkOnly, initiate_load_sequence);

    }

    void initiate_load_sequence(SavedGameRequestStatus status, List<ISavedGameMetadata> savedGameList)
    {
        if (status == SavedGameRequestStatus.Success && savedGameList.Count == 0)
        {
            //no saved game detected!
            //initiate new user data creation
            curLoadStatus = LoadingSequenceStatus.CREATING_NEW_PLAYER;
            statusText.text = "No existing data. Create new user";

        }
        else if (status == SavedGameRequestStatus.Success)
        {
            //initiate opening player saved game meta data
            curLoadStatus = LoadingSequenceStatus.OPENING_META_DATA;
            tempPlayerMetaData = savedGameList[0]; //Store to temp meta data for use on callback
            statusText.text = "Reading player data...";
        }
        else 
        {
            statusText.text = "Error while retrieving data! /n (Error code: 1000)";
        }
    }

    public void saved_game_opened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            statusText.text = "Player Data Loaded!";
            //read player data
        }
        else
        {
            if (status == SavedGameRequestStatus.BadInputError)
            {
                statusText.text = "Creating New Player Data";
            }
            if (status == SavedGameRequestStatus.TimeoutError)
            {
                statusText.text = "Connection timed out!";
            }
        }
    }

    void cloud_save_player_data()
    {

    }

    void creating_new_player()
    {
        
        PlayerMechData starterMech = new PlayerMechData();
        starterMech.mechID = "heroMech";
        starterMech.level = 1;
        starterMech.curExp = 0;
        starterMech.health = 0;
        starterMech.damage = 0;
        starterMech.armor = 0;
        starterMech.pentration = 0;
        starterMech.luck = 0;
        starterMech.energy = 0;
        starterMech.itemsEquipped = new List<string>();
        starterMech.itemsEquipped.Add("000000");
        starterMech.itemsEquipped.Add("000000");
        starterMech.itemsEquipped.Add("000000");
        starterMech.itemsEquipped.Add("000000");
        starterMech.itemsEquipped.Add("000000");
        starterMech.equippedSkill = new string[8];
        for (int ctr = 0; ctr < 8; ctr++)
        {
            starterMech.equippedSkill[ctr] = "000000";
        }

        UserData.create_new_user_data("Test Account",
            starterMech);
    }

    void save_to_google_save(ISavedGameMetadata playerMetaData) 
    {
        ISavedGameClient savedGameClient = ((PlayGamesPlatform)Social.Active).SavedGame;

        //SavedGameMetadataUpdate.Builder saveGameBuilder = new SavedGameMetadataUpdate.Builder();
        
    }

	// Use this for initialization
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        UserData.initialize_user_data();
        creating_new_player();
        UserData.userDataContainer.encode_data_to_byte();
        statusText.text = "Waiting for user input";
    }

    public enum LoginStatus
    {
        CONT_LOGIN_STATE,
        FIRSTLOGIN_WAIT_FOR_INPUT
    }
    LoginStatus loginStatus;

    public void login_method_select(UserDataContainer.PlayerLoginStatus loginMethod)
    {
        if (loginMethod == UserDataContainer.PlayerLoginStatus.GOOGLEPLUS)
        {
            UserData.userDataContainer.loginStatus = UserDataContainer.PlayerLoginStatus.GOOGLEPLUS;
        }
        else if (loginMethod == UserDataContainer.PlayerLoginStatus.GUEST)
        {
            UserData.userDataContainer.loginStatus = UserDataContainer.PlayerLoginStatus.GUEST;
        }
    }

    public TextMesh statusText;
    bool startLoginSequence = false;
    bool loadingInitiated = false;

	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            if (!Social.localUser.authenticated)
            {
                player_login_sequence();
            }
            if (Social.localUser.authenticated
                && loadingInitiated == false)
            {
                statusText.text = "Loading start!";
                //initiate loading
                StartCoroutine(player_load_sequence());
                loadingInitiated = true;
            }
        }
        if (Social.localUser.authenticated == true && loadingInitiated == false)
        {
            statusText.text = "Tap screen to continue";
        }
	}
}
