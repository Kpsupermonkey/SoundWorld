using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("References")]
    public Light sunLight;          // Main directional light (the sun)
    public Light moonLight;         // Optional secondary light
    public Light[] streetLamps;     // All lamps in the city

    [Header("Settings")]
    [Range(0f, 24f)] public float timeOfDay = 12f; // Current time, 0–24
    public float dayLengthInMinutes = 5f;          // How long a full cycle takes in real minutes
    public float sunIntensity = 1f;
    public float nightSunIntensity = 0.1f;

    private float timeSpeed;

    void Start()
    {
        timeSpeed = 24f / (dayLengthInMinutes * 60f);

        // Auto-find street lamps by tag
        GameObject[] lampObjects = GameObject.FindGameObjectsWithTag("StreetLight");
        streetLamps = new Light[lampObjects.Length];
        for (int i = 0; i < lampObjects.Length; i++)
        {
            streetLamps[i] = lampObjects[i].GetComponent<Light>();
        }
    }

    void Update()
    {
        // Advance time
        timeOfDay += Time.deltaTime * timeSpeed;
        if (timeOfDay >= 24f) timeOfDay = 0f;

        UpdateLighting();
    }

    void UpdateLighting()
    {
        // Rotate the sun
        float sunAngle = (timeOfDay / 24f) * 360f - 90f;
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // Adjust sun intensity based on angle
        float intensity = Mathf.Lerp(nightSunIntensity, sunIntensity, Mathf.Clamp01(Vector3.Dot(sunLight.transform.forward, Vector3.down)));
        sunLight.intensity = intensity;

        // Determine if it’s night
        bool nightTime = timeOfDay >= 18f || timeOfDay < 6f;

        // Toggle street lamps and moon
        foreach (var lamp in streetLamps)
        {
            if (lamp != null)
                lamp.enabled = nightTime; // on at night, off during day
        }

        if (moonLight != null)
            moonLight.enabled = nightTime;
    }
    
    public bool IsNightTime()
    {
        return timeOfDay >= 18f || timeOfDay < 6f; // same logic you use for street lamps
    }
}