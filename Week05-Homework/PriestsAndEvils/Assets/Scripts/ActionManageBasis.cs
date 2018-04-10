using System.Collections.Generic;
using UnityEngine;
using BasisforPriAndEvi;

namespace ActionManageBasicCode {
	public enum SSActionEventType {Started, Competeted}

	public interface ISSActionCallback {
		void SSActionDone(SSAction source, SSActionEventType events = SSActionEventType.Competeted, int intPara = 0, string strPara = null, Object objectParam = null);
	}

	public abstract class SSAction : ScriptableObject {
		public bool enable = true;
		public bool destroy = false;

		public GameObject gameobject { get; set; }
		public Transform transform { get; set; }
		public ISSActionCallback callback { get; set; }

		protected SSAction() {}
		public virtual void  Start () {
			throw new System.NotImplementedException();
		}

		public virtual void  Update () {
			throw new System.NotImplementedException();
		}
	}

	public class CCMoveToAction : SSAction {
		private float speed = 5.0f;
		private Vector3 destination;

		public static CCMoveToAction GetSSAction(Vector3 destination, float speed) {
			CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
			action.destination = destination;
			action.speed = speed; 
			return action;
		}
		//behavior
		public override void Start() {	}
		public override void Update() {
			gameobject.transform.position = Vector3.MoveTowards(gameobject.transform.position, destination,Time.deltaTime*speed);
			if(gameobject.transform.position == destination) { //判断动作完成并通知受影响者
				callback.SSActionDone(this);//告知自己：我已经完成了这个动作
				destroy = true;//被销毁
				
			}
		}
	}

	public class CCSequenceAction : SSAction, ISSActionCallback {
		public List<SSAction> sequence;
		public int repeat = -1;//repeat forever
		public int current = 0;

		//protected CCSequenceAction()
		public static CCSequenceAction GetSSAction(List <SSAction> sequence, int currentIndex, int repeatTimes) {
			Debug.Log("call the getAction in Sequence");
			CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();//!
			action.sequence = sequence;
			action.current = currentIndex;
			action.repeat = repeatTimes;
			return action;
		}

		public override void Update( ) {
			if(sequence.Count == 0) 
				return;
			if(current < sequence.Count) {
				Debug.Log("sequence Current");
				sequence[current].Update();//MoveToAction中的Update方法将自己销毁了
			}			
		}

		//implementation of the interface
		public void SSActionDone(SSAction source, SSActionEventType events, int intPara, string strPara, Object objectParam){
			Debug.Log("call the callback in Sequence");
			source.destroy = false;//系列动作没有被销毁
			current++;
			if(current >= sequence.Count) {
				current = 0;
				if(repeat > 0) repeat--;
				if(repeat == 0) {
					destroy = true; 
				//	callback.SSActionDone(this); 
				}
			}
		}

		public override void Start() {
			foreach(SSAction action in sequence) {//将系列动作的每一个的发起者和作用者都是自己
				action.gameobject = gameobject;
				action.transform = this.transform;
				action.callback = this;
				action.Start();
			}
		}

		void OnDestroy() {
			foreach(SSAction action in sequence) {
				DestroyObject(action);
			}
			//ScriptableObject.OnDestroy()

			//如果自己被destroy则释放内存
		}
	}


	public abstract class SSActionManager : MonoBehaviour {
		private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction> ();//正在进行
		private List<SSAction> waitingAdd = new List<SSAction> ();//等待加入
		private List<int> waitingDelete = new List<int> ();//等待删除
		protected void Update () {
			foreach (SSAction ac in waitingAdd) 
				actions[ac.GetInstanceID()] = ac;
			waitingAdd.Clear();

			foreach(KeyValuePair <int, SSAction> kv in actions) {
				SSAction ac = kv.Value;
				if(ac.destroy) {
					waitingDelete.Add(ac.GetInstanceID());
				}
				else if (ac.enable) {
					ac.Update();
				}
			}

			foreach(int key in waitingDelete) {
				SSAction ac = actions[key];
				actions.Remove(key);
				DestroyObject(ac);
			}

			waitingDelete.Clear();
		}

		public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager) {
			action.gameobject = gameobject;
			action.callback = manager;
			waitingAdd.Add(action);
			action.Start();
		}

		protected virtual void Start() {

		}
	}
}
