using UnityEngine;

public class ScoreDetector : MonoBehaviour {

	public int playerGoalNumber;
	void OnCollisionEnter(Collision collision)
    {
		if (collision.transform.gameObject.tag == "Ball")
		{
			GameManager.instance.OnGoalScored(playerGoalNumber);
		}
    }
}
