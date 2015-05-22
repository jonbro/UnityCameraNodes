using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadScene : NodeBehaviour {
	public string 	levelName;
	void Start(){
		DontDestroyOnLoad (gameObject);
	}
	override public void OnEnterNode(){
		// unload the current level
		Application.LoadLevel(levelName);
	}
}
