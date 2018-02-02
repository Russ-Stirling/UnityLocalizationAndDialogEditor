using System;
using System.Collections.Generic;
[Serializable]
public class Language
{
    //Constructors for use by the language dictionary when creating new languages
    public Language(string languageName)
    {
        LanguageKey = languageName;
        KeyWords = new List<TranslationKeyValue>();
    }
    public Language(string languageName, List<TranslationKeyValue> keyWords)
    {
        LanguageKey = languageName;
        KeyWords = keyWords;
    }
    //Idenntifier key
    public string LanguageKey;
    //List of translations
    public List<TranslationKeyValue> KeyWords;

    //To determine which language is currently selected
    public bool IsCurrent;

}
