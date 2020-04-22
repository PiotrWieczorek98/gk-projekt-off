using UnityEngine;
public class DynamicCrosshair : MonoBehaviour
{
    static public float spread = 0;
    const int WALK_SPREAD = 1000;
    const int SHOT_SPREAD = 10000;
    const int JUMP_SPREAD = 70000;
    const int RUN_SPREAD = 50000;
    const int MAX_SHOT_SPREAD = 70000;

    GameObject top;
    GameObject bot;
    GameObject left;
    GameObject right;
    float initPosition;

    void Start()
    {
        top = this.transform.Find("top").gameObject;
        bot = this.transform.Find("bot").gameObject;
        left = this.transform.Find("left").gameObject;
        right = this.transform.Find("right").gameObject;

        initPosition = top.GetComponent<RectTransform>().localPosition.y;
    }

    
    void Update()
    {
        if(spread != 0)
        {
            top.GetComponent<RectTransform>().localPosition = new Vector3(0, initPosition + spread, 0);
            bot.GetComponent<RectTransform>().localPosition = new Vector3(0, -initPosition - spread, 0);
            left.GetComponent<RectTransform>().localPosition = new Vector3(-(initPosition + spread),0, 0);
            right.GetComponent<RectTransform>().localPosition = new Vector3(initPosition + spread,0, 0);
            spread -= 2;
            if (spread < 0)
                spread = 0;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            top.SetActive(false);
            bot.SetActive(false);
            left.SetActive(false);
            right.SetActive(false);
        }
        else
        {
            top.SetActive(true);
            bot.SetActive(true);
            left.SetActive(true);
            right.SetActive(true);
        }
    }

    public static void changeSpread(string type)
    {
        if (type == "walk")
            spread = WALK_SPREAD; 
        else if (type == "jump")
            spread = JUMP_SPREAD;
        else if(type == "run")
            spread = RUN_SPREAD;
        else if (type == "shot")
        {
            if (spread + SHOT_SPREAD < MAX_SHOT_SPREAD)
                spread += SHOT_SPREAD;
            else
                spread = MAX_SHOT_SPREAD;
        }
    }
}
