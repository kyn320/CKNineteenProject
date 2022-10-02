using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SnapToFloor
{
    public class STFAbout : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset STFAboutUXML;
    
        [MenuItem("Window/SnapToFloor/About")]
        public static void Init()
        {
            STFAbout wnd = GetWindow<STFAbout>();
            wnd.titleContent = new GUIContent("About");
            wnd.minSize = new Vector2(350, 120);
            wnd.maxSize = new Vector2(350, 120);
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
        
            // Import UXML
            VisualTreeAsset visualTree = STFAboutUXML;
            VisualElement container = visualTree.Instantiate();
            root.Add(container);
        }
    }
}