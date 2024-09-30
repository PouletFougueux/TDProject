using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	public Color hoverColor;
	public Color notEnoughMoneyColor;
    public Vector3 positionOffset;

	[HideInInspector]
	public GameObject turret;
	[HideInInspector]
	public TurretBlueprint turretBlueprint;
	[HideInInspector]
    
	public bool isUpgraded = false;

    public bool hasTurret;
	private Renderer rend;
	private Color startColor;
    private BoxCollider nodeCollider;
	private NodeUI ui;

	BuildManager buildManager;

	void Start ()
	{
        nodeCollider = GetComponent<BoxCollider>();
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;
        

		buildManager = BuildManager.instance;
        ui = buildManager.nodeUI;
    }

    private void Update()
    {
        if (hasTurret)
        {
            if (DetectCursor())
            {
                if (Input.GetMouseButtonDown(1))
                    ui.Sell();
                if (Input.GetMouseButtonDown(2) && !isUpgraded)
                    ui.Upgrade();
                //buildManager.SelectNode(this);
                return;
            }
        }
    }


    private bool DetectCursor()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Si le ray touche quelque chose
        if (Physics.Raycast(ray, out hit))
        {
			if (hit.collider == nodeCollider)
				return true;
            else return false;
        }
        else return false;

    }
   


    public Vector3 GetBuildPosition ()
	{
		return transform.position + positionOffset;
	}

    private void OnMouseOver()
    {
        /*if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }*/
    }



    void OnMouseDown ()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (turret != null)
		{
			buildManager.SelectNode(this);
			return;
		}

		if (!buildManager.CanBuild)
			return;

		BuildTurret(buildManager.GetTurretToBuild());
        buildManager.DeselectTurret();
	}

	void BuildTurret (TurretBlueprint blueprint)
	{
		if (PlayerStats.Money < blueprint.cost)
		{
			Debug.Log("Not enough money to build that!");
			return;
		}

		PlayerStats.Money -= blueprint.cost;

		GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
		turret = _turret;

		turretBlueprint = blueprint;

		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);
        hasTurret = true;

		Debug.Log("Turret build!");
	}

	public void UpgradeTurret ()
	{
		if (PlayerStats.Money < turretBlueprint.upgradeCost)
		{
			Debug.Log("Not enough money to upgrade that!");
			return;
		}

		PlayerStats.Money -= turretBlueprint.upgradeCost;

		//Get rid of the old turret
		Destroy(turret);

		//Build a new one
		GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
		turret = _turret;

		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		isUpgraded = true;

		Debug.Log("Turret upgraded!");
	}

	public void SellTurret ()
	{
		PlayerStats.Money += turretBlueprint.GetSellAmount();

		GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		Destroy(turret);
		turretBlueprint = null;
	}

	void OnMouseEnter ()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (!buildManager.CanBuild)
			return;

		if (buildManager.HasMoney)
		{
			rend.material.color = hoverColor;
		} else
		{
			rend.material.color = notEnoughMoneyColor;
		}

	}

	void OnMouseExit ()
	{
		rend.material.color = startColor;
    }

}
