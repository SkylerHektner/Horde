using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetCheckpointTool : EditorWindow
{
    private Dictionary<string, Room> nameToRoom;
    private List<string> checkpointNames;
    private int index;

    [MenuItem ("Tools For Alex/Set Current Checkpoint")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(SetCheckpointTool));
        window.Show();
    }


    void OnGUI()
    {
        nameToRoom = new Dictionary<string, Room>();
        checkpointNames = new List<string>();

        foreach(Room r in FindObjectsOfType<Room>())
        {
            if(r.IsCheckpoint)
            {
                checkpointNames.Add(r.RoomName);
                nameToRoom.Add(r.RoomName, r);
            } 
        }

        GUILayout.Label("\nSelect which room should be the current checkpoint:\n ");
        index = EditorGUILayout.Popup(index, checkpointNames.ToArray());
        GUILayout.Label("\n");

        if (GUILayout.Button("Set"))
            SetCheckpoint();

        if(GUILayout.Button("Reset Checkpoint"))
            PlayerPrefs.SetInt("Checkpoint", 0);
    }

    private void SetCheckpoint()
    {
        Room newCheckpoint = nameToRoom[checkpointNames[index]];
        PlayerPrefs.SetInt("Checkpoint", GameManager.Instance.Rooms.IndexOf(newCheckpoint));
    }
}
