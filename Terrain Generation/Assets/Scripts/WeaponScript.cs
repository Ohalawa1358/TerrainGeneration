using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    
    public float maxSway = 1f;
    public float swayAmount = 0.4f;
    public float swaySmoothValue = 0.5f;
    

    private Vector3 initialSwayPosition;

    public Camera camera;
    public GameObject projectile;
    public Transform projectileStart;
    public float rayRange = 10f;
    public float gunDamage = 5f;

    public SpriteRenderer muzzleFlash;
    private bool flashFlag = false;
    
    private void Start()
    {
        initialSwayPosition = transform.localPosition;
        muzzleFlash.gameObject.transform.position = projectileStart.position;
    }

    private void Update()
    {
//        anim.SetBool("isShooting", false);
//
//        if (Input.GetKeyDown(KeyCode.Mouse0) && isRaycast)
//        {
////            Instantiate(projectile, projectileStart.position, projectileStart.rotation);
//            Vector3 rayOrigin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
//            RaycastHit hitInfo = new RaycastHit();
//
//            if (Physics.Raycast(rayOrigin, camera.transform.forward, out hitInfo, rayRange))
//            {
//                if (hitInfo.collider.CompareTag("Enemy"))
//                {
//                    hitInfo.collider.GetComponent<EnemyHealth>().takeDamage(gunDamage);
//                }
//            }
//            
//        }
//        
//        if (Input.GetKeyDown(KeyCode.Mouse0) && isProjectile)
//        {
//            anim.SetBool("isShooting", true);
//            Instantiate(projectile, projectileStart.position, projectileStart.rotation);
//            
//        }
    }

    void LateUpdate()
    {
        muzzleFlash.gameObject.SetActive(false);
        float horizontal = -Input.GetAxis("Mouse X") * swayAmount;
        float vertical = -Input.GetAxis("Mouse Y") * swayAmount;

        horizontal = Mathf.Clamp(horizontal, -maxSway, maxSway);
        vertical = Mathf.Clamp(vertical, -maxSway, maxSway);
        
        Vector3 finalSwayPos = new Vector3(horizontal, vertical, 0f);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalSwayPos + initialSwayPosition, swaySmoothValue * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
//            Instantiate(projectile, projectileStart.position, projectileStart.rotation);
            Vector3 rayOrigin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
            RaycastHit hitInfo = new RaycastHit();
           
            muzzleFlash.gameObject.SetActive(true);

            if (Physics.Raycast(rayOrigin, camera.transform.forward, out hitInfo, rayRange))
            {
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    hitInfo.collider.GetComponent<EnemyHealth>().takeDamage(gunDamage);
                }
            }
            
        }
        
    }
}
