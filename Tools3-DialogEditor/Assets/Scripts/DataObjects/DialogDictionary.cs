using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Dialog Dictionary", order = 1)]
public class DialogDictionary : ScriptableObject
{
    //the list of dialogs we will be accessing
    public List<DialogOption> DialogOptions = new List<DialogOption>();

    public void AddNewDialogOption()
    {
        //if its the first dialog the index will be 0
        int index = 0;

        //but if another dialog exists the index will be the highes current index +1
        var lastItem = DialogOptions.OrderBy(x => x.Index).LastOrDefault();
        if (lastItem != null)
            index = lastItem.Index + 1;
        //call dialog constructor and call the dialog adding function
        var newDialog = new DialogOption(index);
        DialogOptions.Add(newDialog);
    }

    public void RemoveDialog(int DialogIndex)
    {
        //find the dialog to remove
        var toRemove = DialogOptions.FirstOrDefault(x => x.Index == DialogIndex);

        if (toRemove == null)
            return;

        int removedIndex = toRemove.Index;

        //remove it from the list
        DialogOptions.Remove(toRemove);

        //for all the dialogs that had a higher index we need to decrement there indexes
        foreach (var dialog in DialogOptions)
        {
            //check for less and then decrement
            if(dialog.Index>removedIndex)
                dialog.Index--;

            //also need to lower all responses that were meant to link to one above and reset any set to that dialog to the 0 default
            foreach (var posResp in dialog.PossibleResponses)
            {
                if (posResp.NextDialogIndex > removedIndex)
                    posResp.NextDialogIndex--;
                else if (posResp.NextDialogIndex == removedIndex)
                    posResp.NextDialogIndex = 0;
            }
        }
    }

    public void AddResponseToDialog(int dialogIndex)
    {
        //get the dialog of that index
        var dialog = DialogOptions.FirstOrDefault(x => x.Index == dialogIndex);

        if (dialog == null)
            return;
        //add a response
        dialog.AddResponse();

    }

    public void RemoveResponseFromDialog(int dialogIndex, int responseId)
    {
        //get the specified dialog
        var dialog = DialogOptions.FirstOrDefault(x => x.Index == dialogIndex);

        if (dialog == null)
            return;

        //remove the response
        dialog.RemoveResponse(responseId);
    }

    public string GetFirstDialogAndResponses()
    {
        //get dialog at index 0
        var dialog = DialogOptions.FirstOrDefault(x => x.Index == 0);
        if (dialog == null)
            return "No dialog found";

        //translate the dialog main message
        string result = LanguageHelper.ReplacePhraseWithTranslation(dialog.DisplayText);

        int i = 1;
        foreach (var respOption in dialog.PossibleResponses.OrderBy(x=>x.ResponseId))
        {
            //add a new line
            result += "\n";
            //translate the response
            result += string.Format("{0}){1}",i, LanguageHelper.ReplacePhraseWithTranslation(respOption.ResposeText));
            i++;
        }
        //return dialog main message and response options
        return result;
    }

    public int GetNextDialogIndex(int previousDialogIndex, int dialogResponse)
    {
        //get the previous dialog
        var dialog = DialogOptions.FirstOrDefault(x => x.Index == previousDialogIndex);
        if (dialog == null)
            return -1;
        //get the chosen response
        var response = dialog.PossibleResponses.FirstOrDefault(x => x.ResponseId == dialogResponse);

        if (response == null)
            return -1;

        //if it is meant to end the convo return -2
        if (response.IsDialogEnd)
            return -2;

        //get next dialog based on response
        var nextDialog = DialogOptions.FirstOrDefault(x => x.Index == response.NextDialogIndex);

        if (nextDialog == null)
            return -1;

        //return the index of the next dialog
        return nextDialog.Index;
    }

    public string GetCurrentDialogAndResponses(int currentDialogIndex)
    {
        //get the selected dialog
        var dialog = DialogOptions.FirstOrDefault(x => x.Index == currentDialogIndex);
        if (dialog == null)
            return "Dialog not found";

        //get main dialog message
        string result = LanguageHelper.ReplacePhraseWithTranslation(dialog.DisplayText);

        int i = 1;
        foreach (var respOption in dialog.PossibleResponses.OrderBy(x=>x.ResponseId))
        {
            //add new line to message
            result += '\n';
            //add response option to message
            result += string.Format("{0}){1}", i, LanguageHelper.ReplacePhraseWithTranslation(respOption.ResposeText));
            i++;
        }
        return result;
    }
}
