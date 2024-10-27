using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableObject : MonoBehaviour
{
    public float dropYOffset = 0f;

    // Objenin merkez offset'i
    public Vector3 centerOffset = Vector3.zero;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        // Oyunun durumuna göre objenin taþýnabilirliðini kontrol et
        if (GameManager.Instance.currentGameState == GameState.InGame)
        {
            // Oyun baþladý, obje artýk taþýnamaz
            rb.isKinematic = true;
        }
        else
        {
            // PreLevel veya diðer durumlarda obje taþýnabilir
            rb.isKinematic = false;
        }
    }

    /// <summary>
    /// Objenin merkez offset'ini hesaplar.
    /// Bu metod, objenin Renderer bileþenlerine dayanarak merkezini hesaplar.
    /// </summary>
    public void CalculateCenterOffset()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            centerOffset = Vector3.zero;
            return;
        }

        Bounds combinedBounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        // Dünya pozisyonundaki merkez ile objenin pivot noktasý arasýndaki fark
        centerOffset = transform.InverseTransformPoint(combinedBounds.center) - transform.localPosition;
    }
}
