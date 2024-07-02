using UnityEngine;

public class SpeedUnity : MonoBehaviour
{
  
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
            Time.timeScale = 5;
        if (Input.GetKeyUp(KeyCode.C))
            Time.timeScale = 1;
#endif
    }
}
