using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Watering : MonoBehaviour
{
    public bool isWatering = false;
    public float destroyDelay = 2f;
    public float waterNeeded;
    public float waterGiven;

    [Header("Noise Settings")]
    public float noiseRadius = 8f;
    public float noiseInterval = 1f;

    private float noiseTimer = 0f;

    public GameObject wateringUI;
    public Image wateringValueUI;

    public SpriteRenderer plantRenderer;
    public Sprite grownSprite;
    
    public GameManager gameManager;

    [HideInInspector]
    public Movement playerMovement;
    // ensure plantsWatered is only incremented once
    private bool plantsCounted = false;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        plantRenderer = GetComponent<SpriteRenderer>();

        // INITIALIZE WATERING UI
        if (wateringUI == null)
        {
            wateringUI = FindInChildren(GameObject.Find("Canvas"), "WateringBar");
        }

        if (wateringValueUI == null && wateringUI != null)
        {
            GameObject wateringValueObj = FindInChildren(wateringUI, "WateringValue");
            if (wateringValueObj != null)
            {
                wateringValueUI = wateringValueObj.GetComponent<Image>();
            }
        }

        if (wateringUI != null)
        {
            wateringUI.SetActive(false);
        }
    }

    //INITIALIZE WATERING UI
    GameObject FindInChildren(GameObject parent, string name)
    {
        if (parent == null) return null;

        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
       if (isWatering)
        {
            wateringUI.SetActive(true);

            waterGiven += 10 * Time.deltaTime;
            fillWater();

            noiseTimer += Time.deltaTime;

            if (noiseTimer >= noiseInterval)
            {
                NoiseManager.MakeNoise(transform.position, noiseRadius);
                noiseTimer = 0f;
            }
        }

        if (waterGiven >= waterNeeded)
        {
            Debug.Log("Plant has grown!");
            wateringUI.SetActive(false);
            playerMovement.interacting = false;
            isWatering = false;
            this.gameObject.GetComponent<Collider2D>().enabled = false;

            PlantManager.Instance.PlantRemoved();
            if (!plantsCounted)
            {
                plantsCounted = true;
                gameManager.plantsWatered++;
            }

            plantRenderer.sprite = grownSprite;
            StartCoroutine(destroyAfterDelay(destroyDelay));
        }
    }

    public void fillWater()
    {
        wateringValueUI.fillAmount = waterGiven / waterNeeded;
    }

    IEnumerator destroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

        void OnDrawGizmosSelected()
    {
        // Watering noise radius
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }
}
