using UnityEngine;

public class SlimeColourChanger : MonoBehaviour
{
    public Color baseColour;
    [SerializeField] SpriteRenderer baseColourSpriteRenderer;
    [SerializeField] SpriteRenderer shadowColourSpriteRenderer;
    [SerializeField] SpriteRenderer shineColourSpriteRenderer;

    void Start()
    {
        if (baseColourSpriteRenderer == null)
        {
            Debug.LogError("No baseColourSpriteRenderer found");
            return;
        }

        if (shadowColourSpriteRenderer == null)
        {
            Debug.LogError("No shadowColourSpriteRenderer found");
            return;
        }

        if (shineColourSpriteRenderer == null)
        {
            Debug.LogError("No shineColourSpriteRenderer found");
            return;
        }

        // shadow and shine are transparent before import
        // so they automatically scale the baseColour to their transparency etc.
        baseColourSpriteRenderer.color = baseColour;
        shadowColourSpriteRenderer.color = baseColour;
        shineColourSpriteRenderer.color = baseColour;
    }
}
