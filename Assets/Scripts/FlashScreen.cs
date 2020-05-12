using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour
{
    public Image flashScreen;
    Color yellow;
    Color red;
    Color green;
    Color blue;
    Color gold;

    void Start()
    {
        red = new Color(1, 0, 0, 0.6f);
        yellow = new Color(1, 1, 0, 0.6f);
        green = new Color(0, 1, 0, 0.6f);
        blue = new Color(0, 0, 1, 0.6f);
        gold = new Color(0.83f, 0.68f, 0.21f, 1.0f);
    }

    void Update()
    {
        if(flashScreen.color.a > 0)
        {
            Color invisible = new Color(flashScreen.color.r, flashScreen.color.g, flashScreen.color.b, 0);
            flashScreen.color = Color.Lerp(flashScreen.color, invisible, Time.deltaTime * 5);
        }
    }

    public void flash(string colorName)
    {
        if (colorName == "red")
            flashScreen.color = red;
        else if (colorName == "yellow")
            flashScreen.color = yellow;
        else if (colorName == "green")
            flashScreen.color = green;
        else if (colorName == "blue")
            flashScreen.color = blue;
        else if (colorName == "gold")
            flashScreen.color = gold;
    }
}
