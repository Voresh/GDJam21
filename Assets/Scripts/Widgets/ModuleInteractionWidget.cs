using System;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets {
    public class ModuleInteractionWidget : MonoBehaviour {
        public Text Text;
        public Button Button;
        public Image Icon;
        public Sprite Enabled;
        public Sprite Disabled;

        private void Start() {
            Button.onClick.AddListener(OnClick);
        }

        private void Update() {
            Button.gameObject.SetActive(RepairController.Instance.ReadyToInteract);
            Button.interactable = RepairController.Instance.InteractionAvailable;
            Text.gameObject.SetActive(!RepairController.Instance.InteractionAvailable);
            Icon.sprite = RepairController.Instance.InteractionAvailable ? Enabled : Disabled;
            Text.text = RepairController.Instance.InteractionAvailable ? "Repair" : "<color=red>Not enough points!</color>";
        }

        private void OnClick() {
            RepairController.Instance.TryInteract();
        }
    }
}
