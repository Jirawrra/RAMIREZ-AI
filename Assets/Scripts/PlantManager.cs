using UnityEngine;
using System;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance;
    public event Action OnNoPlantsLeft;
    [SerializeField] private int plantsRemaining; // shows in Inspector

    private bool hasFiredNoPlantsEvent = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        plantsRemaining = CountPlants();
        Debug.Log("Initial plants in scene: " + plantsRemaining);
    }

    public void PlantRemoved()
    {
        plantsRemaining = CountPlants();

        Debug.Log("Plants remaining: " + plantsRemaining);

        if (!hasFiredNoPlantsEvent && plantsRemaining <= 1)
        {
            hasFiredNoPlantsEvent = true;
            OnNoPlantsLeft?.Invoke();
            Debug.Log("No more plants left!");
        }
    }

    public int CountPlants()
    {
        return FindObjectsOfType<Watering>().Length;
    }

    // Optional getter if needed elsewhere
    public int GetPlantsRemaining()
    {
        return plantsRemaining;
    }
}