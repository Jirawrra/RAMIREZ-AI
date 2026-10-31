using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer playerRenderer;
    public Sprite makahiya;

    [SerializeField] private GameObject particlePrefab;

    private void OnEnable()
    {
        if (PlantManager.Instance != null)
        {
            PlantManager.Instance.OnNoPlantsLeft += ShowOverlaySprite;
            PlantManager.Instance.OnNoPlantsLeft += SpawnParticles;
        }
    }

    private void OnDisable()
    {
        if (PlantManager.Instance != null)
        {
            PlantManager.Instance.OnNoPlantsLeft -= ShowOverlaySprite;
            PlantManager.Instance.OnNoPlantsLeft -= SpawnParticles;
        }
    }

    private void Start()
    {
        if (playerRenderer != null)
            playerRenderer.enabled = false;
    }

    void ShowOverlaySprite()
    {
        if (playerRenderer != null && makahiya != null)
        {
            playerRenderer.sprite = makahiya;
            playerRenderer.enabled = true;
            Debug.Log("Player overlay sprite shown: No Plants Left");
        }
    }

    private void SpawnParticles()
    {
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
    }
}