using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public Color TeamColor; // new variable declared (pass the color that the user selects between scenes)

    //This code enables you to access the MainManager object from any other script.  
    private void Awake()
    {
        //Start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }//End of new code 

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadColor();
    }


    //This line is required for JsonUtility, as you just learned — it will only transform things to JSON if they are tagged as Serializable.
    [System.Serializable]

    //SaveData is a simple class, which only contains the color that the user selects.
    /*Obs:Why are you creating a class and not giving the MainManager instance directly to the JsonUtility?
     * Well, most of the time you won’t save everything inside your classes. 
     * It’s good practice and more efficient to use a small class that only contains the specific data that you want to save.
     */
    class SaveData
    {
        public Color TeamColor;

    }


    public void SaveColor()
    {
        //you created a new instance of the save data and filled its team color class member with the TeamColor variable saved in the MainManager
        SaveData data = new SaveData();
        data.TeamColor = TeamColor;

        //transformed that instance to JSON with JsonUtility.ToJson: 
        string json = JsonUtility.ToJson(data);

        //you used the special method File.WriteAllText to write a string to a file
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

        /*OBS: The first parameter is the path to the file.
         * You’ve used a Unity method called Application.persistentDataPath that will give you a folder where you can save data that will survive between application
         * reinstall or update and append to it the filename savefile.json.
         * ---------------
         * The second parameter is the text you want to write in that file — in this case, your JSON!
         */
    }


    //This method is a reversal of the SaveColor method: 
    public void LoadColor()
    {
        /*It uses the method File.Exists to check if a .json file exists.
        *If it doesn’t, then nothing has been saved, so no further action is needed.
        *If the file does exist, then the method will read its content with File.ReadAllText:
        */
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            //It will then give the resulting text to JsonUtility.FromJson to transform it back into a SaveData instance: 
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            //Finally, it will set the TeamColor to the color saved in that SaveData:
            TeamColor = data.TeamColor;
        }

    }

}

