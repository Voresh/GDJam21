using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaboratoryModule : Module {
    public List<LaboratoryBranch> Branches;

    public Dictionary<string, int> BranchProgress = new Dictionary<string, int>();

    public int Points { get; private set; }

    protected override void AddStaticEffects() {
        LoadProgress();
        foreach (var progressKVP in BranchProgress) {
            var branchName = progressKVP.Key;
            var branch = Branches.First(_ => _.Name == branchName);
            var buff = branch.Buffs[progressKVP.Value];
            UpdateBuff(buff);
        }
        BotSpawner.Instance.onBotSpawned += OnBotSpawned;
    }

    private void LoadProgress() {
        foreach (var branch in Branches) {
            var progress = PlayerPrefs.GetInt(branch.Name, 0);
            BranchProgress.Add(branch.Name, progress);
        }
    }

    private void SaveProgress() {
        foreach (var branchProgressKVP in BranchProgress) {
            PlayerPrefs.SetInt(branchProgressKVP.Key, branchProgressKVP.Value);
        }
    }

    public void UpgradeBranchProgress(string branchName) {
        var branch = Branches.First(_ => _.Name == branchName);
        var currentProgress = BranchProgress[branchName];
        var nextProgress = currentProgress + 1;
        if (!UpgradeAvailable(branchName, nextProgress)) {
            Debug.LogError("upgrade failed!");
            return;
        }
        var nextBuff = branch.Buffs[nextProgress];
        Points -= nextBuff.Price;
        BranchProgress[branchName] = nextProgress;
        SaveProgress();
    }

    public bool UpgradeAvailable(string branchName, int progress) {
        var branch = Branches.First(_ => _.Name == branchName);
        var currentProgress = BranchProgress[branchName];
        if (progress >= branch.Buffs.Count) {
            Debug.Log("maximum progress!");
            return false;
        }
        var nextBuff = branch.Buffs[progress];
        if (Points < nextBuff.Price) {
            Debug.Log("not enough points!");
            return false;
        }
        if (progress <= currentProgress) {
            Debug.Log("already upgraded!");
            return false;
        }
        if (progress > currentProgress + 1) {
            Debug.Log("skipped upgrade step!");
            return false;
        }
        return true;
    }

    public bool EnoughPoints(string branchName, int index) {
        var branch = Branches.First(_ => _.Name == branchName);
        var nextBuff = branch.Buffs[index];
        return Points >= nextBuff.Price;
    }

    public bool UpgradeComplete(string branchName, int index) {
        var currentProgress = BranchProgress[branchName];
        return currentProgress >= index;
    }

    private void OnApplicationQuit() {
        SaveProgress();
    }

    private void OnApplicationFocus(bool hasFocus) {
        SaveProgress();
    }

    private void UpdateBuff(LaboratoryBuff buff) {
        switch (buff.Type) {
            case LaboratoryBuffType.Health:
                PlayerController.Instance.Health.GlobalHealthBuff = buff.Amount;
                break;
            case LaboratoryBuffType.Damage: 
                PlayerController.Instance.BulletSpawner.GlobalDamageBuff = buff.Amount;
                break;
            case LaboratoryBuffType.Repair:
                ModulesController.Instance.GlobalRepairBuff = buff.Amount;
                break;
            default: 
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnBotSpawned(Bot bot) {
        bot.Health.onDeadStatusUpdated += OnBotDeadStatusUpdated;
    }

    private void OnBotDeadStatusUpdated(bool dead) {
        if (!dead)
            return;
        Points += 10;
    }
}
