using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DialogOption
{
    //basic constructor
    public DialogOption(int index)
    {
        Index = index;
        DisplayText = string.Empty;
        PossibleResponses = new List<ResponseOption>();
    }
    public string DisplayText;
    public int Index;
    public List<ResponseOption> PossibleResponses;

    public void AddResponse()
    {
        //create if its null
        if(PossibleResponses==null)
            PossibleResponses = new List<ResponseOption>();

        //new index will be zero only if no responses exist
        int nextId = 0;

        //if there are other responses
        if (PossibleResponses.Count > 0)
        {
            //get the response with the highest index
            var lastResponse = PossibleResponses.OrderBy(x => x.ResponseId).LastOrDefault();
            //set index to highest + 1
            nextId = lastResponse.ResponseId + 1;
        }

        PossibleResponses.Add(new ResponseOption(nextId));
    }

    public void RemoveResponse(int responseId)
    {
        if (PossibleResponses == null)
            return;
        //get the response to remove and remove it
        var toRemove = PossibleResponses.FirstOrDefault(x => x.ResponseId== responseId);
        PossibleResponses.Remove(toRemove);
        
        //check for any higher indexes than the one removed and decrement any that exist
        var higherIds = PossibleResponses.Where(x => x.ResponseId > responseId);
        foreach (var resp in higherIds)
        {
            resp.ResponseId--;
        }
        
    }
}
