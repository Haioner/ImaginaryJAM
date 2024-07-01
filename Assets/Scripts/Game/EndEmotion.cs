using UnityEngine;

public class EndEmotion : StateMachineBehaviour
{
    private MeticEmotions meticEmotions;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        meticEmotions = animator.GetComponent<MeticEmotions>();
        meticEmotions.canAddWeight = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        meticEmotions.canAddWeight = false;
    }
}
