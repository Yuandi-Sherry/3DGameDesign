# Week 04 - Homework

## 游戏对象运动的本质是什么？

- 游戏对象运动的本质是通过在极短时间`Time.deltaTime`内改变游戏对象的`transform.position`实现运动效果的。

## 请用三种方法以上方法，实现物体的抛物线运动。（如，修改Transform属性，使用向量Vector3的方法…）

1. 方法一【计算每一时刻x, y方向的分速度】：根据斜抛公式计算物体每个Time.deltaTime内沿x,y方向的分速度，用`Vector3*Time.deltaTime`计算每个deltaTime过后物体的所在位置。

   - 分速度计算公式如下：

     ​	为简便计算，假设物体斜抛的水平方向初速度为`initialVelocityRight`，竖直方向初速度为`initialVelocityUp`：

     - $V_x = initialVelocityRight$
     - $V_y = initialVelocityUp - g\sum\delta t$

   - 关键代码如下：

     ```csharp
     void Update () {
     	count += Time.deltaTime; //计算从抛出到当前时刻经过的时间
     	transform.position += new Vector3(initialVelocityRight, initialVelocityUp-9.8f*count, 0.0f)*Time.deltaTime;
     }
     ```

2. 方法二【计算物体每一时刻的和速度和位置】：和方法一类似，根据斜抛公式计算物体每一时刻的位移向量，根据能量守恒公式计算物体某一时刻的合速度。

   ​	运用`MoveTowards`函数，它表示的是表示以`maxDistanceDelta`速度从`current`直线移动到`target`：

   ```csharp
   public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta);
   ```

   可以将`current` 设置为物体当前位置，`target`设置为物体目标位置，`maxDistanceDelta`用能量守恒计算的某一时刻的合速度得到。

   ​	虽然从current到target不是真正的速度方向，由于$\delta t$很短，可以将位移方向近似为速度方向。

   - 关键代码如下：

     ```csharp
     void Update () {
         count += Time.deltaTime;
         //极短时间内的下一个位置
         Vector3 nextPos = new Vector3(initialVelocityRight*count, initialVelocityUp* count - 0.5f*9.8f*count*count);
         //能量守恒计算此时的和速度
         float currentSpeed = Mathf.Sqrt(initialVelocityRight*initialVelocityRight+initialVelocityUp*initialVelocityUp - 2*9.8f*(initialVelocityUp* count - 0.5f*9.8f*count*count));
         //因为这里的deltaTime是很小的，所以速度方向近似等于位移方向
         transform.position = Vector3.MoveTowards(transform.position, nextPos, currentSpeed*Time.deltaTime);
     }
     ```

3. 方法三【给定物体初速度并添加恒力】：通过rigidbody模拟给物体施加重力，并设置初速度。

   - 关键代码如下：

     ```csharp
     void Start() {
     	gameObject.AddComponent<Rigidbody>(); //添加rigidbody组件
     	rb = gameObject.GetComponent<Rigidbody>();
     	rb.useGravity = true;//添加重力
     	rb.velocity = new Vector3(initialVelocityRight, initialVelocityUp, 0.0f);//设置初速度
     }
     ```

## 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。

​	这个程序中用到的函数主要有：

 1.  `RotateAround`：用于控制八大行星绕太阳的公转和月亮绕地球的公转

     > `public void RotateAround(Vector3 point, Vector3 axis, float angle)`
     >
     > ​	Rotates the transform about `axis` passing through `point` in world coordinates by `angle` degrees.

 2. `Rotate`：用于控制行星的自转

     > `public void Rotate(Vector3 eulerAngles, Space relativeTo = Space.Self)`
     >
     > ​	Applies a rotation of `eulerAngles.z` degrees around the z axis, `eulerAngles.x` degrees around the x axis, and `eulerAngles.y`degrees around the y axis (in that order).
     >
     > ​	If `relativeTo` is not specified or set to Space.Self the rotation is applied around the transform's local axes. If `relativeTo` is set to Space.World the rotation is applied around the world x, y, z axes.

    - 第一步：创建Sphere作为太阳、八大行星、月球，贴图Sphere，这个[连接](https://tieba.baidu.com/p/4876471245?red_tag=0043515197)有比较好看的贴图；并在Hierarchy构造好他们的继承关系。（如果愿意的话可以查一下各个行星的大小比例会做得更逼真一点）

    ![hierarchy in sun system](https://github.com/Yuandi-Sherry/3DGameDesign/blob/master/%E7%AE%80%E7%AD%94%E9%A2%98%E9%85%8D%E5%9B%BE/4.1sun%E7%BB%93%E6%9E%84.PNG?raw=true)

    - 第二步：Start函数初始化行星环绕半径以及所在法平面。这里初始化直接根据八大行星的半径进行相对位置的初始化（其中太阳的位置我设置在原点），然后用[`public static float Range(float min, float max)`](https://docs.unity3d.com/ScriptReference/Random.Range.html)随机RotateAround中aixs参数的值。

    ```csharp
    void Start () {
    	sun.position = Vector3.zero;
    	Mercury.position = new Vector3 (6, 0, 0);
    	...
    	Neptune.position = new Vector3 (32, 0, 0);
    	axisMercury = new Vector3 (0, Random.Range(0, 100), Random.Range(0, 100));
    	...
    	axisNeptune = new Vector3 (0, Random.Range(0, 100), Random.Range(0, 100));
    }
    ```

    - 第三步：Update函数中每个极短时间deltaTime内用两个旋转函数更新自转和公转的状态。 

    ```csharp
    void Update () {
    	//RotateAround第三个参数都是不同的
    	Mercury.RotateAround (sun.position, axisMercury, 20*Time.deltaTime);
    	Mercury.Rotate (Vector3.up*50*Time.deltaTime);
    	...
    	Earth.RotateAround (sun.position, axisEarth, 10*Time.deltaTime);
    	Earth.Rotate (Vector3.up*30*Time.deltaTime);
    	moon.transform.RotateAround (Earth.position, Vector3.up, 359 * Time.deltaTime);
    	...
    	Neptune.RotateAround (sun.position, axisNeptune, 4*Time.deltaTime);
    	Neptune.Rotate (Vector3.up*30*Time.deltaTime);
    }
    ```

    - 最后把这个脚本挂在sun上面就可以啦~效果如下：

    ![效果图](https://github.com/Yuandi-Sherry/3DGameDesign/blob/master/%E7%AE%80%E7%AD%94%E9%A2%98%E9%85%8D%E5%9B%BE/4.2%E5%A4%AA%E9%98%B3%E7%B3%BB%E6%95%88%E6%9E%9C%E5%9B%BE.PNG?raw=true)

    这幅图大概做了一下各个行星比例的不同，实际太阳系各个行星的比例更加悬殊 :-|
