# Priests and Evils: an MVC Structure Game Design

What's MVC? 简单地说，MVC是将Model, Controller, View分离开来的设计。但对于一个之前没有了解过MVC的初学者，这种说法太过抽象，让人摸不着头脑。借牧师与魔鬼这个作业说一下我对于这个设计模式的初步思考与认识。在此感谢[这篇博客]()给了我很大的启发~如果想看比较专业的讲解可以点它 :)

既然不太知道MVC的整体架构，就先从游戏本身入手：

### Model有哪些？

首先你要思考游戏中有什么：魔鬼、牧师和船。但只有这些吗？这些的确是显而易见的玩家可控的游戏对象，但对于一个游戏设计者，游戏中的每一个对象都是你要负责的，比如：河岸和河水。虽然它们不会移动，从玩家的视角无法直接操控它们，但如果它们与玩家可控的对象有接触（或者概括为信息交换）就需要纳入考虑范围。

先从简单的河水说起：河水和谁有接触？船。但河水需要知道船的信息吗？船的位置完全可以用世界坐标由船自己控制，而且河水上面只有一艘船，河水也不需要为船安排位置。

用对河水的几个问题思考一下河岸呢？就会发现，河岸与魔鬼和牧师有直接接触，而且一方面要安排他们的站位（六个角色总不能挤在一起吧），另一方面要统计他们的人数来判断游戏输赢。

#### 先用面向对象的方法思考游戏对象

如前文所说，这个游戏中的游戏对象有：

| 游戏对象    | 说明                                 |
| ------- | ---------------------------------- |
| Priests | 游戏中的牧师，用正方体表示。有下船、上岸行为。            |
| Evils   | 游戏中的魔鬼，用球体表示。亦有下船、上岸行为。【可以和牧师作为一类】 |
| Boat    | 游戏中的船，用有木纹贴图的长方体表示。有左移、右移行为。       |
| Coasts  | 游戏中的岸，用有沙滩贴图的长方体。有计数行为。            |
| water   | 游戏中的水，用有水纹贴图的长方体表示。无行为。【不需要构造类】    |

当我们清楚了游戏中有哪些对象的时候，就可以开始考虑controller了。

### Controller控制的是什么？

最初学习别人的代码时，发现这个游戏有很多带有`controller`的类。包括：`MyCharacterController`，`CoastController`，`BoatController`以及放在与他们不同的另一个文件中的`FirstController`（在我的代码中叫`mainController`）

所以，Controller可以被分为两种：第一种为GameObjectController，就我目前的理解，我认为controller封装了游戏对象（GameObject类）、控制游戏对象的方法和所需的一些变量（例如位置、标记状态、类别的bool或int等等）；第二种为MainController，简单的说就是调用第一种Controller中定义的方法操作Models的。

#### 声明GameObjectController中方法时需要考虑的两个重要问题

我真的觉得这里超级重要，理解了这个之后大概就能初步理解这个设计模式了。

1. Controller的隔离：一个Controller只能控制自己应该控制的游戏对象，比如：魔鬼上岸的时候，调用`getOnCoast(CoastController coast0)`控制魔鬼上coast0这个河岸，同时，我们导致了coast0河岸上多了一个魔鬼，但不能够在`MyCharacterController`试图修改coast0的相关属性（如魔鬼的数量），而应当在`CoastController`中定义相应的方法接收这一事件。

2. Controller的自治：其实这个是第一条隔离导致的结果，正是不能在一个Controller中定义方法修改另一个Controller的字段，所以每个Controller要定义自己的方法解决这个问题。在上面的例子中：`CoastController`也有一个`getOnCoast(MyCharacterController char0)`来让河岸自己应对char0游戏对象上岸这一事件。

   **总结起来，这两个问题其实是一个问题，也就是一个变化产生的时候，所有关联的物体都要发生变化：getOnCoast()也要相应地在不同物体中被调用。**

三个类中所声明的字段和方法如下，也有相应注释。

```csharp
		//MyCharacterController
		readonly GameObject character;
		readonly int characterType; //0->priest, 1->evil
		private string name;
		private CoastController coast;
		bool onBoat;
		readonly Moveable moveableScript;
		private ClickGUI clickGUI;
		//构造函数：实例化预制，判断是魔鬼或牧师，添加ClickGUI组件和MoveScript脚本（这个之后再说）
		public MyCharacterController(string charName);
		//set/get成员
		public void setName (string newName);
		public string getName();
		public int getType();
		public void setPos(Vector3 pos0);
		//对于岸
		public void getOnCoast(CoastController coast0);
		public CoastController getCoast();
		//对于船
		public void getOnBoat (BoatController boat0);
		public bool whetherOnBoat();
		//移动 - 这一步暂时用不到
		public void setDestination(Vector3 dest0);
		//重置 - 用户重新开始时被调用
		public void reset();
```

```csharp
		//MyBoatController
		readonly GameObject boat;
		private int boatState; //0->left, 1->right
		private Vector3 moveLeft = new Vector3(-2.0f, -0.25f, 0.0f);
		private Vector3 moveRight = new Vector3(2.0f, -0.25f, 0.0f);
		readonly MyCharacterController [] boatCharController = new MyCharacterController[2];
		private Vector3 [] positions;
		private Moveable moveableScript;
		public BoatController ();
		//set/get成员字段
		public GameObject getBoat();
		public int getBoatState();
		//船上空位管理
		public int getEmptyIndex();
		public Vector3 getEmptyPos();
			//统计船上人数
		public int [] getCount();
		//游戏角色上下船管理
		public bool getOnBoat(MyCharacterController char0);
		public void getOffBoat(MyCharacterController char0);
		//船移动的管理
		public void moveBoat();
		//重置 - 用户重新开始时被调用
		public void reset();
```

```csharp
		private GameObject coast;
		private Vector3 leftPos = new Vector3(-6,0,0);
		private Vector3 rightPos = new Vector3(6,0,0);
		readonly int coastType; //0->left, 1->right
		private Vector3 [] positions;
		readonly MyCharacterController [] coastCharController;
		//构造函数：赋值岸为左岸还是右岸，实例化预制，为岸上六个位置申请内存
		public CoastController(string coastLeftOrRight);
		//set/get字段
		public GameObject getCoast()；
		public int getType()；
		//characters位置管理
		public int getEmptyIndex();
		public Vector3 getEmptyPos();
		//character上岸、下岸管理
		public bool getOnCoast(MyCharacterController newChar);
		public void getOffCoast(MyCharacterController removeChar);
		//character数目统计，判断输赢时调用
		public int [] getCount ();
		//重置 - 用户重新开始时被调用
		public void reset();
```

#### FirstController需要做什么？

我暂且理解为对游戏整体的初始化，first顾名思义，应是游戏运行时第一个调用的脚本。

1. 第一步：确定导演

   ​	在Awake()函数中先初始化导演。这里需要指出的是导演为单例模式，整个场景中只能有一个导演，否则一群导演一个往东一个往西就会乱套。而导演类中还需要一个场记的**接口**，可以理解为单例的导演需要时常呼叫场记跑来跑去去应对各种外界的事情以及安排演员。

2. 第二步：指派场记

   ​	将现在FirstController赋值给导演里的场记，之后就可以调用FirstController的类成员函数：

   ```csharp
   	//加载资源
   	public void LoadResources();
   	private void LoadCharacters();
   	//控制船、对象点击时的变化View的部分会讲到，是IUserAction接口中的方法
   	public void moveBoat();
   	public void characterClicked(MyCharacterController charController0);
   	//控制游戏状态
   	public void restart();
   	void judge () ;
   ```

3. 第三步：动态加载资源

   ​	在Awake里调用场记里面的LoadResources(). 

4. 第四步：初始化一些游戏变量

另外，考虑到GameObjectController之间最好不要彼此控制，所以在FirstController中，对于一个状态的改变，场记要通过调用不同控制器的相应方法，通知这一改变所涉及的所有游戏对象。如在魔鬼下岸上船这一改变中：

```csharp
if(boat.getBoatState() == 1 && charController0.getCoast().getType() == 1 || boat.getBoatState() == 0 && charController0.getCoast().getType() == 0) { //船在右侧且要从右岸上船，或船在左侧要从左岸上船，当然这个函数是用来响应Click事件的（在后一部分会说）
	Vector3 emptyPos = boat.getEmptyPos();
	if(emptyPos != Vector3.zero) {//判断船上是否有空位
		boat.getOnBoat(charController0);//告诉船：有人要上船啦，占用船上一个空位
		charController0.getOnBoat(boat);//告诉物体：上船（下岸），改变onBoat属性，且tranform的父类变为船
		charController0.setDestination(emptyPos);//告诉物体：你要去这个地方（这里会在后面的对运动的单独控制中简析）
		charController0.getCoast().getOffCoast(charController0);//告诉岸：一个物体离开了你的怀抱，相应的，岸会多一个空位
	}
}
```

### View需要实现哪些功能？

View主要负责管理与用户交互的**视图**部分，包括Restart按钮、"You Win"/"Game Over"提示。需要声明`IUserAction`接口来管理用户行为：

```csharp
public interface IUserAction {
	//Click事件的响应
	void moveBoat();//用户点击船
	void characterClicked(MyCharacterController charContoller0);//用户点击魔鬼或牧师，由于有多个，需要在参数中指出具体是哪个
	//用户点击restart按钮时
	void restart();
}
```

#### UserGUI

新建一个`UserGUI`类，这个类的主要属性是一个由`IUserAction`类实例化的`action`，在`Start()`函数中为：

```csharp
//将导演的场记强制转化为用户行为管理
action = SSDirector.getInstance().currentSceneController as IUserAction;
```

（我本来想试着不用强制类型转化，在Director里面再建立一个IUserAction的属性，但会出现无法对接口实例化的问题，目前还没有想到更好的方法只能先强制转化）

这里Button和Label和上次作业中井字棋差不多用`OnGUI()`实现，主要思路就是Restart Button被点击的时候，调用`IUserAction`中的`restart()`函数。当其中的标记状态的int值改变时，相应的按钮/标签表示即可。

#### ClickGUI

和UserGUI类，当游戏对象被点击时，触发游戏对象所在Controller的相应方法。比如，在boat这个游戏对象接收到点击事件时，会调用`BoatController`中的`moveBoat()`函数。这里为了MVC设计模式的层次性，在`ClickGUI`中调用`IUserAction`实例的`characterClicked(MyCharacterController char0)`控制魔鬼和牧师被点击的事件或`moveBoat()`来控制船的移动，在场记`FirstController`类（上一步中已经把FirstController赋值给场记了）中再定义相应的方法。

### 这三者如何关联起来

首先，FirstController要继承MonoBehavior和定义的两个接口（`IUserBehavior`和`IScenceController`）.

对于需要接收用户点击事件的游戏对象，需要将ClickGUI类作为一个组件添加上去：

```csharp
//在MyCharacterController中
public class MyCharacterController {
	...
	private ClickGUI clickGUI;
	public MyCharacterController(string charName) {
		...
		clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
		clickGUI.setController(this);
	}
}
//在BoatController中
public class BoatController {
	...
	public BoatController () {
		...
		boat.AddComponent (typeof(ClickGUI));
	}
}
```

用户界面类`UserGUI`控制的是整个场景，和`ClickGUI`的方法类似，只不过作为组件添加到`IScenceController`的实例`FirstController`上。

```csharp
public class MainController : MonoBehaviour, ISceneController, IUserAction {
	...
	UserGUI userGUI;
	void Awake () {
		...
		userGUI = gameObject.AddComponent(typeof(UserGUI)) as UserGUI;
		userGUI.gameOn = 1;
		...
	}
}
```

### 对运动的单独控制

最开始阅读这个游戏的代码时，会发现和所有Controller放在一个文件下的还有一个`Moveable`用来控制有位置变化的对象的运动。

最开始觉得，将position的变化在定义每个改变状态的函数（如`MyCharacterController.getOnBoat()`），但会发现这里需要改变牧师/魔鬼的`onBoat`，而且还要将boat的空位作为参数传入或者是在`MyCharacterController`的函数中调用`BoatController`的`getEmptyPosition()`。

虽然这样做看上去没有大问题，但这里的位置变化并不是简单地从起点到终点直线运动那么简单。因为魔鬼和牧师不可能穿越河岸到船上，所以对于一个折线运动的控制倘若在每个上岸下船、下岸上船的运动中都实现一遍，不仅会增加代码量、出错几率，假如将来运动的轨迹要变得更加复杂或者说有更多的对象要在更多的位置之间移动，还是单独建立一个控制运动的类`Moveable`比较方便。

```csharp
public class Moveable : MonoBehaviour {
	Vector3 dest;//目的地绝对坐标
	Vector3 middle;//中间位置绝对坐标
	int state = 0;//0->stop, 1->start-middle, 2->middle-dest
	private int moveSpeed = 5;
	//
	public void setDestination( Vector3 dest0) {
		if(dest0.y - transform.position.y < 0.0000000001 && dest0.y - transform.position.y > -0.0000000001) {//船移动
			dest = dest0;
			state = 3;
		}
		else if(dest0.y > transform.position.y) {//上岸下船 ↑ →
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
		...
	}
}
```

另外从软件工程多人合作实现一个项目的角度看，将运动的实现单独分出一个人负责实现，而另一个人只需要调用相应的函数，告知目的地，游戏对象即可按照`Moveable`中定义的轨迹运行而不需了解运动函数内部的实现原理。在更复杂的工程项目中，将各个控制板块分离会更易于分工合作，提高开发效率。