using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class SetStartingRoom : ScriptableWizard
{
    [SerializeField] private string roomName;

    [MenuItem ("Tools For Alex/SetStartingRoom")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<SetStartingRoom> ("Set Starting Room", "Set");
    }

    void OnWizardCreate()
    {
        // Get a reference to the player spawn of the room.
        GameObject roomGO = GameObject.Find(roomName);
        Room r = roomGO.GetComponent<Room>();
        Transform t = r.transform.Find("Player-Spawn");

        // Get a reference to the player and warp him to the spawn.
        Player p = GameObject.FindObjectOfType<Player>();
        p.GetComponent<NavMeshAgent>().Warp(t.position);
        p.transform.rotation = t.rotation;

        // Change the current room to the one the player is in.
        GameManager.Instance.StartingRoom = r;
    }
}
