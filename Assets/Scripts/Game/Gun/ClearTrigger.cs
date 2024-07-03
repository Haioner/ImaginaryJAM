using UnityEngine;

[System.Serializable]
public enum TriggerType
{
    Clear, Switch
}

public class ClearTrigger : MonoBehaviour
{
    [SerializeField] private TriggerType triggerType;
    private GunController gunController;

    private MeshRenderer meshRenderer;
    private Color initialMatColor;
    private bool isEnable;

    private void OnEnable()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        //Get or Set Initial Color
        if (GetComponent<DoorController>() != null)
        {
            if (initialMatColor == Color.clear)
                initialMatColor = meshRenderer.material.GetColor("_Tint");
            else
            {
                meshRenderer.material.SetColor("_Tint", initialMatColor);
                GetComponent<DoorController>().ChangeInitialMat(meshRenderer.material);
            }
        }

        
        isEnable = true;
    }

    private void OnDisable()
    {
        if(GetComponent<DoorController>() != null)
        {
            Color grayColor = Color.gray;
            grayColor.a = initialMatColor.a;
            meshRenderer.material.SetColor("_Tint", grayColor);
            GetComponent<DoorController>().ChangeInitialMat(meshRenderer.material);
        }

        isEnable = false;
    }

    public bool GetIsEnable()
    {
        return isEnable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isEnable) return;

        if (other.CompareTag("Player"))
        {
            CallTrigger();
        }

        if (other.CompareTag("Player") && gunController != null)
        {
            gunController.shootDistance = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isEnable) return;

        if (other.CompareTag("Player") && triggerType == TriggerType.Clear)
        {
            CallTrigger();
        }

        if (other.CompareTag("Player") && gunController != null)
        {
            gunController.shootDistance = 50f;
        }
    }

    public void CallTrigger()
    {
        if (gunController == null)
            gunController = FindFirstObjectByType<GunController>();

        switch (triggerType)
        {
            case TriggerType.Clear:
                gunController.Clear();
                break;
            case TriggerType.Switch:
                gunController.CallLockerInteract();
                gunController.Clear();
                break;
        }
    }
}
