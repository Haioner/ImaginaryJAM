using UnityEngine;

public class LoadingTrigger : MonoBehaviour
{
    [SerializeField] private string NextSceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadingData.instance.ChangeScene(NextSceneName);
        }
    }
}
