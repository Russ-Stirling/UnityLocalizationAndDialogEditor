using UnityEngine;
using UnityEngine.UI;

public class UITextReplacementScript : MonoBehaviour
{
    void Awake()
    {
        //Script is attached to the parent object of all UI text objects (the canvas) so it gets them all at once
        var myTextArray = this.GetComponentsInChildren<Text>();

        foreach (var textInstance in myTextArray)
        {
            //only replace text enclosed in [!text here!]
            if (textInstance.text.StartsWith("[!") && textInstance.text.EndsWith("!]"))
            {
                textInstance.text = LanguageHelper.ReplacePhraseWithTranslation(
                    textInstance.text.Replace("[!", string.Empty).Replace("!]", string.Empty));
            }
        }

    }
}
