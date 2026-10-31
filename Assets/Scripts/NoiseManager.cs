using UnityEngine;
using System;

public class NoiseManager : MonoBehaviour
{
    public static Action<Vector2, float> OnNoiseMade;

    public static void MakeNoise(Vector2 position, float radius)
    {
        OnNoiseMade?.Invoke(position, radius);
    }
}