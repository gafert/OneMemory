using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    [SerializeField]
    private int id;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool active = false;

    void Awake()
    {
        animator = this.GetComponent<Animator>();
        spriteRenderer = this.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>();
    }

    public void setSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public Sprite getSprite()
    {
        return spriteRenderer.sprite;
    }

    public void setActive(bool active)
    {
        this.active = active;
    }

    public bool isActive()
    {
        return this.active;
    }

    public void setID(int id)
    {
        this.id = id;
    }

    public int getID()
    {
        return id;
    }

    public void flipCard(bool up)
    {
        if (up)
        {
            animator.SetTrigger("flipUp");
            setActive(true);
        }
        if (!up)
        {
            animator.SetTrigger("flipDown");
            setActive(false);
        }
    }
}
