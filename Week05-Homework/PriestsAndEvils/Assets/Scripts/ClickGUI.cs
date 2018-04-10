using UnityEngine;
using BasisforPriAndEvi;
public class ClickGUI : MonoBehaviour {
	IUserAction action;
	MyCharacterController charContoller;

	public void setController (MyCharacterController charContoller0) {
		charContoller = charContoller0;
	}


	// Use this for initialization
	void Start () {
		action = SSDirector.getInstance().currentSceneController as IUserAction;
	}
	
	// Update is called once per frame
	void OnMouseDown() {
		if(gameObject.name == "boat") {
			action.moveBoat();
		}
		else {
			
			action.characterClicked(charContoller);
		}
	}
}
