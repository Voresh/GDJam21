using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Image FillImage;

    public float Fill {
        set => FillImage.fillAmount = value;
    }
}
