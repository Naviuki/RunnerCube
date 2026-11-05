using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sunLight;  // Directional Light acting as the sun
    public float cycleDuration = 120f;  // Duration of a complete day-night cycle in seconds
    public Gradient ambientColorGradient;  // Gradient for ambient color over the cycle
    public AnimationCurve sunIntensityCurve;  // Animation curve for sun intensity over the cycle

    private float time;

    void Start()
    {
        if (sunLight == null)
        {
            sunLight = GetComponent<Light>();
        }

        time = 1000f;
    }

    void Update()
    {
        // Increase time
        time += Time.deltaTime;
        float cycleTimeNormalized = (time % cycleDuration) / cycleDuration;

        // Rotate the sun
        float sunAngle = Mathf.Lerp(0f, 360f, cycleTimeNormalized);
        sunLight.transform.rotation = Quaternion.Euler(sunAngle - 90, 170, 0);

        // Update sun intensity based on curve
        sunLight.intensity = sunIntensityCurve.Evaluate(cycleTimeNormalized);

        // Update ambient light color based on gradient
        RenderSettings.ambientLight = ambientColorGradient.Evaluate(cycleTimeNormalized);
    }
}
