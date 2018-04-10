using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasisforPriAndEvi;
using ActionManageBasicCode;

public class ActionManager : SSActionManager, ISSActionCallback {
	public MainController sceneController;

	protected new void Start() {
		sceneController = (MainController)SSDirector.getInstance().currentSceneController;
		sceneController.actionManager = this;
	}
	CCMoveToAction moveBoatAction;
	public void moveBoat (BoatController boat) {
		if(boat.getBoatState() == 1) {
			moveBoatAction = CCMoveToAction.GetSSAction(new Vector3(-2, -0.25f, 0), 10.0f);
		}
		else {
			moveBoatAction = CCMoveToAction.GetSSAction(new Vector3(2, -0.25f, 0), 10.0f);
		}	
		this.RunAction(sceneController.boat.getBoat(), moveBoatAction, this);
	}
	CCSequenceAction moveChar;
	public void moveCharacter (MyCharacterController char0, Vector3 destination) {
		//Debug.Log("起始位置 " + char0.getGO().transform.position.x + " " + char0.getGO().transform.position.y + " " + char0.getGO().transform.position.z );
		//Debug.Log("destination  " + destination.x + destination.y + destination.z);
		Vector3 currentPosition = char0.getGO().transform.position;//这里一定要记录一下， 如果后面每次都取的话，这个是实时变化的
		CCMoveToAction move1;
		CCMoveToAction move2;
		Vector3 middlePos;
		if(char0.whetherOnBoat()) {//在船上
			middlePos = new Vector3(destination.x, currentPosition.y, currentPosition.z);
		}
		else {
			middlePos = new Vector3(currentPosition.x, destination.y, currentPosition.z);
		}
		move1 = CCMoveToAction.GetSSAction(middlePos, 10.0f);
		move2 = CCMoveToAction.GetSSAction(destination, 10.0f);
		moveChar = CCSequenceAction.GetSSAction(new List<SSAction>{move1, move2}, 0, 1);
		this.RunAction(char0.getGO(), moveChar, this);

	}


	public void SSActionDone(SSAction source, SSActionEventType events = SSActionEventType.Competeted, int intPara = 0, string strPara = null, Object objectParam = null) {
		Debug.Log("SSActionDone");
	}

}