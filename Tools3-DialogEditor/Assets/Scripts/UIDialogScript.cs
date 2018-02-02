using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDialogScript : MonoBehaviour {
    //for number entry
    private KeyCode[] keyCodes = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
    };

    public Text myText;

    //i want to track the dialog im on
    public int currentDialog;

    // Use this for initialization
    void Awake () {
        myText = GetComponent<Text>();
        //get the first dialog in the editor to display
        myText.text = DialogHelper.GetFirstDialog();
        currentDialog = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if(myText==null)
            myText = GetComponent<Text>();

        for (int i = 0; i < keyCodes.Length; i++)
	    {
	        if (Input.GetKeyDown(keyCodes[i]))
	        {
                //this is the returned index value when the conversation is ended
	            if (currentDialog == -2)
	            {
	                myText.text = LanguageHelper.ReplacePhraseWithTranslation("RestartPhrase");
	                currentDialog = -3;
                }
                //for restarting conversation
                else if (currentDialog == -3)
	            {
	                currentDialog = 0;
	                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	            }
                //otherwise we get the response number, ask the dialog helper what our next dialog is
                //and then ask for the text of that dialog
	            else
	            {
	                int responseNum = i; //not i + 1 because our responses are indexed starting at 0 :)
	                int checkCurrentDialog = DialogHelper.GetNextDialogIndex(currentDialog, responseNum);
	                if (checkCurrentDialog == -1)
	                {
	                    myText.text += string.Format("\n{0}",
	                        LanguageHelper.ReplacePhraseWithTranslation("InvalidPhrase"));
	                }
	                else
	                {
	                    currentDialog = checkCurrentDialog;

                        myText.text = DialogHelper.HandleResponse(currentDialog);
	                }
	            }

	        } 
	    }

    }
}
