using UnityEngine;
using System.Collections;

public class NodeCamera : MonoBehaviour {
	public CameraNode cameraNodeRoot;
	public CameraNode currentNode;
	public int currentLookIndex = 0;
	Vector3 targetLookVector, currentLookVector;
	// Use this for initialization
	public bool moving = false;
	public bool allowTurn = true;
	IEnumerator visionMover;
	void Start () {
		SetNode (cameraNodeRoot);
	}
	Vector3 lookVectorToV3(Vector2 lookVector){
		return new Vector3 (lookVector.x, 0, lookVector.y).normalized;
	}
	// Update is called once per frame
	void Update () {
		if (!currentNode)
			return;
		camera.transform.LookAt (currentLookVector, Vector3.up);
		if (Input.GetKeyDown (KeyCode.RightArrow) && allowTurn) {
			currentLookIndex = (currentLookIndex + 1) % currentNode.connectedNodes.Count;
			SetLookAt (currentLookIndex);
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow) && allowTurn) {
			currentLookIndex = (currentLookIndex - 1);
			while (currentLookIndex < 0)
				currentLookIndex += currentNode.connectedNodes.Count;
			SetLookAt (currentLookIndex);
		}
		if (Input.GetKey (KeyCode.UpArrow) && currentNode.connectedNodes[currentLookIndex].walkable && !moving) {
			visionMover = MoveForward ();
			StartCoroutine (visionMover);
		}
		Debug.DrawLine (targetLookVector, currentNode.transform.position);
		currentLookVector = Vector3.Lerp (currentLookVector, targetLookVector, 5f*Time.deltaTime);
	}
	public void SetLookAt(int lookTarget, bool immediate = false){
		currentLookIndex = lookTarget;
		targetLookVector = currentNode.connectedNodes[currentLookIndex].transform.position;
		if (immediate)
			currentLookVector = targetLookVector;
		// clear all existing text
		foreach (NodeBehaviour nb in currentNode.connectedNodes[currentLookIndex].GetComponents<NodeBehaviour>()) {
			nb.OnLookAtNode ();
		}
	}
	public void SetNode(CameraNode targetNode){
		currentNode = cameraNodeRoot = targetNode;
		transform.position = currentNode.transform.position;
		targetLookVector = currentLookVector = cameraNodeRoot.connectedNodes[currentLookIndex].transform.position;
	}
	IEnumerator MoveForward(){
		moving = true;
		Vector3 startPosition = transform.position;
		Vector3 targetPosition = currentNode.connectedNodes[currentLookIndex].transform.position;
		currentNode = currentNode.connectedNodes[currentLookIndex];
		// find the nearest node to where we are already looking, and bind the camera to that
		Vector3 lookDirection = (targetPosition - startPosition).normalized;
		int lowestLookIndex = 0;
		float lowestLookDistance = -1*Vector3.Dot (lookDirection, (currentNode.connectedNodes[0].transform.position-currentNode.transform.position).normalized);
		for (int i = 0; i < currentNode.connectedNodes.Count; i++) {
			float lookDistance = -1*Vector3.Dot (lookDirection, (currentNode.connectedNodes[i].transform.position-currentNode.transform.position).normalized);
			if (lookDistance < lowestLookDistance) {
				lowestLookDistance = lookDistance;
				lowestLookIndex = i;
			}
		}
		SetLookAt (lowestLookIndex);
		foreach (NodeBehaviour nb in currentNode.GetComponents<NodeBehaviour>()) {
			nb.OnEnterNode ();
		}
		float startTime = Time.time;
		while (Time.time < startTime + 1.0f) {
			float percentage = Time.time - startTime;
			transform.position = Vector3.Lerp (startPosition, targetPosition, percentage);
			yield return new WaitForEndOfFrame ();
		}
		transform.position = targetPosition;
		moving = false;
		yield return null;
	}
	void OnLevelWasLoaded(int level) {
		moving = false;
		if(visionMover != null)
			StopCoroutine (visionMover);
	}
}
