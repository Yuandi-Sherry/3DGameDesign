using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class method3 : MonoBehaviour {
	public float initialVelocityRight;
    public float initialVelocityUp;
    private float count = 0;
    // Update is called once per frame
    void Update () {
        count += Time.deltaTime;
        //极短时间内的下一个位置
        Vector3 nextPos = new Vector3(initialVelocityRight*count, initialVelocityUp* count - 0.5f*9.8f*count*count);
        //能量守恒计算此时的和速度
        float currentSpeed = Mathf.Sqrt(initialVelocityRight*initialVelocityRight+initialVelocityUp*initialVelocityUp - 2*9.8f*(initialVelocityUp* count - 0.5f*9.8f*count*count));
        //因为这里的deltaTime是很小的，所以速度方向近似等于位移方向
        transform.position = Vector3.MoveTowards(transform.position, nextPos, currentSpeed*Time.deltaTime);
    }
}
