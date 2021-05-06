using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour, IRepairable {
    public PriceBar PriceBarPrefab;
    public Renderer Renderer;
    public NavMeshObstacle NavMeshObstacle;
    public List<Module> ModulesToUnlock;
    public List<GameObject> RespawnesToUnlock;
    public List<Door> DoorsToActivate;
    public int InitialRepairPrice = 5;
    private PriceBar _PriceBar;
    public float PriceHeightOffset = 2f;
    
    public Vector3 RepairablePosition => transform.position;
    public bool Repaired { get; set; }
    public int RepairPrice => InitialRepairPrice;

    private void Start() {
        if (PriceBarPrefab != null) {
            _PriceBar = Instantiate(PriceBarPrefab);
            _PriceBar.Price = RepairController.Instance.GetRepairPrice(this);
        }
    }

    private void Update() {
        if (_PriceBar != null) {
            _PriceBar.transform.position = transform.position + Vector3.up * PriceHeightOffset;
            _PriceBar.Text.text = RepairController.Instance.GetRepairPrice(this).ToString();
        }
    }

    public void Repair() {
        _PriceBar.gameObject.SetActive(false);
        Renderer.enabled = false;
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
