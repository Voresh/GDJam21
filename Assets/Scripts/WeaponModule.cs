using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine;

public class WeaponModule : Module {
    public Transform DropPoint;
    public GameObject[] DropItems;
    private bool Work;

    public GameObject Weapon;

    protected override void AddStaticEffects() {
        Work = this.RepairedAtStart;
        BotSpawner.Instance.onWaveStatusUpdated += OnWaveStatusUpdated;
    }

    private void OnWaveStatusUpdated(bool active) {
        if (!active && Work && Weapon == null)
        {
            var Drop = Instantiate(DropItems[Random.Range(0, DropItems.Length - 1)]);
            Drop.transform.position = DropPoint.position;
            Weapon = Drop;
        }
    }

    protected override void UpdateEffects(bool active) {
        Work = active;
    }
}
