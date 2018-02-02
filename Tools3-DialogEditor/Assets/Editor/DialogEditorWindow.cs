using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DialogEditorWindow : EditorWindow
{

    public Vector2 scrollPos;
    public DialogDictionary DialogOptions;
    public string[] NextDialogOptions;



    [MenuItem("Window/Dialog Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        DialogEditorWindow window = (DialogEditorWindow)GetWindow(typeof(DialogEditorWindow));
        window.Show();
    }

    //Reload the resources on awake
    void Awake()
    {
        DialogOptions = Resources.Load<DialogDictionary>("DialogData");
    }

    void OnGUI()
    {
        if (DialogOptions == null)
        {
            Debug.Log("DialogDictionary is null");
            EditorGUILayout.HelpBox("If no dialog data exists you will need to create the asset. Click \"Assets->Create->Dialog Dictionary\" " +
                                    "and ensure it is created in the \"Assets\"Resources\" folder and titled \"DialogData\" (if you look in " +
                                    "windows explorer it will have the extension \".asset\"). You may have to restart unity.", MessageType.Info);
            return;
        }

        //Scrolling for if window is too large for screen space
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.Separator();
        

        NextDialogOptions = DialogOptions.DialogOptions.OrderBy(x => x.Index).Select(x => x.DisplayText).ToArray();
        //Dialog option

        int i = 1;
        foreach (var dialogOption in DialogOptions.DialogOptions)
        {
            EditorGUILayout.LabelField(string.Format("Dialog {0}:", i), EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Display Text:", GUILayout.Width(100));
            dialogOption.DisplayText = EditorGUILayout.TextField(string.Empty, dialogOption.DisplayText);
            EditorGUILayout.EndHorizontal();
            foreach (var respOption in dialogOption.PossibleResponses)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(string.Format("Response {0}:", dialogOption.PossibleResponses.IndexOf(respOption)), GUILayout.Width(100));
                EditorGUILayout.BeginVertical();
                respOption.ResposeText = EditorGUILayout.TextField(string.Empty, respOption.ResposeText);

                //respOption.NextDialogIndex = EditorGUILayout.IntField("Next Dialog", respOption.NextDialogIndex);
                if (!respOption.IsDialogEnd)
                {
                    respOption.NextDialogIndex =
                        EditorGUILayout.Popup("Next Dialog:", respOption.NextDialogIndex, NextDialogOptions);
                }

                respOption.IsDialogEnd = EditorGUILayout.Toggle("Ends dialog:", respOption.IsDialogEnd);
                EditorGUILayout.EndVertical();
                if (GUILayout.Button("X"))
                {
                    DialogHelper.RemoveResponseFromDialog(dialogOption.Index, respOption.ResponseId);
                    return;
                    //delete response option
                }
                EditorGUILayout.EndHorizontal();
                
            }
            EditorGUILayout.Separator();
            if (GUILayout.Button("Add Response"))
            {
                DialogHelper.AddNewResponseToDialog(dialogOption.Index);
                return;
            }
            if (GUILayout.Button(string.Format("Delete Dialog {0}", i)))
            {
                DialogHelper.RemoveDialog(dialogOption.Index);
                return;
            }
            i++;
            EditorGUILayout.Separator();
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
            EditorGUILayout.Separator();
        }
        

        //add new dialog
        if (GUILayout.Button("Add Dialog"))
        {
            DialogHelper.AddNewDialogOption();
            return;
        }


        EditorGUILayout.Separator();
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        EditorGUILayout.Separator();

        EditorGUILayout.HelpBox("If no dialog data exists you will need to create the asset. Click \"Assets->Create->Dialog Dictionary\" " +
                                "and ensure it is created in the \"Assets\"Resources\" folder and titled \"DialogData\" (if you look in " +
                                "windows explorer it will have the extension \".asset\")", MessageType.Info);

        EditorGUILayout.EndScrollView();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(DialogOptions);
        }
    }
}
