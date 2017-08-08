﻿using UnityEngine;
using UnityEditor;
using System.IO;

namespace JGame.Map
{
    public class MapEditor : EditorWindow
    {
        Vector2 scrollPos;
        protected static string folderPath = "";
        protected static string fileExtension = "json";

        public MapData          mapData;

        [MenuItem("Window/Map Editor")]
        static void Init()
        {
            folderPath = Application.dataPath + "/Resources/Map";
            fileExtension = "json";

            EditorWindow.GetWindow(typeof(MapEditor)).Show();
        }

        // draw ui
        private void OnGUI()
        {
            if (mapData != null)
            {
                if (mapData.datas != null && mapData.datas.Length != (mapData.tileX * mapData.tileY))
                {
                    var newDatas = new int[mapData.tileX * mapData.tileY];
                    var length = Mathf.Min(newDatas.Length, mapData.datas.Length);
                    for(int i=0;i< length; ++i)
                    {
                        newDatas[i] = mapData.datas[i];
                    }                    
                    mapData.datas = newDatas;
                }

                // begin scroll
                EditorGUILayout.BeginVertical();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                SerializedObject serializedObject = new SerializedObject(this);
                SerializedProperty serializedProperty = serializedObject.FindProperty("mapData");
                EditorGUILayout.PropertyField(serializedProperty, true);
                serializedObject.ApplyModifiedProperties();

                // end scroll
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                // save data
                if (GUILayout.Button("Preview"))
                {
                    Preview();
                }

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

        private void Preview()
        {
            GameObject mapObj = GameObject.Find("Map");

            if( mapObj == null )
            {
                Debug.LogError("Need map Object.");
                return;
            }

            while(mapObj.transform.childCount > 0 )
            {
                var obj = mapObj.transform.GetChild(0);
                DestroyImmediate(obj.gameObject);
            }

            Map map = mapObj.GetComponent<Map>();

            map.Initialize(mapData);
        }

        private void LoadGameData()
        {
            string filePath = EditorUtility.OpenFilePanel("Select localization data file", folderPath, fileExtension);

            if (!string.IsNullOrEmpty(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);

                mapData = JsonUtility.FromJson<MapData>(dataAsJson);
            }
        }

        private void SaveGameData()
        {
            string filePath = EditorUtility.SaveFilePanel("Save localization data file", folderPath, "", fileExtension);

            if (!string.IsNullOrEmpty(filePath))
            {
                string dataAsJson = JsonUtility.ToJson(mapData);
                File.WriteAllText(filePath, dataAsJson);
            }
        }

        private void CreateNewData()
        {
            mapData = new MapData();
        }
    }
}
