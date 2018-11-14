using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public Transform activeRod = null;

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
