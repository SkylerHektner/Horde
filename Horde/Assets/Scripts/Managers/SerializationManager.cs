using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SerializationManager : MonoBehaviour {

    public static SerializationManager Instance;

    private SerializableDictionary<string, bool> levelProgress;
    private SerializableDictionary<int, bool> heuristicsProgress;
    private SerializableDictionary<string, bool> baseUnitProgress;
    private string levelProgressFilePath;
    private string heuristicsProgressFilePath;
    private string baseUnitProgressFilePath;

    private void Start()
    {
        // set instance, create the filepath, and attempt to load the JSON or create an empty dict if not found
        Instance = this;
        levelProgressFilePath = Application.persistentDataPath + "/LevelProgress.json";
        heuristicsProgressFilePath = Application.persistentDataPath + "/HeuristicsProgress.json";
        baseUnitProgressFilePath = Application.persistentDataPath + "/BaseUnitProgress.json";
        // check for file for level progress
        if (File.Exists(levelProgressFilePath))
        {
            string JSON = File.ReadAllText(levelProgressFilePath);
            levelProgress = JsonUtility.FromJson<SerializableDictionary<string, bool>>(JSON);
        }
        else
        {
            levelProgress = new SerializableDictionary<string, bool>();
        }
        // check for file for heuristics progress
        if (File.Exists(heuristicsProgressFilePath))
        {
            string JSON = File.ReadAllText(heuristicsProgressFilePath);
            heuristicsProgress = JsonUtility.FromJson<SerializableDictionary<int, bool>>(JSON);
        }
        else
        {
            heuristicsProgress = new SerializableDictionary<int, bool>();
        }
        // check for file for baseUnitProgress
        if (File.Exists(baseUnitProgressFilePath))
        {
            string JSON = File.ReadAllText(baseUnitProgressFilePath);
            baseUnitProgress = JsonUtility.FromJson<SerializableDictionary<string, bool>>(JSON);
        }
        else
        {
            baseUnitProgress = new SerializableDictionary<string, bool>();
        }
    }

    /// <summary>
    /// Use this method to save the completion status of a level.
    /// Automatically serializes changed when called
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
        saveLevelProgress();
    }

    /// <summary>
    /// Use this method to save the unlock status of a heuristic.
    /// Automatically serializes changed when called
    /// </summary>
    /// <param name="levelID"></param>
    /// <param name="completed"></param>
    public void ChangeHeuristicUnlockStatus(HInterface.HType heuristic, bool unlocked)
    {
        if (heuristicsProgress.ContainsKey((int)heuristic))
        {
            heuristicsProgress[(int)heuristic] = unlocked;
        }
        else
        {
            heuristicsProgress.Add((int)heuristic, unlocked);
        }
        saveHeuristicProgress();
    }

    /// <summary>
    /// Use this method to save the unlock status of a unit.
    /// Automatically serializes changed when called
    /// </summary>
    /// <param name="levelID"></param>
    /// <param name="completed"></param>
    public void ChangeUnitUnlockStatus(string unitID, bool completed)
    {
        if (baseUnitProgress.ContainsKey(unitID))
        {
            baseUnitProgress[unitID] = completed;
        }
        else
        {
            baseUnitProgress.Add(unitID, completed);
        }
        saveUnitProgress();
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

    /// <summary>
    /// Use this method to get the unlock status of a given heuristic.
    /// Returns false if there is no entry about the given heuristic.
    /// </summary>
    /// <param name="levelID"></param>
    /// <returns></returns>
    public bool GetHeuristicUnlockStatus(HInterface.HType heuristic)
    {
        if (heuristicsProgress.ContainsKey((int)heuristic))
        {
            return heuristicsProgress[(int)heuristic];
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Use this method to get the unlock status of a given base Unit.
    /// Returns false if there is no entry about the given base Unit.
    /// </summary>
    /// <param name="levelID"></param>
    /// <returns></returns>
    public bool GetBaseUnitUnlockStatus(string unitID)
    {
        if (baseUnitProgress.ContainsKey(unitID))
        {
            return baseUnitProgress[unitID];
        }
        else
        {
            return false;
        }
    }

    private void saveLevelProgress()
    {
        string JSON = JsonUtility.ToJson(levelProgress);
        File.WriteAllText(levelProgressFilePath, JSON);
    }

    private void saveHeuristicProgress()
    {
        string JSON = JsonUtility.ToJson(heuristicsProgress);
        File.WriteAllText(heuristicsProgressFilePath, JSON);
    }

    private void saveUnitProgress()
    {
        string JSON = JsonUtility.ToJson(baseUnitProgress);
        File.WriteAllText(baseUnitProgressFilePath, JSON);
    }

    [ContextMenu("ClearLevelProgress")]
    private void clearLevelProgress()
    {
        levelProgress = new SerializableDictionary<string, bool>();
        saveLevelProgress();
    }

    [ContextMenu("ClearHeuristicProgress")]
    private void clearHeuristicProgress()
    {
        heuristicsProgress = new SerializableDictionary<int, bool>();
        saveHeuristicProgress();
    }

    [ContextMenu("ClearBaseUnitProgress")]
    private void clearUnitProgress()
    {
        baseUnitProgress = new SerializableDictionary<string, bool>();
        saveUnitProgress();
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

        ChangeHeuristicUnlockStatus(HInterface.HType.Attack, true);
        //ChangeHeuristicUnlockStatus(HInterface.HType.TargetNearestEnemy, true);

        Debug.Log(GetHeuristicUnlockStatus(HInterface.HType.Attack)); // should print false
        //Debug.Log(GetHeuristicUnlockStatus(HInterface.HType.TargetNearestEnemy)); // should print true

        ChangeUnitUnlockStatus("TestUnit1", true);
        ChangeUnitUnlockStatus("TestUnit2", false);
        ChangeUnitUnlockStatus("TestUnit1", false);
        ChangeUnitUnlockStatus("TestUnit2", true);

        Debug.Log(GetBaseUnitUnlockStatus("TestUnit1")); // should print false
        Debug.Log(GetBaseUnitUnlockStatus("TestUnit2")); // should print true
    }
}

// We need to include a serializable version of the Dictionary for this to work
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}
