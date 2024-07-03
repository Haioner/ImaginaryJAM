using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine.AI;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private bool interactOnce = false;
    [SerializeField] private DialogueSystemTrigger dialogueTrigger;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] List<Transform> targetLocation = new List<Transform>();
    private bool dialogueTriggered = false;
    private int _currentTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        if (!dialogueTriggered)
        {
            if (interactOnce)
                dialogueTriggered = true;

            if (DialogueManager.isConversationActive)
                DialogueManager.StopConversation();

            dialogueTrigger.OnUse();
        }
    }

    public void MoveToTargetLocation()
    {
        if (agent != null && targetLocation != null)
        {
            if (_currentTarget <= targetLocation.Count)
            {
                agent.stoppingDistance = 0;
                agent.SetDestination(targetLocation[_currentTarget].position);
                _currentTarget++;
            }
        }
    }
}