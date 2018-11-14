using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public Transform activeRod = null;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

}
