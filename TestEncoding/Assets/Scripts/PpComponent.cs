using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[ExecuteInEditMode]
public class PpComponent : MonoBehaviour {
	
	public Texture2D 	originalTexture;
	public Material 	originalMaterial;
	
	public Vector2[] 	uvOriginal;
	public Vector2[] 	uvPacked;//OnInit: ASSIGN MESH TO MAKE SURE THAT IT USES THE OVERRIDE UV
	
	//Collection Variables
	public string 	collectionName;
	public int		collectionMaxSize;
	public int		collectionPadding;
	
	
	
	
	[HideInInspector] public Material mat;
	[HideInInspector] public Mesh mesh;
	public List<GameObject> _relatedObjects = new List<GameObject>();
	
	
	/// <summary>
	/// DYNAMIC ARRAY OF THE RELATED GAME OBJECTS THAT WERE PACKED WITH THIS SET.
	/// </summary>
	public List<GameObject> relatedGameObjects{
		get{
			//CHECK AGAINS NULL GAME OBJECTS...
			List<GameObject> rtrn = new List<GameObject>();
			foreach(GameObject go in _relatedObjects){
				if (go != null){
					rtrn.Add(go);
				}
			}
			return rtrn;
		}
		set{
			_relatedObjects = value;
		}
	}
	
	/// <summary>
	/// GET THE ORIGINAL TEXTURE (EITHER BASED ON THE ORIGINAL'S MATERIAL MAIN TEXTURE OR THE ORIGINAL LINKED TEXTURE
	/// </summary>
	public Texture2D GetOriginalTexture(){
	
		if (originalMaterial != null){//PREFERRED
			return (Texture2D)originalMaterial.mainTexture;
		}else{
			return originalTexture;
		}
	}
	
	
	public void SetOriginalTexture(Texture2D tex){
		originalTexture = tex;
	}
	
	
	
	/// <summary>
	/// UNDO THE TEXTURE PACKING PROCESS
	/// </summary>
	public void Undo(){

		if (originalMaterial != null){//PREFERRED
			renderer.sharedMaterial = mat = originalMaterial;
		}
		
		if (originalTexture != null){//MAKE SURE THAT IF THERE IS A ORIGINAL TEXTURE REFERENCE THAT WE RESET THAT TOO
			mat.mainTexture = originalTexture;
		}
		
		
		
		
		if(uvOriginal != null){
			if(mesh.uv.Length == uvOriginal.Length){
				mesh.uv = uvOriginal;
			}
		}
		
		
		//Update RELATED OBJECTS
		
		if(_relatedObjects.Contains(this.gameObject)){
			_relatedObjects.Remove(this.gameObject);
		}
		foreach(GameObject go in relatedGameObjects){
			PpComponent cmp = go.GetComponent<PpComponent>();
			if (cmp != null){
				cmp._relatedObjects = this._relatedObjects;
			}
		}
		

		DestroyImmediate(this,false);//HAS TO BE CALLED IN EDITOR MODE INSTEAD OF OBJECT.DESTROY()
	}
	
	/// <summary>
	/// UNDO THE TEXTURE PACKING PROCESS FOR THE ENTIRE PACKING SET
	/// </summary>
	public void UndoAll(){
		
		foreach(GameObject go in relatedGameObjects){
			if (go != this.gameObject){//DON'T UNDO THIS GAMEOBJECT, DO THAT AT THE END
				
				PpComponent[] ppcs = go.GetComponentsInChildren<PpComponent>();
				foreach(PpComponent ppc in ppcs){
					if (ppc != this){
						ppc.Undo();
					}
				}
			}
		}
		
		Undo();//UNDO THIS ONE
	}
	

	/// <summary>
	/// WHEN THE GAME OBJECT INITIALIZES IN THE GAME MODE MAKE SURE THAT WE USE THE UPDATED UV'S
	/// </summary>
	void OnEnable(){
		if (this.mesh != null && this.uvPacked != null){
			
			if (this.mesh.uv.Length == this.uvPacked.Length){
				this.mesh.uv = this.uvPacked;
			}
			
		}
    }
	
	/// <summary>
	/// DRAW A CUSTOM GIZMO IN THE UNITY EDITOR VIEW
	/// </summary>
	
	
	void OnDrawGizmosSelected() {
		
		Gizmos.color = new Color(175f/255f,218f/255f,251f/255f,1f);
		Gizmos.DrawWireCube( renderer.bounds.center, renderer.bounds.size);
    }
	
	
	
}
