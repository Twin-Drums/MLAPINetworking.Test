using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.SceneManagement;

namespace TwinDrums.Utilities.Prefabs
{
    public class ScatterPrefabs : EditorWindow
    {
        private const int DefaultSpacePx = 10;
        private Object prefab;        
        private bool applyScaling;
        private int amount;
        private bool anchorToMesh = true;
        private Bounds bounds;

        [MenuItem("Twindrums/Prefabs/Scatter Prefabs")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ScatterPrefabs));
        }

        private void OnEnable()
        {
            
        }

        private void OnGUI()
        {
            GUILayout.Label("Scatter Prefabs", EditorStyles.boldLabel);

            GUILayout.Space(DefaultSpacePx);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Prefab: ", EditorStyles.label);
            prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), true);
            GUILayout.EndHorizontal();

            GUILayout.Space(DefaultSpacePx);

            amount = EditorGUILayout.IntField("Amount:", amount);

            GUILayout.Space(DefaultSpacePx);

            anchorToMesh = EditorGUILayout.Toggle("Anchor to Mesh:", anchorToMesh);

            bounds = EditorGUILayout.BoundsField("Bounds:", bounds);

            GUILayout.Space(DefaultSpacePx);

            if (GUILayout.Button("Scatter"))
            {
                if (Selection.gameObjects == null)
                    return;


                for(int i = 0; i < amount; i++)
                {
                    var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;                    

                    if(Selection.activeObject != null && Selection.activeObject is GameObject)
                    {
                        go.transform.SetParent((Selection.activeObject as GameObject).transform);
                    }

                    go.transform.position = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));

                    if(anchorToMesh)
                    {
                        RaycastHit rayhit;
                        if (Physics.Raycast(go.transform.position, Vector3.down, out rayhit))
                        {
                            go.transform.position = rayhit.point;
                        }
                    }
                                     
                }

                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
}

