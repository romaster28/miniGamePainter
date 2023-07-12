using Sources.Core.Map;
using UnityEditor;
using UnityEngine;

namespace Sources.Editor
{
    public class CellsCreatorWindow : EditorWindow
    {
        private static int _width;

        private static int _height;

        private static Vector2 _start;
        
        private static Vector2 _offset;
        
        private const int MinValue = 1;

        private static GameObject _prefab;
        
        [MenuItem("Tools/Create Cells")]
        private static void ShowWindow()
        {
            var window = GetWindow<CellsCreatorWindow>();
            window.titleContent = new GUIContent("Create cells");
            window.Show();
        }

        private void OnGUI()
        {
            _prefab = EditorGUILayout.ObjectField("Prefab", _prefab, typeof(GameObject), false) as GameObject;

            _width = EditorGUILayout.IntField("Width", _width);

            if (_width < MinValue)
                _width = MinValue;

            _height = EditorGUILayout.IntField("Height", _height);

            if (_height < MinValue)
                _height = MinValue;

            _start = EditorGUILayout.Vector2Field("Start", _start);

            _offset = EditorGUILayout.Vector2Field("Offset", _offset);

            if (GUILayout.Button("Create"))
            {
                CreateCells();
            }
        }

        private static void CreateCells()
        {
            Vector2 current = _start;
            
            Transform parent = new GameObject("Cells").transform;

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    CreateCell($"{x}_{y}", current, parent);
                    
                    current.x += _offset.x;
                }

                bool isVerticalEven = y % 2 == 0;
                
                current.x = !isVerticalEven ? _start.x : _start.x + _offset.x / 2;
                
                current.y -= _offset.y;
            }
        }

        private static Cell CreateCell(string name, Vector2 position, Transform parent)
        {
            GameObject created = PrefabUtility.InstantiatePrefab(_prefab) as GameObject;

            created.name = name;

            Transform createdTransform = created.transform;
            
            createdTransform.position = position;

            createdTransform.parent = parent;

            return created.GetComponent<Cell>();
        }
    }
}