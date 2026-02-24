using UnityEngine;
using UnityEditor;
using UnityToolbarExtender;

namespace _Game.Editor
{
    [InitializeOnLoad]
    public class TimeScaleController
    {
        private static float _timeScaleValue = 1.0f;
        private static bool _useStepValues = false;
        private const float MinTimeScale = 0.01f;
        private const float MaxTimeScale = 10f;
        private const float SliderWidth = 150f;
        private const float LabelWidth = 70f;
        private const float StepSize = 0.5f;
        
        static TimeScaleController()
        {
            ToolbarExtender.LeftToolbarGUI.Add(DrawTimeScaleSlider);
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            _timeScaleValue = EditorPrefs.GetFloat("SavedTimeScale", 1.0f);
            _useStepValues = EditorPrefs.GetBool("UseTimeScaleSteps", false);
        }
        
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                Time.timeScale = _timeScaleValue;
            }
        }
        
        private static void DrawTimeScaleSlider()
        {
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Label("Time Scale:", GUILayout.Width(LabelWidth));
            
            EditorGUI.BeginChangeCheck();
            _timeScaleValue = GUILayout.HorizontalSlider(
                _timeScaleValue, 
                MinTimeScale, 
                MaxTimeScale, 
                GUILayout.Width(SliderWidth)
            );
            
            if (_useStepValues)
            {
                _timeScaleValue = RoundToNearestStep(_timeScaleValue, StepSize);
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                UpdateTimeScale();
            }
            
            GUILayout.Label(_timeScaleValue.ToString("F2"), GUILayout.Width(35));
            
            EditorGUI.BeginChangeCheck();
            _useStepValues = GUILayout.Toggle(_useStepValues, "0.5 Steps", GUILayout.Width(80));
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool("UseTimeScaleSteps", _useStepValues);
                
                if (_useStepValues)
                {
                    _timeScaleValue = RoundToNearestStep(_timeScaleValue, StepSize);
                    UpdateTimeScale();
                }
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private static float RoundToNearestStep(float value, float step)
        {
            return Mathf.Round(value / step) * step;
        }
        
        private static void UpdateTimeScale()
        {
            EditorPrefs.SetFloat("SavedTimeScale", _timeScaleValue);
            
            if (EditorApplication.isPlaying)
            {
                Time.timeScale = _timeScaleValue;
            }
        }
    }
}