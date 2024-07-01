using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    private int _currentCharacterIndex = 0;

    private void Update() => ChangeCharacter();

    private void ChangeCharacter()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Switch character
            characters[_currentCharacterIndex].SetActive(false);
            _currentCharacterIndex = (_currentCharacterIndex + 1) % characters.Count;
            characters[_currentCharacterIndex].SetActive(true);
        }
    }
}
