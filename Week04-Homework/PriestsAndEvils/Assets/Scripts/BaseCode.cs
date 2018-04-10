using UnityEngine;
using BasisforPriAndEvi;

namespace BasisforPriAndEvi {
	public class SSDirector : System.Object {//和PPT一样：单例，不被Unity内存管理
		private static SSDirector _instance;
		public ISceneController currentSceneController {get; set;}
		public static SSDirector getInstance() {
			if(_instance == null) {
				_instance = new SSDirector();
			}
			return _instance;
		}
	}
	
	public interface ISceneController {
		void LoadResources();

		//void Pause();
		//void Resume();
	}
	public interface IUserAction {
		void moveBoat();
		void characterClicked(MyCharacterController charContoller0);
		void restart();
	}

	public class Moveable : MonoBehaviour {
		Vector3 dest;//目的地绝对坐标
		Vector3 middle;//中间位置绝对坐标
		int state = 0;//0->stop, 1->start-middle, 2->middle-dest
		private int moveSpeed = 5;
		public void setDestination( Vector3 dest0) {
			if(dest0.y - transform.position.y < 0.0000000001 && dest0.y - transform.position.y > -0.0000000001) {//船移动
				dest = dest0;
				state = 3;
			}
			else if(dest0.y > transform.position.y) {//上岸下船 ↑ →
				Debug.Log("上岸下船");
				middle.y = dest0.y;
				middle.x = transform.position.x;
				dest = dest0;
				state = 1;
			}
			else {//下岸上船
				middle.x = dest0.x;
				middle.y = transform.position.y;
				dest = dest0;
				state = 1;
			}
			

		}
		void Update() {
			Debug.Log("move update");
			if(state == 1) {
				Debug.Log("move to middle");
				transform.position = Vector3.MoveTowards(transform.position,middle,Time.deltaTime*moveSpeed);
				if(middle - transform.position == Vector3.zero) {//已到达中间位置
					state = 2;
				}
			}
			else if(state == 2) {
				Debug.Log("move to dest");
				transform.position = Vector3.MoveTowards(transform.position,dest,Time.deltaTime*moveSpeed);
				if(dest - transform.position == Vector3.zero) {//已到达中间位置
					state = 0;
				}
			}
			else if(state == 3) {
				Debug.Log("move Boat in Moveable");
				transform.position = Vector3.MoveTowards(transform.position,dest,Time.deltaTime*moveSpeed);
				if(dest - transform.position == Vector3.zero) {
					state = 0;
				}
			}
		}

	}

	public class MyCharacterController {
		readonly GameObject character;
		readonly int characterType; //0->priest, 1->evil
		private string name;
		private CoastController coast;
		bool onBoat;
		readonly Moveable moveableScript;
		private ClickGUI clickGUI;
		public MyCharacterController(string charName) {
			if(charName == "Priest") {
				character = Object.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject)), Vector3.zero,  Quaternion.identity) as GameObject;
				characterType = 0;
			}
			else {
				character = Object.Instantiate(Resources.Load("Prefabs/Evil", typeof(GameObject)), Vector3.zero,  Quaternion.identity) as GameObject;
				characterType = 1;
			}
			moveableScript = character.AddComponent (typeof(Moveable)) as Moveable;

			clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
			clickGUI.setController(this);
		}
		//set/get成员
		public void setName (string newName) {
			name = newName;
		}

		public string getName() {
			return name;
		}

		public int getType() {
			return characterType;
		}

		public void setPos(Vector3 pos0) {
			character.transform.position = pos0;

		}
		//对于岸
		public void getOnCoast(CoastController coast0) { //=getOffBoat
			coast = coast0;
			onBoat = false;
			character.transform.parent = null;
		}
		public CoastController getCoast() {
			return coast;
		}

		
		//对于船
		public void getOnBoat (BoatController boat0) {
			onBoat = true;
			character.transform.parent = boat0.getBoat().transform;
			//moveableScript.setDestination(boat0.getEmptyPos());
		}
		public bool whetherOnBoat() {
			return onBoat;
		}

		//移动
		public void setDestination(Vector3 dest0) {
			moveableScript.setDestination(dest0);
		}

		//重置
		public void reset() {
			onBoat = false;
		}
	}

	public class BoatController {
		readonly GameObject boat;
		private int boatState; //0->left, 1->right
		private Vector3 moveLeft = new Vector3(-2.0f, -0.25f, 0.0f);
		private Vector3 moveRight = new Vector3(2.0f, -0.25f, 0.0f);
		readonly MyCharacterController [] boatCharController = new MyCharacterController[2];
		private Vector3 [] positions;
		private Moveable moveableScript;
		
		public BoatController () {
			boat = Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), moveRight,  Quaternion.identity) as GameObject;
			boat.name = "boat";
			positions = new[] {new Vector3(-0.5f,0.5f, 0.0f), new Vector3(0.5f, 0.5f, 0.0f)};//相对船的位置
			moveableScript = boat.AddComponent (typeof(Moveable)) as Moveable;
			boat.AddComponent (typeof(ClickGUI));
			boatState = 1;
		}

		//set/get成员字段
		public GameObject getBoat() {
			return boat;
		}
		public int getBoatState() {
			return boatState;
		}

		//船上空位管理
		public int getEmptyIndex() {
			for(int i = 0; i < 2; i++) {
				if(boatCharController[i] == null)
					return i;
			}
			return -1;
		}
		public Vector3 getEmptyPos() {
			int tempIndex = getEmptyIndex();
			if(tempIndex != -1) {
				Debug.Log("find empty position in the boat");
				return positions[tempIndex] + boat.transform.position;
			}
			return Vector3.zero;
		}
			//统计船上人数
		public int [] getCount() {
			int [] count = new [] {0,0};
			for(int i = 0; i < 2; i++) {
				if(boatCharController[i] != null)
					count[boatCharController[i].getType()]++;				
			}
			Debug.Log("Priest: "+ count[0] + " | Evils: " + count[1]);
			return count;
		}

		//游戏角色上下船管理
		public bool getOnBoat(MyCharacterController char0) {
			int tempIndex = getEmptyIndex();
			if(tempIndex != -1) {
				boatCharController[tempIndex] = char0;
				return true;
			}
			return false;
		}	
		public void getOffBoat(MyCharacterController char0) {
			for(int i = 0; i < 2; i++) {
				if(boatCharController[i] == char0) {
					boatCharController[i] = null;
				}
			}
		}

		//船移动那管理
		public void moveBoat() {
			Debug.Log("moveBoat in BoatController");
			if(getCount()[0] + getCount()[1] > 0) {//船上有人才能移动
				if(boatState == 0) {
					moveableScript.setDestination(moveRight);
					boatState = 1;
				}
				else {
					moveableScript.setDestination(moveLeft);
					boatState = 0;
				}
			}
		}

		//重置 
		public void reset() {
			moveableScript.setDestination(moveRight);
			boatState = 1;
			for(int i = 0; i < 2; i++) {
				boatCharController[i] = null;
			}
		}
	}

	public class CoastController {
		private GameObject coast;
		private Vector3 leftPos = new Vector3(-6,0,0);
		private Vector3 rightPos = new Vector3(6,0,0);
		readonly int coastType; //0->left, 1->right
		private Vector3 [] positions;
		readonly MyCharacterController [] coastCharController;

		public CoastController(string coastLeftOrRight) {
			if(coastLeftOrRight == "left") {
				coast = Object.Instantiate(Resources.Load("Prefabs/Coast", typeof(GameObject)), leftPos,  Quaternion.identity) as GameObject;
				coastType = 0;
			}
			else {
				coast = Object.Instantiate(Resources.Load("Prefabs/Coast", typeof(GameObject)), rightPos,  Quaternion.identity) as GameObject;
				coastType = 1;
			}
			coastCharController  = new MyCharacterController[6];
			positions = new [] {new Vector3(-0.5f,1.75f,0.0f), new Vector3(-1.5f,1.75f,0.0f), new Vector3(-2.5f,1.75f,0.0f), 
			new Vector3(0.5f,1.75f,0.0f), new Vector3(1.5f,1.75f,0.0f), new Vector3(2.5f,1.75f,0.0f)};

			for(int i = 0; i < 6; i++) {
				coastCharController[i] = null;
			}

		}
		//set/get字段
		public GameObject getCoast() {
			return coast;
		}

		public int getType() {
			return coastType;
		}

		//characters位置管理
		public int getEmptyIndex() {
			for(int i = 0; i <6; i++) {
				if(coastCharController[i] == null)
					return i;
			}
			Debug.Log("No Empty Ind");
			return -1;
		}
		public Vector3 getEmptyPos() {
			int tempIndex = getEmptyIndex();
			if(tempIndex != -1)
				return (coastType == 0 ? leftPos: rightPos) + positions[tempIndex];
			Debug.Log("No Empty Pos");
			return Vector3.zero;
		}

		//character上岸、下岸管理
		public bool getOnCoast(MyCharacterController newChar) {
			int tempIndex = getEmptyIndex();
			if(tempIndex != -1) {
				coastCharController[getEmptyIndex()] = newChar;
				return true;
			}
			return false;
		}

		public void getOffCoast(MyCharacterController removeChar) {
			for(int i = 0; i < 6; i++) {
				if(coastCharController[i] == removeChar) {
					coastCharController[i] = null;
				}
			}
		}

		public int [] getCount () {
			int [] count = new [] {0,0};
			for(int i = 0; i < 6; i++) 
				if(coastCharController[i] != null)
					count[coastCharController[i].getType()]++;
			return count;
		}

		public void reset() {
			for(int i = 0; i < 6; i++) {
				coastCharController[i] = null;
			}
		}
	}
}