using UnityEngine;
using UnityEditor;
using System.IO;

namespace JGame.Localization
{
    public class LocalizedTextEditor : EditorWindow
    {
        protected static string folderPath = "";
        protected static string fileExtension = "json";

        public LocalizationData localizationData;

        [MenuItem("Window/Localized Text Editor")]
        static void Init()
        {
            folderPath = Application.dataPath + "/Resources/Localization";
            fileExtension = "json";

            EditorWindow.GetWindow(typeof(LocalizedTextEditor)).Show();
        }

        // draw ui
        private void OnGUI()
        {
            if (localizationData != null)
            {
                SerializedObject serializedObject = new SerializedObject(this);
                SerializedProperty serializedProperty = serializedObject.FindProperty("localizationData");
                EditorGUILayout.PropertyField(serializedProperty, true);
                serializedObject.ApplyModifiedProperties();

                // save data
                if (GUILayout.Button("Save data"))
                {
                    SaveGameData();
                }
            }

            // load localization data
            if (GUILayout.Button("Load data"))
            {
                LoadGameData();
            }

            // create new localization data
            if (GUILayout.Button("Create new data"))
            {
                CreateNewData();
            }
        }

        private void LoadGameData()
        {
            string filePath = EditorUtility.OpenFilePanel("Select localization data file", folderPath, fileExtension);

            if (!string.IsNullOrEmpty(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);

                localizationData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            }
        }

        private void SaveGameData()
        {
            string filePath = EditorUtility.SaveFilePanel("Save localization data file", folderPath, "", fileExtension);

            if (!string.IsNullOrEmpty(filePath))
            {
                string dataAsJson = JsonUtility.ToJson(localizationData);
                File.WriteAllText(filePath, dataAsJson);
            }
        }

        private void CreateNewData()
        {
            localizationData = new LocalizationData();
        }
    }
}