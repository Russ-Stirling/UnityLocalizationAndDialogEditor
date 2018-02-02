using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

//for giving initial data creation option
[CreateAssetMenu(fileName = "Data", menuName = "Language Dictionary", order = 1)]
public class LanguageDictionary : ScriptableObject
{
    //the list of languages we will be accessing
    public List<Language> Languages = new List<Language>();

    public void AddLanguage(string languageName)
    {
        //generates a new language with specified name
        var newLanguage = new Language(languageName);

        //if keys have been added to the other languages this creates those keys with placeholder values for the language
        if (Languages.Any(x => x.LanguageKey != languageName && x.KeyWords.Count > 0))
        {
            var otherKeys = Languages.First(x => x.LanguageKey != languageName && x.KeyWords.Count > 0).KeyWords
                .Select(x => x.Key).ToList();

            foreach (var key in otherKeys)
            {
                newLanguage.KeyWords.Add(new TranslationKeyValue(key, string.Format("{0} Translation not entered", languageName)));
            }
        }
        //adds new language to list of languages
        Languages.Add(newLanguage);
    }

    public void RemoveLanguage(string languageKey)
    {
        //finds language to remove from list of languages based on key and removes it
        var toRemove = Languages.FirstOrDefault(x => x.LanguageKey == languageKey);
        if (toRemove != null)
        {
            Languages.Remove(toRemove);
        }
    }

    public void AddPhraseToLanguages(string translationKey, string translation, string initialLanguage)
    {
        //adds specified phrasekey to all languages
        string translationValue = string.Empty;
        foreach (var language in Languages)
        {
            //if language is the current language adds specified value, otherwise adds placeholder value
            translationValue = language.LanguageKey == initialLanguage ? translation : string.Format("{0} Translation not entered", language.LanguageKey);
            language.KeyWords.Add(new TranslationKeyValue(translationKey, translationValue));
        }
    }

    public bool CheckIfTranslationPhraseExists(string phraseKey)
    {
        //if any languages have a phrasekey already retur true
        return Languages.Any(x => x.KeyWords.Any(y => y.Key == phraseKey));
    }

    public bool CheckIfLanguageExists(string languageName)
    {
        //if any languages have a name already retur true
        return Languages.Any(x => x.LanguageKey == languageName);
    }

    public string FindTranslation(string replacementPhraseKey)
    {
        //get currenttly selected language
        var selectedLanguage = Languages.FirstOrDefault(x => x.IsCurrent);

        if (selectedLanguage == null)
            return string.Empty;

        //get translation if it exists
        var keyWordPair = selectedLanguage.KeyWords.FirstOrDefault(x => x.Key == replacementPhraseKey);

        return keyWordPair == null ? string.Empty : keyWordPair.Value;
    }

    internal void SetSelected(string languageKey)
    {
        //set selected language based on languagekey
        foreach (var language in Languages)
        {
            language.IsCurrent = language.LanguageKey == languageKey;
        }
    }

    public void RemovePhraseFromLanguages(string translationKey)
    {
        //remove a phrase key and value from all languages in list
        foreach (var language in Languages)
        {
            var toRemove = language.KeyWords.FirstOrDefault(x => x.Key == translationKey);
            if (toRemove != null)
            {
                language.KeyWords.Remove(toRemove);
            }
        }
    }

    public List<Language> GetLanguages()
    {
        //gets the language list
        return Languages;
    }
}
