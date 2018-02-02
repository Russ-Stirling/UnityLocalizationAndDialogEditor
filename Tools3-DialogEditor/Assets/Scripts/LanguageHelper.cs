using UnityEngine;

public static class LanguageHelper
{
    //dictionary containing all languages
    public static LanguageDictionary Languages;

    //loads the language data to the language dictionary
    static LanguageHelper()
    {
        Languages = Resources.Load<LanguageDictionary>("Data");
    }

    public static string ReplacePhraseWithTranslation(string replacementPhraseKey)
    {

        string value = Languages.FindTranslation(replacementPhraseKey);

        //for null returns inform the user which language is missing a translation using the returned text
        if (string.IsNullOrEmpty(value))
            value = string.Format("[Translation not found for {0}]", replacementPhraseKey);

        return value;
    }

    //All below functions are simply helpers for accessing Language dictionary functions without giving editor direct access to language dictionary

    public static void AddLanguage(string languageName)
    {
        Languages.AddLanguage(languageName);
    }
    public static void RemoveLanguage(string languageName)
    {
        Languages.RemoveLanguage(languageName);
    }

    public static void AddPhraseToLanguage(string translationKey, string translation, string initialLanguage)
    {
        Languages.AddPhraseToLanguages(translationKey, translation, initialLanguage);
    }

    public static void RemovePhraseFromLanguages(string translationKey)
    {
        Languages.RemovePhraseFromLanguages(translationKey);
    }

    public static void SetSelectedLanguage(string languageKey)
    {
        Languages.SetSelected(languageKey);
    }

    public static bool LanguageNameExists(string languageName)
    {
        return Languages.CheckIfLanguageExists(languageName);
    }

    public static bool CheckIfTranslationKeyExists(string phraseKey)
    {
        return Languages.CheckIfTranslationPhraseExists(phraseKey);
    }
}
