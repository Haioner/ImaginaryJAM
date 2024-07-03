using UnityEngine;

public class InstanceCreator : MonoBehaviour
{
    [SerializeField] private Transform instancePos;
    [SerializeField] private GameObject instancePrefab;
    private GameObject currentInstance;

    public void Instance()
    {
        RemoveInstance();
        CreateInstance();
    }

    public void RemoveInstance()
    {
        if (currentInstance != null) 
        {
            if (currentInstance.TryGetComponent(out GrabbableDestroyer destroyer))
                destroyer.DestroyInstance();
            else
                Destroy(currentInstance);

            currentInstance = null;
        }
    }

    private void CreateInstance()
    {
        GameObject currentInstancePrefab = Instantiate(instancePrefab, instancePos.position, Quaternion.identity);
        currentInstance = currentInstancePrefab;
    }
}
