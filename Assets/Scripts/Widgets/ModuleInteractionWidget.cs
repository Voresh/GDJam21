using System;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets {
    public class ModuleInteractionWidget : MonoBehaviour {
        public Text Text;
        public Button Button;

        private void Start() {
            Button.onClick.AddListener(OnClick);
        }

        private void Update() {
            Button.gameObject.SetActive(ModulesController.Instance.ReadyToInteract);
            Button.interactable = ModulesController.Instance.InteractionAvailable;
            Text.text = ModulesController.Instance.InteractionAvailable ? "Repair" : "<color=red>Not enough points!</color>";
        }

        private void OnClick() {
            ModulesController.Instance.TryInteract();
        }
    }
}
