using UnityEngine.UI;

public class PriceBar : Billboard {
    public Text Text;

    public int Price {
        set => Text.text = value.ToString();
    }
}