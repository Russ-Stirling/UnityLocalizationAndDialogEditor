using System;

[Serializable]
public class TranslationKeyValue
{
    //Constructor for creating new translations
    public TranslationKeyValue(string key, string value)
    {
        Key = key;
        Value = value;
    }
    public string Key;
    public string Value;
}
