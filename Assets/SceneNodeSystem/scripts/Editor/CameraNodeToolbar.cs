using System;
using UnityEditor;
using UnityEngine;
public class CameraNodeToolbar
{
	const int BOTTOM_TOOLBAR_HEIGHT = 20;
	public static CameraNodeEditorData editData;
	public void OnSceneGUI(SceneView sceneView){
		Event e = Event.current;
		if (e.type == EventType.Repaint || e.type == EventType.Layout)
		{
			OnRepaint(sceneView, e);
		}
	}
	private static void OnRepaint(SceneView sceneView, Event e)
	{
		Rect rectangle = new Rect(0, sceneView.position.height - BOTTOM_TOOLBAR_HEIGHT, sceneView.position.width, BOTTOM_TOOLBAR_HEIGHT);

		GUIStyle style = new GUIStyle(EditorStyles.toolbar);
		style.fixedHeight = BOTTOM_TOOLBAR_HEIGHT;
		GUILayout.Window(0, rectangle, OnBottomToolbarGUI, "", style);//, EditorStyles.textField);
	}
	private static void OnBottomToolbarGUI(int windowID)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label ("Camera Nodes");
		if (editData.selectedNode != null) {
			if (GUILayout.Button ("Add Node")) {
				// create a new node
				GameObject newNode = new GameObject ("new node");
				newNode.transform.position = editData.selectedNode.transform.position;
				CameraNode newCam = newNode.AddComponent<CameraNode> ();
				editData.selectedNode.connectedNodes.Add (newCam);
				newCam.connectedNodes.Add (editData.selectedNode);
				newNode.transform.SetParent (editData.selectedNode.transform);
				Selection.activeGameObject = newNode;
			}
			editData.selectedNode.walkable = GUILayout.Toggle (editData.selectedNode.walkable, "walkable");
		}
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal();
	}
	private static bool CheckKeyPress(KeyCode toCheck, EventModifiers modifiers = EventModifiers.None, EventType eventType = EventType.KeyDown){
		//		Debug.Log (Event.current.keyCode + " " + Event.current.modifiers + " " + Event.current.type);
		return Event.current.type == eventType && Event.current.modifiers == modifiers && Event.current.keyCode == toCheck;
	}

}