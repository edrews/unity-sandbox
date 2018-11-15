using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public GameObject ballPrefab;
	public GameObject playerOneRods;
	public GameObject playerTwoRods;

	GameObject currentPlayerOneRods = null;
	bool gamePaused = false;
	Transform playerOneActiveRod = null;
	Transform playerTwoActiveRod = null;
	Vector3 ballSpawnPosition;
	float maxRandomVelocity = 5f;
	float ballInitialVelocity = 22f;
	List<GameObject> balls;

	const float LOWER_BOUND = -10f;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			addPhysicsRaycaster();
			balls = new List<GameObject>();
			ballSpawnPosition = GameObject.Find("FoosballSpawnPosition").transform.position;
			if (Config.isTwoPlayer)
			{
				foreach (Transform child in playerTwoRods.transform)
				{
					PlayerRod rod = child.transform.GetComponent<PlayerRod>();
					rod.SetInputs("P2_Horizontal", "P2_Vertical");
				}
			}
			ResetGameState();
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void AddBall(int number)
	{
		StartCoroutine(AddBallRoutine(number));
	}

	public IEnumerator AddBallRoutine(int number)
	{
        for (int i = 0; i < number; i++)
        {
            GameObject ball = Instantiate(ballPrefab, ballSpawnPosition, Quaternion.identity);
			ball.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-maxRandomVelocity, maxRandomVelocity), 
			Random.Range(-maxRandomVelocity, maxRandomVelocity),-ballInitialVelocity);
			balls.Add(ball);
			if (i < number - 1)
			{
				yield return new WaitForSeconds(0.02f);
			}
        }
	}

	public void ResetGameState()
	{
		foreach (var ball in balls)
		{
			Destroy(ball, 0.0f);
		}
		balls.Clear();
		AddBall(1);
		currentPlayerOneRods = playerOneRods;
		SetActiveRod(currentPlayerOneRods.transform.GetChild(1)); //Midline
		//ResetChildRotation(playerOneRods.transform);
		//ResetChildRotation(playerTwoRods.transform);
		if (Config.isTwoPlayer)
		{
			SetActiveRod(playerTwoRods.transform.GetChild(1));
		}
	}

	void ResetChildRotation(Transform transform)
	{
		foreach (Transform child in transform)
		{
			child.transform.rotation = Quaternion.identity;
		}
	}

	void HandleToggleTeam()
	{
		if (Config.isTwoPlayer) return;
		if (Input.GetButtonDown("ToggleTeam"))
		{
			if (currentPlayerOneRods == playerOneRods)
			{
				currentPlayerOneRods = playerTwoRods;
			}
			else
			{
				currentPlayerOneRods = playerOneRods;
			}
			playerOneActiveRod = currentPlayerOneRods.transform.GetChild(1); //Midline
		}
	}

	void HandleLeftSelect()
	{
       if (Input.GetButtonDown("LeftSelect"))
        {
			if (currentPlayerOneRods == playerOneRods)
			{
				SelectLeftRod(ref playerOneActiveRod, currentPlayerOneRods);
			}
			else
			{
				SelectRightRod(ref playerOneActiveRod, currentPlayerOneRods);
			}	
        }
		if (Input.GetButtonDown("P2_LeftSelect") && Config.isTwoPlayer)
		{
			SelectRightRod(ref playerTwoActiveRod, playerTwoRods);
		}
	}

	void HandleRightSelect()
	{
        if (Input.GetButtonDown("RightSelect"))
        {
			if (currentPlayerOneRods == playerOneRods)
			{
				SelectRightRod(ref playerOneActiveRod, currentPlayerOneRods);
			}
			else
			{
				SelectLeftRod(ref playerOneActiveRod, currentPlayerOneRods);
			}
		}
		if (Input.GetButtonDown("P2_RightSelect") && Config.isTwoPlayer)
		{
			SelectLeftRod(ref playerTwoActiveRod, playerTwoRods);
		}
	}

	void Update()
    {
		HandleToggleTeam();
		HandleLeftSelect();
		HandleRightSelect();
		//HandleQuit();
		HandleOutOfBounds();
    }

	void HandleOutOfBounds()
	{
		for (int i = balls.Count - 1; i >= 0; i--)
		{
			var ball = balls[i];
			if (ball.transform.position.y < LOWER_BOUND)
			{
				balls.RemoveAt(i);
				Destroy(ball, 0.0f);
				return;
			}			
		}
	}

	void HandleQuit()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			SceneManager.LoadScene(0);
		}
	}

	public bool IsActive(Transform transform)
	{
		if (transform == playerOneActiveRod || transform == playerTwoActiveRod)
		{
			return true;
		}
		return false;
	}

	public void SetActiveRod(Transform rod)
	{	
		foreach (Transform child in currentPlayerOneRods.transform)
		{
			if (child == rod)
			{
				playerOneActiveRod = rod;
				return;
			}
		}
		foreach (Transform child in playerTwoRods.transform)
		{
			if (child == rod)
			{
				playerTwoActiveRod = rod;
				return;
			}
		}
	}

	void SelectRightRod(ref Transform currentRod, GameObject rods)
	{
		Transform parent = rods.transform;
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			if (child == currentRod)
			{
				if (i == parent.childCount - 1)
				{
					currentRod = parent.GetChild(0);
				}
				else
				{
					currentRod = parent.GetChild(i+1);
				}
				break;
			}
		}
	}

	void SelectLeftRod(ref Transform currentRod, GameObject rods)
	{
		Transform parent = rods.transform;
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			if (child == currentRod)
			{
				if (i == 0)
				{
					currentRod = parent.GetChild(parent.childCount-1);
				}
				else
				{
					currentRod = parent.GetChild(i-1);
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
		StartCoroutine(ResetGame());
	}

	IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(2);
		ResetGameState();
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
