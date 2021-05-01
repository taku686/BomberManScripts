using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

public class TimeControlTest : BaseBehaviour
{
	private float moveSpeed = 1.0f;

	void Update()
	{
		//この書き方だと、キー操作しても期待通りの時間制御ができない。
		//transform.Translate(Vector3.up * Time.deltaTime);

		if (time.timeScale > 0)
		{
			time.rigidbody.velocity = new Vector3(transform.localScale.x * moveSpeed, time.rigidbody.velocity.y, 0);
		}

		Clock clock = Timekeeper.instance.Clock("Root");
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			clock.localTimeScale = -1; // Rewind：巻き戻し
		}
		else if (Input.GetKey(KeyCode.Alpha2))
		{
			clock.localTimeScale = 0; // Pause：停止
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			clock.localTimeScale = 0.5f; // Slow：ゆっくり移動
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			clock.localTimeScale = 1; // Normal：通常速度で移動
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			clock.localTimeScale = 2; // Accelerate：速く移動
		}
	}
}
