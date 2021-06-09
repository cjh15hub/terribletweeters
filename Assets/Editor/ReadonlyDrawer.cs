using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InspectorReadonlyAttribute))]
public class ReadonlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string asString;

        switch(property.propertyType)
        {
            case SerializedPropertyType.Integer:
                asString = property.intValue.ToString();
                break;
            case SerializedPropertyType.Float:
                asString = property.floatValue.ToString("0.0000");
                break;
            case SerializedPropertyType.Boolean:
                asString = property.boolValue.ToString();
                break;
            case SerializedPropertyType.String:
            case SerializedPropertyType.Character:
                asString = property.stringValue;
                break;
            case SerializedPropertyType.Vector2:
                asString = property.vector2Value.ToString();
                break;
            case SerializedPropertyType.Vector3:
                asString = property.vector3Value.ToString();
                break;
            case SerializedPropertyType.ObjectReference:
                if(property.objectReferenceValue)
                {
                    asString = property.objectReferenceValue.ToString();
                }
                else
                {
                    asString = "None (Game Object)";
                }
                break;
            default:
                asString = "No Custom Drawer Available";
                break;
        }

        EditorGUI.LabelField(position, label.text, asString);
    }
}
