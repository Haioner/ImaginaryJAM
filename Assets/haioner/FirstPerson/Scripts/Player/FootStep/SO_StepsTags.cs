using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundTypes
{
    public string tagName;
    public List<AudioClip> clips = new List<AudioClip>();
}

[CreateAssetMenu(fileName = "FootSteps")]
public class SO_StepsTags : ScriptableObject
{
    public List<GroundTypes> groundTypes = new List<GroundTypes>();
}
