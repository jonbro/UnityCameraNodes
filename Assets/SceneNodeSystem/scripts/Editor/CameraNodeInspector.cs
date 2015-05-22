using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;

[CustomEditor(typeof(CameraNode))]
public class CameraNodeInspector : Editor {
	Tool LastTool = Tool.None;
	float buttonSize = 0.04f;
	private const float handleSize = 0.04f;
	private const float pickSize = 0.06f;

	CameraNodeEditorData editData;
	CameraNodeToolbar toolbar;
	private ReorderableList list;
	[MenuItem("GameObject/Camera Nodes/New Camera Node", false, 10)]

	static void CreateCameraNode(MenuCommand menuCommand) {
		// Create a custom game object
		GameObject go = new GameObject("Camera Node");
		CameraNode cameraNode = go.AddComponent<CameraNode> ();
		// Ensure it gets reparented if this was a context click (otherwise does nothing)
		GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
		// Register the creation in the undo system
		Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
		Selection.activeObject = go;
	}

	void Awake(){
		editData = new CameraNodeEditorData ();
		editData.selectedNode = target as CameraNode;
		CameraNodeToolbar.editData = editData;
		toolbar = new CameraNodeToolbar ();
	}
	void OnEnable(){
		list = new ReorderableList(serializedObject, 
			serializedObject.FindProperty("connectedNodes"), 
			true, true, true, true);
		list.drawElementCallback =  
			(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;
			EditorGUI.ObjectField(
				new Rect(rect.x, rect.y, rect.width-30, EditorGUIUtility.singleLineHeight),
				element, GUIContent.none);
		};

		if (toolbar == null)
			toolbar = new CameraNodeToolbar ();
		editData = new CameraNodeEditorData ();
		editData.selectedNode = target as CameraNode;
		CameraNodeToolbar.editData = editData;
		SceneView.onSceneGUIDelegate += toolbar.OnSceneGUI;
	}
	public override void OnInspectorGUI() {
		(target as CameraNode).walkable = GUILayout.Toggle ((target as CameraNode).walkable, "walkable");
		serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}
	void OnDestroy() {
		// When the window is destroyed, remove the delegate
		// so that it will no longer do any drawing.
		SceneView.onSceneGUIDelegate -= toolbar.OnSceneGUI;
	}
	private void OnSceneGUI () {
		int controlID = GUIUtility.GetControlID (FocusType.Passive);
		CameraNode cameraNode = target as CameraNode;
		DrawCameraNode (cameraNode);
	}
	void DrawCameraNode(CameraNode root){
		root.transform.position = Handles.DoPositionHandle (root.transform.position, Quaternion.identity);
		foreach (CameraNode cn in root.connectedNodes) {
			Handles.DrawDottedLine (root.transform.position, cn.transform.position, 1);
			Vector3 point = cn.transform.position;
			float size = HandleUtility.GetHandleSize(point);
			if(Handles.Button(cn.transform.position, Quaternion.identity, size*handleSize, size*pickSize, Handles.DotCap)){
				// set this camera object as selected
				Selection.activeGameObject = cn.gameObject;
			}
		}
	}
}
