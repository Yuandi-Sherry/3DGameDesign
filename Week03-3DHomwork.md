#Week 03 - Homework

- 解释 游戏对象（GameObjects） 和 资源（Assets）的区别与联系。

  - **游戏对象**是Unity中最基础的对象，它可以代表游戏角色、 游戏道具和游戏场景。它们本身不会并不会为游戏贡献很多，但是它们却作为**组件**（展现真正游戏效果）的容器发挥着重要作用。[^1]
  - 资源是能够用在游戏中的事物，可以来自Unity外创建的文件，比如3D模型、音频文件、图像，或是任何Unity支持的其他文件类型。同时，Unity内部也可以创建很多资源类型，比如动画控制器、音频混合器和渲染纹理等等。[^2]
  - 区别在于：
    1. 游戏对象是游戏中的一个逻辑概念，有自身的属性和行为（比如玩家、大BOSS、场景）；而大部分游戏资源是来自外部的文件，仅作为游戏对象的组成部分（比如材料、音乐、图片）。
    2. 游戏对象是游戏过程的一部分；资源是游戏编程过程的一部分。
  - 联系在于：
    1. 游戏对象可以作为资源的一部分；
    2. 游戏资源既可以成为游戏对象本身，也可以作为游戏对象的一种属性使游戏对象更为生动。

- 编写一个代码，使用 debug 语句来验证MonoBehaviour基本行为或事件触发的条件

  - 基本行为包括 Awake() Start() Update() FixedUpdate() LateUpdate()

  - 常用事件包括 OnGUI() OnDisable() OnEnable()

  - ```c#
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
     
    public class testOrder : MonoBehaviour {
    	void Awake () {
    		Debug.Log("Awake");	
    	}
    	void Start () {
    		Debug.Log("Start");	
    	}
    	void Update () {
    		Debug.Log("Update");	
    	}
    	void FixedUpdate () {
    		Debug.Log("FixedUpdate");	
    	}
    	void LateUpdate () {
    		Debug.Log("LateUpdate");	
    	}
    	void OnGUI () {
    		Debug.Log("OnGUI");	
    	}
    	void OnDisable () {
    		Debug.Log("OnDisable");	
    	}
    	void OnEnable () {
    		Debug.Log("OnEnable");	
    	}
    }
    ```

  - ![pro2](C:\Users\Sherry\Documents\Curriculums\3D Game Design\Homework1 - 井字棋\pro2.png)

- 查找脚本手册，了解GameObject，Transform，Component 对象

  - 分别翻译官方对三个对象的描述（Description）

    - **GameObject**: 游戏对象是Unity中最基础的对象，它可以代表游戏角色、 游戏道具和游戏场景。它们本身不会并不会为游戏贡献很多，但是它们却作为**组件**（展现真正游戏效果）的容器发挥着重要作用。[^1]
    - **Transform**: Transform（变换）组件决定了场景中每一个物体的位置、旋转角度和缩放大小。每一个游戏对象都有一个变换（组件）。[^3]
    - **Component**: 添加到游戏对象的一切事物的**基类**。注意：你的代码永远不会直接地创建**组件**；相反，你会写**脚本代码**并将脚本添加到**游戏对象**上。[^4]

  - 描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件

    - 本题目要求是把可视化图形编程界面与 Unity API 对应起来，当你在 Inspector 面板上每一个内容，应该知道对应 API。

    - 例如：table 的对象是 GameObject，第一个选择框是 activeSelf 属性。

    - table的对象是**GameObject**，它有如下属性：

      - 第一个选择框是**activeSelf**属性：通过停止勾选（可视化图形编程界面）或将其属性改为inactive（API中），GameObject可以被暂时从场景中移出。[^5]

      - 第二个框是**Transform Component**

      - 第三个框是**Mesh Filter(网格过滤器)**：网格过滤器从资源中获取一个网格，并将其传递给网格渲染器使其得以在屏幕上渲染。[^6]

      - 第四个框是**Box Collider(盒装对撞机)**：是一个盒子形状的原型对撞机。[^7]

        | 属性                                       | 解释                 |
        | ---------------------------------------- | :----------------- |
        | [center](https://docs.unity3d.com/ScriptReference/BoxCollider-center.html) | 在**本地坐标系**下盒子的中心坐标 |
        | [size](https://docs.unity3d.com/ScriptReference/BoxCollider-size.html) | 在对象**本地坐标系**下盒子的大小 |

      - 第五个框（第三个选择框）是**Mesh Renderer(网格渲染器)**：网格渲染器从**网格过滤器**中获取几何图形，并将其在**Transform Component**所定义的位置上渲染出来。[^8]

  - 用UML 图描述 三者的关系（请使用 UMLet 14.1.1 stand-alone版本出图）

    ![UML - GameObject, Component, Transform](C:\Users\Sherry\Documents\Curriculums\3D Game Design\Homework1 - 井字棋\UML - GameObject, Component, Transform.jpg)

- 整理相关学习资料，编写简单代码验证以下技术的实现：

  - 查找对象[^9]：

    - 通过名字查找：

      ```c#
      public static GameObject Find(string name)
      ```

    - 通过标签查找单个对象：

      ```c#
      public static GameObject FindWithTag(string tag)
      ```

    - 通过标签查找多个对象：

      ```c#
      public static GameObject[] FindGameObjectsWithTag(string tag)
      ```

  - 添加子对象 

    - ```c#
      public static GameObect CreatePrimitive(PrimitiveType type);
      ```
      ```c#
      GameObject child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      child.transform.parent = this.transform;//这里child是孩子，this是parent
      ```

  - 遍历对象树

    - ```C#
      foreach (Transform child in transform) {
      	Debug.Log(child.gameObject.name);
      }
      ```

  - 清除所有子对象

    - ```c#
      foreach (Transform child in transform) {
      	Destroy(child.gameObject);
      }
      ```

- 资源预设（Prefabs）与 对象克隆 (clone)

  - 预设（Prefabs）有什么好处？[^10]
    - 当需要创建一个在场景中要被多次使用的对象时，比如：NPC，游戏道具或者一小部分游戏场景。简单复制出来的多个对象，在需要修改的时候，需要一个一个单独编辑，会造成很大的不方便。**预设**就是这样一个可以存放实例（组件和性质齐全的游戏对象）的对象，其中的实例有着相同的属性，需要编辑的时候只需要编辑一个对象就可以对所有对象生效了。
  - 预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？[^11]
    - 对象克隆就是对一个游戏对象的简单复制，复制过后依然可以选择性更改其属性。克隆本体改变，克隆对象不会随之改变。
  - 制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象[^12]
    - ```c#
      public class PrefabBeh : MonoBehaviour {
      	public Transform prefab;
          void Start()
          {
              for (int i = 0; i < 3; i++)
              {
                  Instantiate(prefab, new Vector3(i * 8.0F, 0, 0), Quaternion.identity);
              }
          }
      }
      ```
      运行结果：

      ![tablePrefabResult](C:\Users\Sherry\Documents\Curriculums\3D Game Design\Homework1 - 井字棋\tablePrefabResult.PNG)

      ![tablePrefabResult2](C:\Users\Sherry\Documents\Curriculums\3D Game Design\Homework1 - 井字棋\tablePrefabResult2.PNG)

- 尝试解释组合模式（Composite Pattern / 一种设计模式）。使用 `BroadcastMessage()`方法向子对象发送消息

  `public void BroadcastMessage(string methodName, object parameter = null, SendMessageOptions options = SendMessageOptions.RequireReceiver)`

  ParentCall.cs: 

  ```c#
  public class ParentCall : MonoBehaviour {
  	void Update () {
  		this.BroadcastMessage("ApplyDamage", 5.0F,  SendMessageOptions.RequireReceiver);
  	}
  	void ApplyDamage() {
  		print("table is called");
  	}
  }
  ```
  ChildCalled1.cs:

  ```c#
  public class ChildCalled1 : MonoBehaviour {
  	void ApplyDamage() {
          print("Chair 1 is called");
  	}
  }
  ```
  运行结果：

  ![broadCast](C:\Users\Sherry\Documents\Curriculums\3D Game Design\Homework1 - 井字棋\broadCast.PNG)

[^1]: https://docs.unity3d.com/ScriptReference/GameObject.html
[^2]: https://docs.unity3d.com/Manual/AssetWorkflow.html
[^3]: https://docs.unity3d.com/Manual/class-Transform.html
[^4]: https://docs.unity3d.com/Manual/Components.html
[^5]: https://docs.unity3d.com/Manual/DeactivatingGameObjects.html
[^6]: https://docs.unity3d.com/Manual/class-MeshFilter.html
[^7]: https://docs.unity3d.com/ScriptReference/BoxCollider.html
[^8]: https://docs.unity3d.com/Manual/class-MeshRenderer.html
[^9]: https://docs.unity3d.com/ScriptReference/GameObject.Find.html
[^10]: https://docs.unity3d.com/Manual/Prefabs.html
[^11]: https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
[^12]: https://docs.unity3d.com/ScriptReference/Object.Instantiate.html

### 其他参考资料

1. https://docs.unity3d.com/Manual/UsingTheInspector.html
2. https://docs.unity3d.com/Manual/Prefabs.html
3. https://www.jianshu.com/p/005c6303d55c