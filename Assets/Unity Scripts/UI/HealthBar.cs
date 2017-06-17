using UnityEngine;

[ExecuteInEditMode]
public class HealthBar : MonoBehaviour
{

    [Range(0,1)]
    public float hide_percent = 0;

    private SpriteRenderer spriteRenderer;

    SpriteAnimator animator;
    SpriteAnimator container_animator;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<SpriteAnimator>();
        container_animator = transform.parent.GetComponent<SpriteAnimator>();

        UpdateShader();
    }


    void OnDisable()
    {
        UpdateShader();
    }

    void Update()
    {
        UpdateShader();

        //TODO FIX
        if (animator.IsPlaying("damaged") && (animator.currentFrame == animator.currentAnimation.frames.Length-1))
        {
            animator.Play("pulse", true);
            container_animator.Play("pulse", true);
        }
    }

    public void GetHurt()
    {
        //animator.Play("damaged",false);
        //container_animator.Play("damaged",false);
    }
    

    void UpdateShader()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_HidePercent", hide_percent);
        mpb.SetFloat("_Width", spriteRenderer.sprite.rect.width);
        mpb.SetFloat("_Height", spriteRenderer.sprite.rect.height);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}
