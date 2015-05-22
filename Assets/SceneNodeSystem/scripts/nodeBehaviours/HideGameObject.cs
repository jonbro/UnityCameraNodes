using UnityEngine;
using System.Collections;

public class HideGameObject : NodeBehaviour {
	public GameObject objectToHide;
	override public void OnLookAtNode(){
		objectToHide.SetActive (false);
	}
}
