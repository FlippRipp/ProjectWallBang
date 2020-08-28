using UnityEngine;
[CreateAssetMenu(menuName = "BallisticMaterial/Material", fileName = "Material")]
public class WallMaterial : ScriptableObject
{
    public float hitPoints = 50f;
    public float damageAbsorption = 20f;
    public AudioClip hitSound;
}
