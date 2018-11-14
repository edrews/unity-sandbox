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
    private bool hoveredOver = false;

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
        updateRodColor();
        if (IsActive())
        {
            updateRotation();
            updateTranslation();
        }
    }

    void updateRotation()
    {
        float rotation = Input.GetAxis("Horizontal") * Time.deltaTime * rotateSpeed;
        transform.Rotate(0, 0, rotation);
    }

    void updateTranslation()
    {
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

    void updateRodColor()
    {
        if (hoveredOver)
        {
            setColor(hoverColor);
        }
        else
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
        hoveredOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoveredOver = false;
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
