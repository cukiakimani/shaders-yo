using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public SpriteRenderer[] characterSpriteRenderers;
    public float selectedOutlineThickness = 0.0107f;
    private int currentSelected;

    void Start()
    {
        foreach (var sprite in characterSpriteRenderers)
        {
            sprite.material = new Material(sprite.material);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentSelected = (int)Mathf.Clamp(currentSelected + 1, 0, characterSpriteRenderers.Length - 1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentSelected = (int)Mathf.Clamp(currentSelected - 1, 0, characterSpriteRenderers.Length - 1);
        }

        for (int i = 0; i < characterSpriteRenderers.Length; i++)
        {
            float thickness = i == currentSelected ? selectedOutlineThickness : 0f;
            characterSpriteRenderers[i].material.SetFloat("_OutlineThickness", thickness);
        }
    }
}
