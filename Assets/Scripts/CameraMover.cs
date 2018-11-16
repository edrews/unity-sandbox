using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

	public Transform gameView;
	public Transform menuView;
    public float speed;
	
	IOnComplete onCompleteHandler;
	float t = 0f;
	Transform src;
	Transform dest;
	bool isRunning = false;
	string handlerAction;

	public void MoveToGame(IOnComplete handler, string action)
	{
		src = menuView;
		dest = gameView;
		StartMove(handler, action);
	}

	public void MoveToMenu(IOnComplete handler, string action)
	{
		src = gameView;
		dest = menuView;
		StartMove(handler, action);
	}

	private void StartMove(IOnComplete handler, string action)
	{
		onCompleteHandler = handler;
		handlerAction = action;
		isRunning = true;
		t = 0f;
	}

	void Update() 
	{
		if (isRunning)
		{
			t += Time.deltaTime * speed;
			transform.position = Vector3.Lerp(src.position, dest.position, t);
			transform.rotation = Quaternion.Lerp(src.rotation, dest.rotation, t);
			if (t >= 1)
			{
				isRunning = false;
				onCompleteHandler.OnComplete(handlerAction);
			}
		}
	}
}
