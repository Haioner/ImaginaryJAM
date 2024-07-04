//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnityEngine.Localization.Settings;
//using UnityEngine.Localization;
//using System.Collections.Generic;
using UnityEngine;
//using TMPro;

public class LocaleSelector : MonoBehaviour
{
    //[SerializeField] private TMP_Dropdown localesDropDown;
    //private AsyncOperationHandle initializeOperation;

    //private void Start()
    //{
    //    localesDropDown.onValueChanged.AddListener(OnSelectionChanged);

    //    localesDropDown.ClearOptions();
    //    localesDropDown.options.Add(new TMP_Dropdown.OptionData("Loading..."));
    //    localesDropDown.interactable = false;

    //    initializeOperation = LocalizationSettings.SelectedLocaleAsync;
    //    if (initializeOperation.IsDone)
    //        LoadOptions(initializeOperation);
    //    else
    //        initializeOperation.Completed += LoadOptions;
    //}

    //void LoadOptions(AsyncOperationHandle obj)
    //{
    //    //Add options for each locale
    //    var options = new List<string>();
    //    int selectedOption = 0;
    //    var locales = LocalizationSettings.AvailableLocales.Locales;
    //    for (int i = 0; i < locales.Count; i++)
    //    {
    //        var locale = locales[i];
    //        if (LocalizationSettings.SelectedLocale == locale)
    //            selectedOption = i;
    //        options.Add(locales[i].ToString());
    //    }

    //    //On ERROR
    //    if (options.Count == 0)
    //    {
    //        options.Add("No Locales Available");
    //        localesDropDown.interactable = false;
    //    }
    //    else
    //    {
    //        localesDropDown.interactable = true;
    //    }

    //    localesDropDown.ClearOptions();
    //    localesDropDown.AddOptions(options);
    //    localesDropDown.SetValueWithoutNotify(selectedOption);

    //    LocalizationSettings.SelectedLocaleChanged += LocalizationSelectedLocale;
    //    LoadSelection();
    //}

    //void OnSelectionChanged(int index)
    //{
    //    // Unsubscribe from SelectedLocaleChanged so we don't get an unnecessary callback from the change we are about to make.
    //    LocalizationSettings.SelectedLocaleChanged -= LocalizationSelectedLocale;

    //    var locale = LocalizationSettings.AvailableLocales.Locales[index];
    //    LocalizationSettings.SelectedLocale = locale;

    //    // Resubscribe to SelectedLocaleChanged so that we can stay in sync with changes that may be made by other scripts.
    //    LocalizationSettings.SelectedLocaleChanged += LocalizationSelectedLocale;
    //}

    //void LocalizationSelectedLocale(Locale locale)
    //{
    //    var selectedIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
    //    localesDropDown.SetValueWithoutNotify(selectedIndex);
    //}
    
    //public void SaveSelection()
    //{
    //    PlayerPrefs.SetInt("Locale", localesDropDown.value);
    //}

    //private void LoadSelection()
    //{
    //    localesDropDown.value = PlayerPrefs.GetInt("Locale");
    //}
}