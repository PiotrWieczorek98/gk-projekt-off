using UnityEngine;

public class DynamicMultiDirView : MonoBehaviour
{
    public Sprite[] sprites;
    public string[] animStates = new string[8] { "Bot", "BotLeft", "Left", "TopLeft", "Top", "TopRight", "Right", "BotRight" };
    public bool isAnimated;

    Animator anim;
    SpriteRenderer spriteRenderer;
    EnemyStates es;

    void Awake()
    {
        es = GetComponent<EnemyStates>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
     void Update()
    {
        if(es.currentState != es.attackState)
        getAngle();
    }

    void getAngle()
    {
        Vector3 playerDir = GameObject.FindGameObjectWithTag("UICamera").transform.forward;
        Vector3 enemyDir = transform.Find("Vision").forward;
        Vector3 playerRight = GameObject.FindGameObjectWithTag("UICamera").transform.right;

        playerRight.y = 0;
        playerDir.y = 0;
        enemyDir.y = 0;

        float dotProduct = Vector3.Dot(playerDir, enemyDir);
        float dotProductSide = Vector3.Dot(playerRight, enemyDir);

        if (dotProduct <= -0.66f && dotProduct >= -1f)
            changeSprite(0);
        else if (dotProduct >= 0.66f && dotProduct <= 1f)
            changeSprite(4);
        else if(dotProductSide >= 0)
        {
            //dotProductSide >= 0 = z prawej strony
            if (dotProduct < 0.66f && dotProduct > 0.33f)
                changeSprite(5);
            else if ((dotProduct <= 0.33f && dotProduct >= 0f) || (dotProduct <= 0f && dotProduct >= 0.33f))
                changeSprite(6);
            else if (dotProduct < -0.33f && dotProduct >= -0.66f)
                changeSprite(7);
        }
        else if (dotProductSide < 0)
        {
            //dotProductSide < 0 = z lewej strony
            if (dotProduct < 0.66f && dotProduct > 0.33f)
                changeSprite(3);
            else if ((dotProduct <= 0.33f && dotProduct >= 0f) || (dotProduct <= 0f && dotProduct >= 0.33f))
                changeSprite(2);
            else if (dotProduct < -0.33f && dotProduct >= -0.66f)
                changeSprite(1);
        }
    }

    void changeSprite(int index)
    {
        if (isAnimated)
            anim.Play(animStates[index]);
        else
            spriteRenderer.sprite = sprites[index];
    }
    public bool animatorIsPlaying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).length >
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    public bool animatorIsPlaying(string stateName)
    {
        return animatorIsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
