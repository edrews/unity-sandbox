using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRod : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{    
	public Color hoverColor = Color.green;
	public Color activeColor = Color.yellow;
    public Material playerMaterial;
    public Guys.GuyCount numberOfGuys = Guys.GuyCount.Two; 

    private Guys guys;
    private float rotateSpeed = 500f;
    private float slideSpeed = 40f;
    private float slideMax = 4f;
    private static Color inactiveColor = Color.gray;
    private bool hoveredOver = false;
    private string hInput = "Horizontal";
    private string vInput = "Vertical";

    void OnValidate()
    {
        SetupGuys();
    }

    void Start()
    {
        SetupGuys();
    }

    void SetupGuys()
    {
        switch (numberOfGuys)
        {
            case Guys.GuyCount.Two:
                slideMax = 8.5f;
                break;
            case Guys.GuyCount.Three:
                slideMax = 6.5f;
                break;
            case Guys.GuyCount.Five:
                slideMax = 4.5f;
                break;
            default:
                break;
        }
        if (guys != null)
        {
            guys.SetMaterial(playerMaterial);
            guys.SetCount(numberOfGuys);
        }
    }

    public void SetInputs(string horizontal, string vertical)
    {
        hInput = horizontal;
        vInput = vertical;
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
        float rotation = Input.GetAxis(hInput) * Time.deltaTime * rotateSpeed;
        transform.Rotate(0, 0, rotation);
    }

    void updateTranslation()
    {
        var sliding = -1 * Input.GetAxis(vInput) * Time.deltaTime * slideSpeed;
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
        if (transform != null && GameManager.instance != null)
        {
            return GameManager.instance.IsActive(transform);
        }
        return false;
    }

	public void OnPointerClick(PointerEventData eventData)
    {
		GameManager.instance.SetActiveRod(transform);
        setColor(activeColor);
	}
}
