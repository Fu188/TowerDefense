using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace MagicArsenal
{
public class MagicFireProjectile : MonoBehaviour 
{
    RaycastHit hit;
    public GameObject[] projectiles;
    public static Transform spawnPosition;
    [HideInInspector]
    public int currentProjectile = 0;
	public float speed = 1000;

//    MyGUI _GUI;
	MagicButtonScript selectedProjectileButton;

	void Start () 
	{

	}

	void Update () 
	{	
        print(55);
        if (Physics.Raycast(Camera.main.ScreenPointToRay(spawnPosition.position), out hit, 100f))
        {
            GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
            projectile.transform.LookAt(hit.point);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
            projectile.GetComponent<MagicProjectileScript>().impactNormal = hit.normal;
        }  
        Debug.DrawRay(Camera.main.ScreenPointToRay(spawnPosition.position).origin, Camera.main.ScreenPointToRay(spawnPosition.position).direction*100, Color.yellow);
	}

    public void nextEffect()
    {
        if (currentProjectile < projectiles.Length - 1)
            currentProjectile++;
        else
            currentProjectile = 0;
		selectedProjectileButton.getProjectileNames();
    }

    public void previousEffect()
    {
        if (currentProjectile > 0)
            currentProjectile--;
        else
            currentProjectile = projectiles.Length-1;
		selectedProjectileButton.getProjectileNames();
    }

	public void AdjustSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
}
}