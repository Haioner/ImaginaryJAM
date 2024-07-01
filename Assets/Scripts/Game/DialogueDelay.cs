using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DialogueDelay : MonoBehaviour
{
    [SerializeField] private DialogueSystemTrigger dialogueTrigger;
    [SerializeField] private float Delay = 1f;
    private float currentDelay;
    private bool doOnce = true;

    private void Update()
    {
        if(currentDelay < Delay)
        {
            currentDelay += Time.deltaTime;
        }
        else if (doOnce)
        {
            doOnce = false;
            dialogueTrigger.OnUse();
        }
    }
}
