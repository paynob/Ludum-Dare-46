using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    public Animator animator;

    public Color lowWarningColor;
    public Color midWarningColor;
    public Color highWarningColor;
    public SpriteRenderer[] sprites;
    public ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWarningLevel (float level) {
        if (level > 0f) {
            Color c = GetColor(level);
            for (int i = 0; i < sprites.Length; i++) {
                sprites[i].color = c;
            }

            if (particleSystem != null) {
                ParticleSystem.MainModule mainModule = particleSystem.main;
                mainModule.startColor = c;
            }

            animator.Play("Base Layer.Warning");
        }else{
            animator.Play("Base Layer.WarningDisabled");
        }
    }

    public Color GetColor (float level) {
        if (level < 0.33f) {
            return lowWarningColor;
        }else if (level < 0.66f) {
            return midWarningColor;
        }else {
            return highWarningColor;
        }
    }
}
