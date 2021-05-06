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
            Button.gameObject.SetActive(RepairController.Instance.ReadyToInteract);
            Button.interactable = RepairController.Instance.InteractionAvailable;
            Text.text = RepairController.Instance.InteractionAvailable ? "Repair" : "<color=red>Not enough points!</color>";
        }

        private void OnClick() {
            RepairController.Instance.TryInteract();
        }
    }
}
