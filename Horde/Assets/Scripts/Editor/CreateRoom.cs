using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateRoom : ScriptableWizard
{
    [SerializeField] private string roomName;

    [MenuItem ("Tools For Alex/CreateRoom")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<CreateRoom> ("Create Room", "Create");
    }

    void OnWizardCreate()
    {
        // Root Room object.
        GameObject room = new GameObject("Room" + roomName);
        room.AddComponent<Room>();

        // Children of root
        GameObject staticObjects = new GameObject("StaticObjects");
        GameObject breakables = new GameObject("Breakables");
        GameObject guards = new GameObject("Guards");
        GameObject playerSpawn = new GameObject("Player-Spawn");
        GameObject cameraSpawn = new GameObject("Camera-Spawn");
        GameObject exit = new GameObject("Exit");

        staticObjects.transform.SetParent(room.transform);
        breakables.transform.SetParent(room.transform);
        guards.transform.SetParent(room.transform);
        playerSpawn.transform.SetParent(room.transform);
        cameraSpawn.transform.SetParent(room.transform);
        exit.transform.SetParent(room.transform);

        //Children of StaticObjects
        GameObject walls = new GameObject("Walls");
        GameObject floors = new GameObject("Floors");
        GameObject other = new GameObject("Other");

        walls.transform.SetParent(staticObjects.transform);
        floors.transform.SetParent(staticObjects.transform);
        other.transform.SetParent(staticObjects.transform);
    }
}
