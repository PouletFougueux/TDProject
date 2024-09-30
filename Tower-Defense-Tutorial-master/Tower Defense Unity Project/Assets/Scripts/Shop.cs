using UnityEngine;

public class Shop : MonoBehaviour {

	public TurretBlueprint standardTurret;
	public TurretBlueprint missileLauncher;
	public TurretBlueprint laserBeamer;

	BuildManager buildManager;

    

	void Start ()
	{
		buildManager = BuildManager.instance;
	}

    private void Update()
    {
        
        if (Input.GetKey(KeyCode.Alpha1))
        {
            SelectStandardTurret();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            SelectMissileLauncher();
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            SelectLaserBeamer();
        }
        if (Input.GetMouseButtonDown(1))
            buildManager.DeselectTurret();
    }

    public void SelectStandardTurret ()
	{
		Debug.Log("Standard Turret Selected");
		buildManager.SelectTurretToBuild(standardTurret);
	}

	public void SelectMissileLauncher()
	{
		Debug.Log("Missile Launcher Selected");
		buildManager.SelectTurretToBuild(missileLauncher);
	}

	public void SelectLaserBeamer()
	{
		Debug.Log("Laser Beamer Selected");
		buildManager.SelectTurretToBuild(laserBeamer);
	}

}
