using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public Transform activeRod = null;
	private bool gamePaused = false;
	public GameObject[] rods;
	public GameObject[] blueRods;
	public GameObject[] redRods;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			addPhysicsRaycaster();
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		activeRod = rods[4].transform;
		if (Config.isTwoPlayer)
		{
			Debug.Log("Two Player");
		}
	}

	void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
			if (Config.isTwoPlayer)
			{
				SelectLeftRod(blueRods);
			}
			else
			{
				SelectLeftRod(rods);
			}	
        }
        if (Input.GetButtonDown("Fire2"))
        {
			if (Config.isTwoPlayer)
			{
				SelectRightRod(blueRods);
			}
			else
			{
				SelectRightRod(rods);
			}
		}
    }

	void SelectRightRod(GameObject[] rodArray)
	{
		for (int i = 0; i < rodArray.Length; i++)
		{
			if (rodArray[i].transform == activeRod)
			{
				if (i == rodArray.Length-1)
				{
					activeRod = rodArray[0].transform;
				}
				else
				{
					activeRod = rodArray[i+1].transform;
				}
				break;
			}
		}
	}

	void SelectLeftRod(GameObject[] rodArray)
	{
		for (int i = 0; i < rodArray.Length; i++)
		{
			if (rodArray[i].transform == activeRod)
			{
				if (i == 0)
				{
					activeRod = rodArray[rodArray.Length-1].transform;
				}
				else
				{
					activeRod = rodArray[i-1].transform;
				}
				break;
			}
		}
	}

	public void OnGoalScored(int playerGoalNumber)
	{
		if (gamePaused) return;
		gamePaused = true;

		Text scoreText;
		if (playerGoalNumber == 1)
		{
			scoreText = GameObject.Find("BlueScore").GetComponent<Text>();
		}
		else
		{
			scoreText = GameObject.Find("RedScore").GetComponent<Text>();
		}
		int number = int.Parse(scoreText.text);
		number++;
		scoreText.text = number.ToString();
		StartCoroutine(ResetBallPosition());
	}

	IEnumerator ResetBallPosition()
    {
        yield return new WaitForSeconds(2);
		//Reset ball position
		GameObject.Find("Ball").transform.position = new Vector3(0f, 10f, 0f);
		gamePaused = false;
    }

    void addPhysicsRaycaster()
    {
        PhysicsRaycaster physicsRaycaster = GameObject.FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        }
    }

}
