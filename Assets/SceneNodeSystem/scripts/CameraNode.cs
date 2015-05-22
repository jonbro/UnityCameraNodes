using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNode : MonoBehaviour {
	public List<CameraNode> connectedNodes = new List<CameraNode>();
	public bool walkable = true;
}
