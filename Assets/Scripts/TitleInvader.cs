using UnityEngine;

public class TitleInvader : MonoBehaviour
{
    public Sprite[] animationSprites;
    public float animationTime = 1.0f;
    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    }

    private void AnimateSprite()
    {
        if (animationSprites == null || animationSprites.Length == 0)
            return;

        _spriteRenderer.sprite = animationSprites[_animationFrame];

        _animationFrame++;
        if (_animationFrame >= animationSprites.Length)
        {
            _animationFrame = 0;
        }
    }
}
