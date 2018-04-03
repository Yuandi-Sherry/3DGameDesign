using UnityEngine;
using BasisforPriAndEvi;
public class MainController : MonoBehaviour, ISceneController, IUserAction {

	CoastController leftCoast;
	CoastController rightCoast;
	BoatController boat;
	Vector3 waterPosition = new Vector3(0.0f, -1.0f, 0.0f);
	MyCharacterController [] characters;
	UserGUI userGUI;
	bool GameOn;

	// Use this for initialization
	void Awake () {
		//先定导演
		SSDirector director = SSDirector.getInstance();
		director.currentSceneController = this;
		characters = new MyCharacterController[6];
		director.currentSceneController.LoadResources();
		userGUI = gameObject.AddComponent(typeof(UserGUI)) as UserGUI;
		userGUI.gameOn = 1;
		GameOn = true;
	}

	public void LoadResources( ) {
		GameObject water = Object.Instantiate(Resources.Load("Prefabs/water", typeof(GameObject)), waterPosition, Quaternion.identity) as GameObject;
		leftCoast = new CoastController("left");
		rightCoast = new CoastController("right");
		boat = new BoatController();
		Debug.Log("Load water, 2 coasts and a boat");
		LoadCharacters();
	}

	// Update is called once per frame
	private void LoadCharacters( ) {
		for(int i = 0; i < 3; i++) {
			characters[i] = new MyCharacterController ("Priest");
			characters[i].setName("Priest" + i);
			characters[i].setPos(rightCoast.getEmptyPos());
			rightCoast.getOnCoast(characters[i]);
			characters[i].getOnCoast(rightCoast);
		}
		for(int i = 3; i < 6; i++) {
			characters[i] = new MyCharacterController("Evil");
			characters[i].setName("Evil" + i);
			characters[i].setPos(rightCoast.getEmptyPos());
			rightCoast.getOnCoast(characters[i]);
			characters[i].getOnCoast(rightCoast);
		}
		Debug.Log("Load All Characters");
	}

	public void moveBoat() {
		if(GameOn)
			boat.moveBoat();
		judge();
	}
	public void characterClicked(MyCharacterController charController0) {
		if(!GameOn) {
			return;
		}
		if(charController0.whetherOnBoat()) {//在船上，要去岸上
			if(boat.getBoatState() == 1) {//船在右侧
				charController0.getOnCoast(rightCoast);
				rightCoast.getOnCoast(charController0);//修改了岸上pos的状态
				boat.getOffBoat(charController0);
				charController0.setDestination(rightCoast.getEmptyPos());

			}
			else {//船在左侧
				//游戏角色要上左岸
				charController0.setDestination(leftCoast.getEmptyPos());
				charController0.getOnCoast(leftCoast);
				leftCoast.getOnCoast(charController0);
				boat.getOffBoat(charController0);
			}
		}
		else {//在岸上，要去船上
			if(boat.getBoatState() == 1 && charController0.getCoast().getType() == 1 || boat.getBoatState() == 0 && charController0.getCoast().getType() == 0) { //船在右侧且要从右岸上船
				Vector3 emptyPos = boat.getEmptyPos();
				if(emptyPos != Vector3.zero) {//判断船上是否有空位
					boat.getOnBoat(charController0);//告诉船：有人要上船啦
					charController0.getOnBoat(boat);//告诉物体：上船（下岸）
					charController0.setDestination(emptyPos);//告诉物体：你要去这个地方
					charController0.getCoast().getOffCoast(charController0);//告诉岸：一个物体离开了你的怀抱，你多了一个空位
				}
			}
		}
		judge();
	}

	public void restart() {
		GameOn = true;
		userGUI.gameOn = 2;
		boat.reset();
		leftCoast.reset();
		rightCoast.reset();
		for(int i = 0; i < 6; i++) {
			characters[i].setPos(rightCoast.getEmptyPos());
			rightCoast.getOnCoast(characters[i]);
			characters[i].getOnCoast(rightCoast);
			characters[i].reset();
		}
	}

	void judge () {
		int PriCount = 0;
		int EvilCount = 0;
		//先统计右边的
		PriCount = rightCoast.getCount()[0];
		EvilCount = rightCoast.getCount()[1];
		//如果船在右边
		if(boat.getBoatState() == 1) {
			PriCount += boat.getCount()[0];
			EvilCount += boat.getCount()[1];
		}
		if(EvilCount > PriCount && PriCount > 0) {
			lose();
		}
		Debug.Log("right: Pri " + PriCount + " Evi " + EvilCount);

		//再统计左边的
		PriCount = leftCoast.getCount()[0];
		EvilCount = leftCoast.getCount()[1];
		//如果船在右边
		if(boat.getBoatState() == 0) {
			PriCount += boat.getCount()[0];
			EvilCount += boat.getCount()[1];
		}
		if(EvilCount > PriCount && PriCount > 0) {
			lose();
		}

		if(userGUI.gameOn == 1) {
			if(PriCount == 3 && EvilCount == 3) {
				win();
			}
		}

		Debug.Log("left: Pri " + PriCount + " Evi " + EvilCount);
	}

	void lose() {
		userGUI.gameOn = 3;
		GameOn = false;
		Debug.Log("lose");
	}
	void win() {
		userGUI.gameOn = 2;
		GameOn = false;
		Debug.Log("win");
	}
}
