using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guys : MonoBehaviour
{

    public enum GuyCount
    {
        Two, 
        Three,
        Five
    };

	public GameObject[] guys;
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMaterial(Material material)
	{
		foreach (Transform child in transform)
		{
			child.GetComponent<Renderer>().sharedMaterial = material;
		}
	}

	public void SetCount(GuyCount count)
	{
		if (count == GuyCount.Two)
		{
			SetGuyActive(0, false);
			SetGuyActive(1, false);
			SetGuyActive(2, true);
			SetGuyActive(3, false);
			SetGuyActive(4, false);
			SetGuyActive(5, false);
			SetGuyActive(6, true);
			SetGuyActive(7, false);
			SetGuyActive(8, false);
		}
		else if (count == GuyCount.Three)
		{
			SetGuyActive(0, false);
			SetGuyActive(1, true);
			SetGuyActive(2, false);
			SetGuyActive(3, false);
			SetGuyActive(4, true);
			SetGuyActive(5, false);
			SetGuyActive(6, false);
			SetGuyActive(7, true);
			SetGuyActive(8, false);	
		}
		else if (count == GuyCount.Five)
		{
			SetGuyActive(0, true);
			SetGuyActive(1, false);
			SetGuyActive(2, true);
			SetGuyActive(3, false);
			SetGuyActive(4, true);
			SetGuyActive(5, false);
			SetGuyActive(6, true);
			SetGuyActive(7, false);
			SetGuyActive(8, true);	
		}
	}

	void SetGuyActive(int childNum, bool active)
	{
		transform.GetChild(childNum).gameObject.SetActive(active);
	}

	void OnValidate()
	{

	}
}
