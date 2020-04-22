using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMultiDirView : MonoBehaviour
{
    public Sprite[] sprites;
    public AnimationClip[] anims;
    public bool isAnimated;

    Animator animator;
    SpriteRenderer spriteRenderer;

    public float angle;
    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        angle = getAngle();

        if (angle >= 270 + 22.5 && angle < 315 + 22.5)
            ChangeSprite(5);
        else if ((angle >= 315 + 22.5 && angle <= 360) || (angle >= 0 && angle <= 22.5))
            ChangeSprite(6);
        else if ((angle >=22.5 && angle < 45 + 22.5))
            ChangeSprite(7);
        else if (angle >= 45 + 22.5 && angle < 90 + 22.5)
            ChangeSprite(0);
        else if (angle >= 90 + 22.5 && angle < 135 + 22.5)
            ChangeSprite(1);
        else if (angle >= 135 + 22.5 && angle < 180 + 22.5)
            ChangeSprite(2);
        else if (angle >= 180 + 22.5 && angle < 225 + 22.5)
            ChangeSprite(3);
        else if (angle >= 225 + 22.5 && angle < 270 + 22.5)
            ChangeSprite(4);
    }

    void ChangeSprite(int index)
    {
        if (isAnimated == true)
            animator.Play(anims[index].name);
        else
            spriteRenderer.sprite = sprites[index];
    }

    float getAngle()
    {
        Vector3 direction = Camera.main.transform.position - this.transform.position;
        float angleTemp = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        angleTemp += 180f;
        return angleTemp;
    }
}
