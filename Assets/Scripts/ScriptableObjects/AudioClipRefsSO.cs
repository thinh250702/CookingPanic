using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip chop;
    public AudioClip[] deliveryFailed;
    public AudioClip[] deliverySuccess;
    public AudioClip[] footstep;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickup;
    public AudioClip stoveSizzle;
    public AudioClip[] trash;
    public AudioClip[] warning;

    public AudioClip[] maleHello;
    public AudioClip[] femaleHello;
    public AudioClip[] maleHappy;
    public AudioClip[] femaleHappy;
    public AudioClip[] maleDisappoint;
    public AudioClip[] femaleDisappoint;
    public AudioClip[] maleDisgust;
    public AudioClip[] femaleDisgust;
}
