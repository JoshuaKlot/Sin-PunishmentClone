using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isShooting, readyToShoot;
    public GameObject bulletPrefab;
    public GameObject Recticle;
    public Transform bulletSpawn;
    public float timeBtwnBullets = 0.2f;
    public float bulletSpeed = 30f;
    public float bulletLife = 3f;
    bool allowReset;
    // Start is called before the first frame update
    void Start()
    {
        readyToShoot = true;
        allowReset = true;
    }

    // Update is called once per frame
    void Update()
    {
        isShooting = Input.GetKey(KeyCode.Mouse0);
        if (readyToShoot && isShooting)
        {
            FireWeapon();
        }
    }

    void FireWeapon()
    {
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirection().normalized;
        if (allowReset)
        {
            Invoke("ResetShot", timeBtwnBullets);
            allowReset = false;
        }
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLife));
    }

    private void ResetShot()
    {
        allowReset = true;
        readyToShoot = true;
    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
    public Vector3 CalculateDirection()
    {
        // Get the reticle's screen position
        Vector3 reticleScreenPosition = Recticle.GetComponent<ReticleController>().GetReticlePosition();

        // Use the main camera to create a ray from the screen point
        Ray ray = Camera.main.ScreenPointToRay(reticleScreenPosition);
        RaycastHit hit;

        Vector3 targetPoint;

        // Perform a raycast to check for an "Enemy" first
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // If the ray hits an enemy, aim at the enemy's position
                targetPoint = hit.transform.position;
            }
            else if (hit.collider.CompareTag("AimHere"))
            {
                // If the ray hits the "AimHere" collider, use the collision point
                targetPoint = hit.point;
            }
            else
            {
                // Default target if no relevant tags are hit
                targetPoint = ray.GetPoint(100);
            }
        }
        else
        {
            // If the raycast doesn't hit anything, set a default target point
            targetPoint = ray.GetPoint(100); // A point 100 units away from the camera
        }

        // Calculate the direction vector from the bullet spawn position to the target point
        Vector3 direction = targetPoint - bulletSpawn.position;

        // Ensure the bullet never fires behind the player
        if (Vector3.Dot(-transform.right, direction) < 0)
        {
            direction = -transform.right; // Force the direction to face forward
        }

        return direction.normalized; // Return the normalized direction vector
    }




}
