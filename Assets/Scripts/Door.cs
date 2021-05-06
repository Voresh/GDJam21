using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour, IRepairable {
    public Renderer Renderer;
    public Collider Collider;
    public NavMeshObstacle NavMeshObstacle;
    public List<Module> ModulesToUnlock;
    public List<GameObject> RespawnesToUnlock;
    public List<Door> DoorsToActivate;
    public int InitialRepairPrice = 5;

    public Vector3 RepairablePosition => transform.position;
    public bool Repaired { get; set; }
    public int RepairPrice => InitialRepairPrice;
    
    public void Repair() {
        Renderer.enabled = false;
        Collider.enabled = false;
        NavMeshObstacle.enabled = false;
        Repaired = true;
        foreach (var module in ModulesToUnlock) {
            if (!module.Unlocked)
                module.Unlocked = true;
        }
        foreach (var respawn in RespawnesToUnlock) {
            respawn.SetActive(true);
        }
        foreach (var door in DoorsToActivate) {
            if (door.Repaired)
                continue; //safety
            if (door == this)
                continue; //safety
            door.Repair();
        }
    }
}
