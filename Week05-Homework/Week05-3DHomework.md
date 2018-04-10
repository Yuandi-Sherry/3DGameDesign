# Week05 Homework 游戏对象使用总结

## 游戏对象的创建与克隆

- 常用函数

  - 创建游戏对象基本类型

    ```csharp
    public static GameObject CreatePrimitive(PrimitiveType type);
    ```

  - 克隆游戏对象

    ```csharp
    public static Object Instantiate(Object original);
    ```

  - 从预制中加载游戏对象

    ```csharp
    public static Object Load(string path, Type systemTypeInstance);
    ```

## 游戏对象的组件

- Active

  - 如果没有勾选则不会进行更新和渲染。

- Transform

  - 通过Transform组件可以改变物体的位置、旋转角度以及大小。其中对于positionn的控制是后文中实现游戏对象运动时索要改变的。

  - 可以在程序中实现物体之间的组合关系。

    ```csharp
    A.transform.parent = B.transform
    ```

  - 常用函数

    - 平移

    ```csharp
    public void Translate(Vector3 translation, Space relativeTo = Space.Self);
    ```

    - 旋转

    ```csharp
    public void Rotate(Vector3 eulerAngles, Space relativeTo = Space.Self);
    ```

    - 绕轴旋转

    ```csharp
    public void RotateAround(Vector3 point, Vector3 axis, float angle);
    ```

    - 两点间指定速度直线移动

    ```csharp
    public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta);
    ```

  这个函数是在魔鬼与牧师的实现中常用的函数，需要注意的是target是**目标位置**的向量，而非移动路径的向量。

- Rigidbody

  - 这个是在Unity官方教程Roll a ball中使用的为物体施加重力并能够通过读取键盘输入以及Addforce函数对物体施加外力进行操作。
  - 在抛物线的多种实现中也加入rigidbody添加了gravity. 

- MonoBehaviour(作为现阶段最常用的Script)

  - 这个基类中的主要方法以及他们执行的特点和顺序之前的blog中已经做过试验了，课件中的结论如图：

    ![img](https://github.com/Yuandi-Sherry/3DGameDesign/blob/master/Blog%E9%85%8D%E5%9B%BE/Week05_1.1.PNG?raw=true)

  - 常见问题：

    代码中所定义的类的实例化：

    ​	之前的编程中经常会遇到`NullReferenceException: Object reference not set to an instance of an object`的报错，虽然明白是对象没有实例化的原因，但是缺不知道具体用使用哪一个实例化方法。个人总结：实例化方法的选择与所采用的**设计模式**，游戏对象的加载方式（一般动态加载比较容易出问题）。

| 使用函数                                     | 对应情景                                     |
| ---------------------------------------- | ---------------------------------------- |
| `public Component AddComponent(Type componentType)` | 向游戏对象添加代码中所定义的类作为组件，调用的时候一般后加`as GameObject` 进行强制转换。 |
| ` public Component GetComponent(Type type); ` | 需要实现将定义type类的代码挂在到gameObject上，有一篇魔鬼和牧师的动作分离版本使用了这个函数并将动作管理器挂在到了空对象（主摄像机之上）。 |
| `XX = this`                              | 这是最近两次作业比较常见的实例化方式，所在的实例化的对象本身就在对这个类进行整个游戏中**唯一**的定义。比如，场记要作为导演类唯一场记接口的实现，同样，场记中唯一的动作管理器在实现的时候赋值为this相连接。 |

- Terrain

  ![img](https://github.com/Yuandi-Sherry/3DGameDesign/blob/master/Blog%E9%85%8D%E5%9B%BE/Week05_1.0.PNG?raw=true)

  - 常用的主要是1,3,4按钮，第一个为将地形向上提升，第三个为平滑地形，第四个为美化地形表面（将图片粉饰到地表）。

- Camera

  - 透视镜头
    - Field of View控制所见范围
  - 正交镜头
    - Size 正交镜头的尺寸
  - Viewport Rect控制相机在屏幕中的距离左边界和上边界的坐标位置（用0-1表示所在屏幕比例的位置）以及宽度高度表示占据屏幕宽度高度的比例。（在多镜头观察不同场景并同时在主屏幕显示时比较有用）

- Skybox

  > ​        创建material元素并设置为skybox，将下载的天空盒图片添加到material上，并在摄像机上添加天空盒组件并将新创建的天空盒材料挂载到组件上。