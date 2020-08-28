using UnityEngine;

public class WallPercingShot : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField]
    private float bulletDamage = 10f;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit[] hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            hit = Physics.RaycastAll(ray);
            
            if(hit.Length > 0)
            {
                GameObject[] hitObjects = new GameObject[hit.Length];
                for (int i = 0; i < hit.Length; i++)
                {
                    hitObjects[i] = hit[i].collider.gameObject;
                }
                
                hitObjects = SortObjectsByDistance.SortByDistance(hitObjects, transform.position);

                float damageLeft = bulletDamage;
                for (int i = 0; i < hitObjects.Length; i++)
                {
                    Debug.Log(Vector3.Distance(hitObjects[i].transform.position, transform.position));
                    hitObjects[i].transform.parent.gameObject.GetComponent<Wall>().OnHit(ref damageLeft);
                    if(damageLeft <= 0) break;
                }
            }
        }

    }
}
