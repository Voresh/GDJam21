﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LaboratoryWidget : MonoBehaviour {
    public Button CloseButton;
    public Transform BranchesRoot;
    public LaboratoryBranchWidget BranchPrefab;

    private List<LaboratoryBranchWidget> BranchWidgets = new List<LaboratoryBranchWidget>();
    
    private void Start() {
        CloseButton.onClick.AddListener(OnCloseClick);
    }

    private void OnCloseClick() {
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        Time.timeScale = 0;
        foreach (var branch in BranchWidgets) {
            Destroy(branch.gameObject);
        }
        BranchWidgets.Clear();
        var laboratoryModule = (LaboratoryModule) ModulesController.Instance._Modules.First(_ => _ is LaboratoryModule);
        foreach (var branch in laboratoryModule.Branches) {
            var branchWidget = Instantiate(BranchPrefab, BranchesRoot);
            branchWidget.Setup(branch.Name);
            BranchWidgets.Add(branchWidget);
        }
    }

    private void OnDisable() {
        Time.timeScale = 1;
    }
}
