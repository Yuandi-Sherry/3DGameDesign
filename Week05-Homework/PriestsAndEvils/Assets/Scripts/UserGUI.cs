using UnityEngine;
using BasisforPriAndEvi;
public class UserGUI : MonoBehaviour {
	private IUserAction action;
	public int gameOn;//1-> in game, 2-> win, 3->lose
	GUIStyle buttonStyle1;//button when game is on
	GUIStyle buttonStyle2;//button for restart
	GUIStyle labelStyle;//label for win or lose

	// Use this for initialization
	void Start () {
		Debug.Log("UserGUI start");
		action = SSDirector.getInstance().currentSceneController as IUserAction;
		//gameOn = 1;
		buttonStyle1 = new GUIStyle("button");
		buttonStyle1.fontSize = Screen.width/30;
		buttonStyle2 = new GUIStyle("button");
		buttonStyle2.fontSize = Screen.width/30;
		buttonStyle2.alignment = TextAnchor.MiddleCenter;
		labelStyle = new GUIStyle("label");
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.fontSize = Screen.width/40;
		labelStyle.normal.textColor = Color.black;

	}
	
	// Update is called once per frame
	void OnGUI () {
		Debug.Log("Please Log GUI");
		if(gameOn == 1) {//游戏进行中
			if(GUI.Button(new Rect(Screen.width/14, Screen.width/14, Screen.width/8, Screen.width/12), "Restart", buttonStyle1)) {
				action.restart();
				gameOn = 1;
			}
		}
		else {
			if(gameOn == 2) {
				GUI.Label(new Rect(Screen.width/2-70, Screen.height/4, 140, 70), "Congratulation! You win. ", labelStyle);
			}
			if(gameOn == 3) {
				GUI.Label(new Rect(Screen.width/2-70, Screen.height/4, 140, 70), "Game Over!", labelStyle);
			}
			if(GUI.Button(new Rect(Screen.width/2-Screen.width/16, Screen.height/2, Screen.width/8, Screen.width/12), "Restart", buttonStyle2)) {
				action.restart();
				gameOn = 1;
			}
		}
		
	}
}
