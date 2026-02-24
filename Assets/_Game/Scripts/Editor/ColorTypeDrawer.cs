using UnityEditor;
using UnityEngine;
using _Game.LevelSystem;

namespace _Game.Editor
{
    [CustomPropertyDrawer(typeof(ColorType))]
    public class ColorTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Color originalColor = GUI.color;
            
            ColorType colorType = (ColorType)property.enumValueIndex;
            GUI.color = GetColorFromType(colorType);
            
            EditorGUI.PropertyField(position, property, label);
            
            GUI.color = originalColor;
        }

        private Color GetColorFromType(ColorType colorType)
        {
            switch (colorType)
            {
                case ColorType._1Red:
                    return Color.red;
                case ColorType._2Blue:
                    return Color.blue;
                case ColorType._3Green:
                    return Color.green;
                case ColorType._4Yellow:
                    return Color.yellow;
                case ColorType._5Orange:
                    return new Color(1f, 0.5f, 0f);
                case ColorType._6Purple:
                    return new Color(0.5f, 0f, 1f);
                case ColorType._7Pink:
                    return new Color(1f, 0.4f, 0.7f);
                case ColorType._0None:
                default:
                    return Color.white;
            }
        }
    }
}
