using UnityEngine;
public class MeticEmotions : MonoBehaviour
{
    private Animator anim;

    [HideInInspector] public bool canAddWeight;
    private float currentWeight;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        WeightLerp();
    }

    private void WeightLerp()
    {
        if (canAddWeight)
        {
            if(currentWeight < 1)
                currentWeight += Time.deltaTime;
            
        }
        else
        {
            if(currentWeight > 0)
                currentWeight -= Time.deltaTime;
            
        }
        anim.SetLayerWeight(1, currentWeight);
    }

    public void SetMeticEmotion(string animationName)
    {
        anim.SetLayerWeight(1,1);
        anim.Play(animationName);
    }
}
