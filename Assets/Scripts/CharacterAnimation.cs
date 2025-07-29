using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animacionpj : MonoBehaviour
{
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite leftFootSprite;
    [SerializeField] private Sprite rightFootSprite;

        private SpriteRenderer spriteRenderer;


 void Start()
{
    spriteRenderer = GetComponent<SpriteRenderer>();
}
void ChangeAnimation(string animation)
{
    switch (animation)
    {
        case "Idle":
            spriteRenderer.sprite = idleSprite;
            break;
        case "LeftFoot":
            spriteRenderer.sprite = leftFootSprite;
            break;
        case "RightFoot":
            spriteRenderer.sprite = rightFootSprite;
            break;
    }
}

void Update()
{
    if (Time.time % 0.5f < 0.25f)
    {
        ChangeAnimation("LeftFoot");
    }
    else
    {
        ChangeAnimation("RightFoot");
    }
}


}
