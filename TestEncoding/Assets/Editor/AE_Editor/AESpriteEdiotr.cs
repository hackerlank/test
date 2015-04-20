////////////////////////////////////////////////////////////////////////////////
//  
// @module <module_name>
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(IAESpriteRenderer))]
public class AESpriteEdiotr : Editor {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public override void OnInspectorGUI() {
		if(Selection.activeGameObject == sprite.gameObject) {
			if(sprite.anim != null) {
				if(sprite.anim.IsForceSelected) {
					Selection.activeGameObject = sprite.anim.gameObject;
				}
			}		
		} 
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------

    public IAESpriteRenderer sprite
    {
		get {
            return target as IAESpriteRenderer;
		}
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
