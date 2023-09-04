using System;
using System.Collections;
using TMPro;
using UnityEngine;
// Inspire de https://forum.unity.com/threads/real-fade-of-text-mesh-pro.620833/#:~:text=Changing%20the%20Vertex%20Color%20alpha,any%20of%20the%20material%20properties.
[RequireComponent(typeof(TextMeshProUGUI))]
public class FadeTextMeshPro : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    [SerializeField] float fadingSpeed = 1;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        fadeCoroutine = StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine()
    {
        float waitTime = 0;
        while (waitTime < 1)
        {
            textMeshPro.fontMaterial.SetColor("_FaceColor", Color.Lerp(Color.white, Color.clear, waitTime));
            yield return null;
            waitTime += Time.deltaTime / fadingSpeed;

        }

    }
}