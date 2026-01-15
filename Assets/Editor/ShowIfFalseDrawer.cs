#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfFalseAttribute))]
public class ShowIfFalseDrawer : PropertyDrawer{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
		var attr = (ShowIfFalseAttribute)attribute;
		SerializedProperty boolProp = property.serializedObject.FindProperty(attr.boolFieldName);
		// 当 bool 为 false 时显示
		if(boolProp != null && !boolProp.boolValue){
			EditorGUI.PropertyField(position, property, label);
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
		var attr = (ShowIfFalseAttribute)attribute;
		SerializedProperty boolProp = property.serializedObject.FindProperty(attr.boolFieldName);

		if(boolProp != null && !boolProp.boolValue){
			return EditorGUI.GetPropertyHeight(property, label);
		}

		return 0; // 隐藏时高度为0
	}
}

[CustomPropertyDrawer(typeof(ShowIfTrueAttribute))]
public class ShowIfTrueDrawer : PropertyDrawer{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
		var attr = (ShowIfTrueAttribute)attribute;
		SerializedProperty boolProp = property.serializedObject.FindProperty(attr.boolFieldName);
		// 当 bool 为 false 时显示
		if(boolProp != null && boolProp.boolValue){
			EditorGUI.PropertyField(position, property, label);
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
		var attr = (ShowIfTrueAttribute)attribute;
		SerializedProperty boolProp = property.serializedObject.FindProperty(attr.boolFieldName);

		if(boolProp != null && boolProp.boolValue){
			return EditorGUI.GetPropertyHeight(property, label);
		}

		return 0; // 隐藏时高度为0
	}
}
#endif