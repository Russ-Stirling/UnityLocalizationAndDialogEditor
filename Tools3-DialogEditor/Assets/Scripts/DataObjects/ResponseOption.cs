using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResponseOption
{
    //basic constructor
    public ResponseOption(int responseId)
    {
        ResposeText = string.Empty;
        NextDialogIndex = 0;
        IsDialogEnd = false;
        ResponseId = responseId;
    }
    //for the response text that will display
    public string ResposeText;

    //for tracking the next dialog
    public int NextDialogIndex;

    //for ending the convo with that option
    public bool IsDialogEnd;

    //for finding response
    public int ResponseId;
}
