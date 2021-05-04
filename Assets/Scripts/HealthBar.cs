using UnityEngine.UI;

public class HealthBar : Billboard {
    public Image FillImage;

    public float Fill {
        set => FillImage.fillAmount = value;
    }
}
