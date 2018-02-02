using System.Linq;
using UnityEngine;
using UnityEditor;

public class LoacalizationEditorWindow : EditorWindow
{

    //variables to be set through editor
    public LanguageDictionary Languages;
    public int selected = 0;
    public string[] languageOptions;
    public string languageName = string.Empty;
    public string phraseKey = string.Empty;
    public string phraseValue = string.Empty;
    public Vector2 scrollPos;

    //Add to the window menu
    [MenuItem("Window/Language Localization Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        LoacalizationEditorWindow window = (LoacalizationEditorWindow)GetWindow(typeof(LoacalizationEditorWindow));
        window.Show();
    }

    //Reload the resources on awake
    void Awake()
    {
        Languages = Resources.Load<LanguageDictionary>("Data");
    }

    void OnGUI()
    {
        //Scrolling for if window is too large for screen space
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.Separator();

        //label for clarity
        EditorGUILayout.LabelField("Choose current language:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        //Convert our language objects to an array for use in a drop down.
        //Ordered to ensure they always get same order
        languageOptions = Languages.GetLanguages().OrderBy(x => x.LanguageKey).Select(x => x.LanguageKey).ToArray();
        selected = EditorGUILayout.Popup("Select Language:", selected, languageOptions);

        if (languageOptions != null && languageOptions.Length > 0)
        {
            //To change selected language when it is changed in gui
            SetSelectedLanguage(languageOptions[selected]);
        }

        //Deleting a language
        if (GUILayout.Button("Delete Language"))
        {
            //Call function to delete
            DeleteLanguage(languageOptions[selected]);

            //set selected to zero and remake the languageOptions array
            selected = 0;
            languageOptions = Languages.GetLanguages().OrderBy(x => x.LanguageKey).Select(x => x.LanguageKey).ToArray();
        }
        EditorGUILayout.EndHorizontal();

        //styling
        EditorGUILayout.Separator();
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Add new language:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        //text field for new language names
        languageName = EditorGUILayout.TextField("Language Name: ", languageName);
        if (GUILayout.Button("Create", GUILayout.Width(100)))
        {
            Debug.Log(string.Format("{0}", languageName));
            //we dont want multiple languages with same name created so this checks against
            //and stops from occuring
            if (LanguageHelper.LanguageNameExists(languageName))
            {
                EditorUtility.DisplayDialog("Language Exists",
                    "The language name already exists. Please enter a unique language name.",
                    "OK");
            }
            else
            {
                //create the language
                CreateLanguage(languageName);

                //set selected language to the new language in the dropdown
                var temp = Languages.GetLanguages().OrderBy(x => x.LanguageKey).Select(x => x.LanguageKey).ToList()
                    .IndexOf(languageName);

                selected = temp;
            }
            //clear text field and return to end function processing
            languageName = string.Empty;
            return;
        }
        EditorGUILayout.EndHorizontal();

        //more styling
        EditorGUILayout.Separator();
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Modify language translations:", EditorStyles.boldLabel);

        //get the language object for selected language
        var selectedLanguage = Languages.GetLanguages().FirstOrDefault(x => x.LanguageKey == languageOptions[selected]);
        if (selectedLanguage != null && selectedLanguage.KeyWords != null && selectedLanguage.KeyWords.Count > 0)
        {
            //display a value editor for each pair of phrases and languages that exist
            foreach (var translationPair in selectedLanguage.KeyWords)
            {
                EditorGUILayout.BeginHorizontal();
                translationPair.Value = EditorGUILayout.TextField(translationPair.Key, translationPair.Value);
                //this allows you to delete a phrase-value. It will delete the phrase from all languages
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    //Confirmation to ensure user knows what will happen
                    if (EditorUtility.DisplayDialog("Delete Translation Phrase?",
                        "Deleting the translation will remove this translation phrase from all languages. Are you sure?",
                        "Delete", "Keep"))
                    {
                        DeleteLanguageTranslation(translationPair.Key);
                        //return to end processing of function
                        return;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        //styling fun
        EditorGUILayout.Separator();
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Add new translation:", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

        //Text fields for new phrase key and value
        phraseKey = EditorGUILayout.TextField("Phrase Identifier: ", phraseKey);
        phraseValue = EditorGUILayout.TextField("Phrase: ", phraseValue);

        EditorGUILayout.EndVertical();

        //add button to add what is in the phrase key and value fields to the languages
        if (GUILayout.Button("Add", new GUILayoutOption[] { GUILayout.Height(35), GUILayout.Width(50) }))
        {
            //Check if phrase key already exists
            if (LanguageHelper.CheckIfTranslationKeyExists(phraseKey))
            {
                EditorUtility.DisplayDialog("Phrase Identifier Exists!",
                    "The phrase Identifier already exists. Please enter a unique identifier.",
                    "OK");
            }
            else
            {
                //add phrase to all languages, with value for only selected language and clear text fields
                AddLanguageTranslation(phraseKey, phraseValue, languageOptions[selected]);
                phraseKey = string.Empty;
                phraseValue = string.Empty;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        EditorGUILayout.Separator();

        EditorGUILayout.HelpBox("If no language data exists you will need to create the asset. Click \"Assets->Create->Language Dictionary\" " +
                                "and ensure it is created in the \"Assets\"Resources\" folder and titled \"Data\" (if you look in " +
                                "windows explorer it will have the extension \".asset\")", MessageType.Info);


        EditorGUILayout.EndScrollView();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(Languages);
        }
    }

    //Functions for calling the language helper, seperated for ease of visualization

    private void CreateLanguage(string languageKey)
    {
        LanguageHelper.AddLanguage(languageKey);
    }

    private void DeleteLanguage(string languageKey)
    {
        LanguageHelper.RemoveLanguage(languageKey);
    }

    private void DeleteLanguageTranslation(string translationKey)
    {
        LanguageHelper.RemovePhraseFromLanguages(translationKey);
    }

    private void AddLanguageTranslation(string translationKey, string translation, string currentLanguage)
    {
        LanguageHelper.AddPhraseToLanguage(translationKey, translation, currentLanguage);
    }

    private void SetSelectedLanguage(string languageKey)
    {
        LanguageHelper.SetSelectedLanguage(languageKey);
        
    }

}
