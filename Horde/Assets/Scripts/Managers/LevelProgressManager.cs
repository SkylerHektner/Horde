using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelProgressManager : MonoBehaviour {

    public static LevelProgressManager Instance;

    private Dictionary<string, bool> levelProgress;
    private string filePath;

    private void Start()
    {
        // set instance, create the filepath, and attempt to load the JSON or create an empty dict if not found
        Instance = this;
        filePath = Application.persistentDataPath + "/LevelProgess.json";
        if (File.Exists(filePath))
        {
            string JSON = File.ReadAllText(filePath);
            levelProgress = JsonUtility.FromJson<Dictionary<string, bool>>(JSON);
        }
        else
        {
            levelProgress = new Dictionary<string, bool>();
        }
    }

    /// <summary>
    /// Use this method to save the completion status of your level.
    /// </summary>
    /// <param name="levelID"></param>
    /// <param name="completed"></param>
    public void ChangeLevelCompletionStatus(string levelID, bool completed)
    {
        if (levelProgress.ContainsKey(levelID))
        {
            levelProgress[levelID] = completed;
        }
        else
        {
            levelProgress.Add(levelID, completed);
        }
        saveProgress();
    }

    /// <summary>
    /// Use this method to get the level completion status of a given level.
    /// Returns false if there is no entry about the given level.
    /// </summary>
    /// <param name="levelID"></param>
    /// <returns></returns>
    public bool GetLevelCompletionStatus(string levelID)
    {
        if (levelProgress.ContainsKey(levelID))
        {
            return levelProgress[levelID];
        }
        else
        {
            return false;
        }
    }

    private void saveProgress()
    {
        string JSON = JsonUtility.ToJson(levelProgress);
        File.WriteAllText(filePath, JSON);
    }

    [ContextMenu("ClearLevelProgress")]
    private void ClearLevelProgress()
    {
        levelProgress = new Dictionary<string, bool>();
        saveProgress();
    }

    //Just a small method to test the class
    [ContextMenu("test")]
    private void Test()
    {
        ChangeLevelCompletionStatus("Test1", true);
        ChangeLevelCompletionStatus("Test2", false);
        ChangeLevelCompletionStatus("Test1", false);
        ChangeLevelCompletionStatus("Test2", true);

        Debug.Log(GetLevelCompletionStatus("Test1")); // should print false
        Debug.Log(GetLevelCompletionStatus("Test2")); // should print true
    }
}
