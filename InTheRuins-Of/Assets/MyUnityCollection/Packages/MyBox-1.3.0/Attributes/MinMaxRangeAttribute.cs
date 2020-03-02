// ---------------------------------------------------------------------------- 
// Author: Richard Fine
// Source: https://bitbucket.org/richardfine/scriptableobjectdemo
// ----------------------------------------------------------------------------

using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace MyBox {
  public class MinMaxRangeAttribute : PropertyAttribute {
    public MinMaxRangeAttribute(float min, float max) {
      this.min = min;
      this.max = max;
    }

    public readonly float min;
    public readonly float max;
  }

  [Serializable]
  public struct FloatRange {
    public float min;
    public float max;

    public FloatRange(float min, float max) {
      this.min = min;
      this.max = max;
    }
  }

  [Serializable]
  public struct IntRange {
    public int min;
    public int max;

    public IntRange(int min, int max) {
      this.min = min;
      this.max = max;
    }
  }

  public static class RangedExtensions {
    public static float Lerp(this FloatRange ranged, float t) => Mathf.Lerp(ranged.min, ranged.max, t);
    public static float LerpUnclamped(this FloatRange ranged, float t) => Mathf.LerpUnclamped(ranged.min, ranged.max, t);

    public static float Lerp(this IntRange ranged, float t) => Mathf.Lerp(ranged.min, ranged.max, t);
    public static float LerpUnclamped(this IntRange ranged, float t) => Mathf.LerpUnclamped(ranged.min, ranged.max, t);
  }
}

#if UNITY_EDITOR
namespace MyBox.Internal {
  [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
  public class MinMaxRangeIntAttributeDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      SerializedProperty xProp = property.FindPropertyRelative("x") ?? property.FindPropertyRelative("min");
      SerializedProperty yProp = property.FindPropertyRelative("y") ?? property.FindPropertyRelative("max");
      if (xProp == null || yProp == null) {
        WarningsPool.Log("MinMaxRangeAttribute used on <color=brown>" +
                         property.name +
                         "</color>. Must be used on types with x/min and y/max fields",
          property.serializedObject.targetObject);

        return;
      }

      var minValid = xProp.propertyType == SerializedPropertyType.Integer || xProp.propertyType == SerializedPropertyType.Float;
      var maxValid = yProp.propertyType == SerializedPropertyType.Integer || yProp.propertyType == SerializedPropertyType.Float;
      if (!maxValid || !minValid || xProp.propertyType != yProp.propertyType) {
        WarningsPool.Log("MinMaxRangeAttribute used on <color=brown>" +
                         property.name +
                         "</color>. x/min and y/max fields must be of int or float type",
          property.serializedObject.targetObject);

        return;
      }

      MinMaxRangeAttribute rangeAttribute = (MinMaxRangeAttribute)attribute;

      label = EditorGUI.BeginProperty(position, label, property);
      position = EditorGUI.PrefixLabel(position, label);

      bool isInt = xProp.propertyType == SerializedPropertyType.Integer;

      float xValue = isInt ? xProp.intValue : xProp.floatValue;
      float yValue = isInt ? yProp.intValue : yProp.floatValue;
      float rangeMin = rangeAttribute.min;
      float rangeMax = rangeAttribute.max;


      const float rangeBoundsLabelWidth = 40f;

      var rangeBoundsLabel1Rect = new Rect(position);
      rangeBoundsLabel1Rect.width = rangeBoundsLabelWidth;
      GUI.Label(rangeBoundsLabel1Rect, new GUIContent(xValue.ToString(isInt ? "F0" : "F2")));
      position.xMin += rangeBoundsLabelWidth;

      var rangeBoundsLabel2Rect = new Rect(position);
      rangeBoundsLabel2Rect.xMin = rangeBoundsLabel2Rect.xMax - rangeBoundsLabelWidth;
      GUI.Label(rangeBoundsLabel2Rect, new GUIContent(yValue.ToString(isInt ? "F0" : "F2")));
      position.xMax -= rangeBoundsLabelWidth;

      EditorGUI.BeginChangeCheck();
      EditorGUI.MinMaxSlider(position, ref xValue, ref yValue, rangeMin, rangeMax);

      if (EditorGUI.EndChangeCheck()) {
        if (isInt) {
          xProp.intValue = Mathf.RoundToInt(xValue);
          yProp.intValue = Mathf.RoundToInt(yValue);
        } else {
          xProp.floatValue = xValue;
          yProp.floatValue = yValue;
        }
      }

      EditorGUI.EndProperty();
    }
  }
}
#endif