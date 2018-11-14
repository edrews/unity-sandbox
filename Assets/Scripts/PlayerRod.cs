using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRod : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler {    
	public Color hoverColor = Color.green;
	public Color activeColor = Color.yellow;
    public float rotateSpeed = 2000;
    public float slideSpeed = 50;
    public float slideMax = 5;
    public Material playerMaterial;

    private static Color inactiveColor;

    void Start()
    {
		addPhysicsRaycaster();
        OnValidate();
	}

    void OnValidate()
    {
        foreach (Transform tr in transform)
        {
            if (tr.tag == "Guy")
            {
                tr.GetComponent<Renderer>().sharedMaterial = playerMaterial;
            }
            else if (tr.tag == "Rod")
            {
                inactiveColor = tr.GetComponent<Renderer>().sharedMaterial.color;
            }
        }
    }

	void Update()
	{

	}

	void FixedUpdate()
    {
        if (GameManager.instance.activeRod != this.transform)
        {
            return;
        }

        float rotation = Input.GetAxis("Horizontal") * Time.deltaTime * rotateSpeed;
        transform.Rotate(0, 0, rotation);

        var sliding = -1 * Input.GetAxis("Vertical") * Time.deltaTime * slideSpeed;
        float target = transform.position.z + sliding;
        if (target > slideMax && sliding > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, slideMax);
        }
        else if (target < -slideMax && sliding < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -slideMax);
        }
        else
        {
            transform.Translate(0, 0, sliding);
        }
    }

    //Move to game controller?
    void addPhysicsRaycaster()
    {
        PhysicsRaycaster physicsRaycaster = GameObject.FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        }
    }

	void setColor(Color targetColor)
	{
		foreach (Transform child in transform)
        {
            if (child.tag == "Rod")
            {
                child.GetComponent<Renderer>().material.color = targetColor;
            }
		}
	}
	public void OnPointerEnter(PointerEventData eventData)
    {
        setColor(hoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsActive())
        {
            setColor(activeColor);
        }
        else
        {
            setColor(inactiveColor);
        }
	}

    bool IsActive()
    {
        return GameManager.instance.activeRod == this.transform;
    }

	public void OnPointerClick(PointerEventData eventData)
    {
		GameManager.instance.activeRod = this.transform;
        setColor(activeColor);
	}
}
