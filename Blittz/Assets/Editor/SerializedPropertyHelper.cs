using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class SerializedPropertyHelper
{
	public static readonly GUIContent emptyContent = new GUIContent(" ");

	public static object GetPropValue(SerializedProperty prop)
	{
		var parentValue = GetParent(prop);
		int arrayIndex = GetArrayIndex(prop);
		if (arrayIndex != -1)
			return GetArrayElement(parentValue, arrayIndex);
		return parentValue;
	}
	
	public static object GetParent(SerializedProperty prop)
	{
		var path = prop.propertyPath.Replace(".Array.data[", "[");
		object obj = prop.serializedObject.targetObject;
		var elements = path.Split('.');
		foreach (var element in elements)
		{
			if(element.Contains("["))
			{
				var elementName = element.Substring(0, element.IndexOf("["));
				obj = GetValue(obj, elementName);
				var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[","").Replace("]",""));
				obj = GetArrayElement(obj, index);
			}
			else
			{
				obj = GetValue(obj, element);
			}
		}
		return obj;
	}

	public static FieldInfo GetFieldInfo(SerializedProperty prop)
	{
		var path = prop.propertyPath.Replace(".Array.data[", "[");
		object obj = prop.serializedObject.targetObject;
		var elements = path.Split('.');
		FieldInfo fi = null;
		foreach(var element in elements)
		{
			if(element.Contains("["))
			{
				var elementName = element.Substring(0, element.IndexOf("["));
				var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[","").Replace("]",""));
				fi = GetFieldInfo(obj, elementName);
				obj = GetValue(obj, elementName, index);
			}
			else
			{
				fi = GetFieldInfo(obj, element);
				obj = GetValue(obj, element);
			}
		}
		return fi;
	}

	public static FieldInfo GetFieldInfo(object source, string name)
	{
		if(source == null)
			return null;
		var type = source.GetType();
		return type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
	}

	public static object GetValue(object source, string name)
	{
		var fi = GetFieldInfo(source, name);
		if(fi == null)
			return null;
		return fi.GetValue(source);
	}

	public static int GetArrayIndex(SerializedProperty prop)
	{
		int index = -1;
		var path = prop.propertyPath;
		if (path.Last<char>() == ']')
		{
			int rbInd = path.Length - 1;
			int lbInd = path.LastIndexOf("[");
			index = Convert.ToInt32(path.Substring(lbInd + 1, rbInd - lbInd - 1));
		}
		return index;
	}

	public static object GetValue(object source, string name, int index)
	{
		var enumerable = GetValue(source, name) as IEnumerable;
		if (enumerable == null)
			return null;
		int i = 0;
		foreach (var o in enumerable)
		{
			if (i == index)
				return o;
			i++;
		}
		return null;
	}
			
	public static object GetArrayElement(object source, int index)
	{
		var enumerable = source as IEnumerable;
		if (enumerable == null)
			return null;
		int i = 0;
		foreach (var o in enumerable)
		{
			if (i == index)
				return o;
			i++;
		}
		return null;
	}
			
	public static string[] GetChildFields(string[] fieldPaths, string parentField)
	{
		List<string> childFields = new List<string>();
		foreach (var fieldPath in fieldPaths)
		{
			if (string.Compare(parentField, fieldPath) == 0)
				return childFields.ToArray();
			else if (fieldPath.StartsWith(parentField + "."))
				childFields.Add(fieldPath.Substring(parentField.Length + 1));
		}
		return childFields.Count == 0 ? null : childFields.ToArray();
	}
}
