using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDown : MonoBehaviour
{
	[SerializeField] float moveTime = 1.0f;
	[SerializeField] float moveSpeed = 9.0f; 

	bool up = true;
	float currentTime;
	private void Update()
	{


		currentTime += Time.deltaTime;

		if (currentTime > moveTime)
		{
			if (up) up = false;
			else up = true;

			currentTime = 0;

		}
		if (up)
		{
			transform.Translate(0, Time.deltaTime * moveSpeed, 0);
			
		}
		else
		{
			transform.Translate(0, -Time.deltaTime * moveSpeed, 0);
			
		}
	}
}
