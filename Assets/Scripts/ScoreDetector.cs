using UnityEngine;

public class ScoreDetector : MonoBehaviour {

	public int playerGoalNumber;
	void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.gameObject.tag);
		if (collision.transform.gameObject.tag == "Ball")
		{
			GameManager.instance.OnGoalScored(playerGoalNumber);
		}
    }
}
