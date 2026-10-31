using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int plantCount;
    PlantSpawner plantSpawner;
    public int plantsWatered = 0;
    public float transitionDuration = 1f;
    public TextMeshProUGUI plantsWateredText;
    public GameObject gameOverUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameOverUI.activeSelf)
        {
            gameOverUI.SetActive(false);
        }
        plantSpawner = this.gameObject.GetComponent<PlantSpawner>();
        plantSpawner.SpawnPlant(plantCount);
    }

    // Update is called once per frame
    void Update()
    {
        plantsWateredText.text = plantsWatered.ToString();
        if (plantsWatered >= plantCount)
        {
            StartCoroutine(TransitionToGameOver());
        }
    }

    IEnumerator TransitionToGameOver()
    {
        yield return new WaitForSeconds(transitionDuration);
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }
}
