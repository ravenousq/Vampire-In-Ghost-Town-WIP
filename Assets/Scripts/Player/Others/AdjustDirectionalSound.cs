using UnityEngine;

public abstract class AdjustDirectionalSound
{
    public static void Adjuster(AudioSource audioSource, Player player, float distance, float minVolume = 0, float maxVolume = 1)
    {
        float distanceFromThePlayer = Vector2.Distance(audioSource.gameObject.transform.position, player.transform.position);

        if (distanceFromThePlayer < distance)
            audioSource.volume = Mathf.Clamp(Mathf.InverseLerp(distance, 0, distanceFromThePlayer), minVolume, maxVolume);
        else
            audioSource.volume = 0;
    }
}
