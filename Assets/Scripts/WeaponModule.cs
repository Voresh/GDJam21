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
    public int WeaponIndex;
    public int EquippedWeaponIndex = 999;

    protected override void AddStaticEffects() {
        Work = this.RepairedAtStart;
        BotSpawner.Instance.onWaveStatusUpdated += OnWaveStatusUpdated;
    }

    private void OnWaveStatusUpdated(bool active) {
        if (!active && Work && Weapon == null)
        {
            do {
                WeaponIndex = Random.Range(0, DropItems.Length - 1);
            }
            while (WeaponIndex == EquippedWeaponIndex);

            var Drop = Instantiate(DropItems[WeaponIndex]);
            Drop.transform.position = DropPoint.position;
            Weapon = Drop;
            EquippedWeaponIndex = WeaponIndex;
        }
    }

    protected override void UpdateEffects(bool active) {
        Work = active;
    }
}
