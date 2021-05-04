using System;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets {
    public class ModuleInteractionWidget : MonoBehaviour {
        public Button Button;

        private void Start() {
            Button.onClick.AddListener(OnClick);
        }

        private void Update() {
            Button.gameObject.SetActive(ModulesController.Instance.ReadyToInteract);
            Button.interactable = ModulesController.Instance.InteractionAvailable;
        }

        private void OnClick() {
            ModulesController.Instance.TryInteract();
        }
    }
}
