using UnityEngine;
using UnityEngine.UI;

public class LaboratoryButtonWidget : MonoBehaviour {
    public Button Button;
    public LaboratoryWidget LaboratoryWidget;

    private void Start() {
        Button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        LaboratoryWidget.gameObject.SetActive(true);
    }
}
