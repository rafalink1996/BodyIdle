using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;


[CustomEditor(typeof(ButtonHold))]
public class ButtonHoldEditor : UnityEditor.UI.ButtonEditor
// Start is called before the first frame update
{
    public override void OnInspectorGUI()
    {

        ButtonHold component = (ButtonHold)target;

        base.OnInspectorGUI();

        component.useColor = EditorGUILayout.Toggle("Use Color", component.useColor);

        component.Expand = EditorGUILayout.Toggle("Expand", component.Expand);
        component.expandAmount = EditorGUILayout.FloatField("Expand amount", component.expandAmount);

        component.perecentThreshold = EditorGUILayout.FloatField("Percent Threshold", component.perecentThreshold);
        component.requierdHoldTime = EditorGUILayout.FloatField("Require Hold time", component.requierdHoldTime);

        //component.OnLongClick = EditorGUILayout.PropertyField("On Long Click");

        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnLongClick"), true);
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnShortClick"), true);
        this.serializedObject.ApplyModifiedProperties();
    }
}
