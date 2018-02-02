using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogHelper
{
    public static DialogDictionary DialogOptions;

    //loads the language data to the language dictionary
    static DialogHelper()
    {
        DialogOptions = Resources.Load<DialogDictionary>("DialogData");
    }

    //basic helper functions for accessing dialog dictionary
    public static void AddNewDialogOption()
    {
        DialogOptions.AddNewDialogOption();
    }

    public static void RemoveDialog(int dialogIndex)
    {
        DialogOptions.RemoveDialog(dialogIndex);
    }

    public static void AddNewResponseToDialog(int dialogIndex)
    {
        DialogOptions.AddResponseToDialog(dialogIndex);
    }

    public static void RemoveResponseFromDialog(int dialogIndex, int responseId)
    {
        DialogOptions.RemoveResponseFromDialog(dialogIndex, responseId);
    }

    public static string HandleResponse(int currentDialog)
    {
        if (currentDialog == -2)
        {
            return LanguageHelper.ReplacePhraseWithTranslation("EndPhrase");
        }
        string result = DialogOptions.GetCurrentDialogAndResponses(currentDialog);

        return result;
    }

    public static int GetNextDialogIndex(int prevDialog, int responseNumber)
    {
        return DialogOptions.GetNextDialogIndex(prevDialog, responseNumber);
    }

    public static string GetFirstDialog()
    {
        string result = DialogOptions.GetFirstDialogAndResponses();


        return result;
    }


}
