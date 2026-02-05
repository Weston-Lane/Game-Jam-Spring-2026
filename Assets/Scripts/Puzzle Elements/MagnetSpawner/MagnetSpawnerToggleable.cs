using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(GameObject))]
public class MagnetSpawnerToggleable : MonoBehaviour
{
    #region Inspector Objects
    [Header("Object References")]
    [SerializeField] GameObject magnetPreFab;
    [SerializeField] Transform spawnPos;
    [Header("If Magnet already in scene. Use this to \nmake sure multiple aren't spawned")]
    [SerializeField] GameObject spawnerMagnet;

    [Header("Configuration Variables")]
    [SerializeField] BaseMagnet.Polarity spawnMagnetPolarityNorth;
    [SerializeField] BaseMagnet.Polarity spawnMagnetPolaritySouth;
    [SerializeField] float polePower;
    [SerializeField] float fieldRadius;

    [Header("State")]
    bool isMagnetSpawned = true;
    #endregion

    public void SpawnMagnet()
    {
        if(spawnerMagnet == null)
        {
            spawnerMagnet = Spawn();  
        }
        else
        {
            Destroy(spawnerMagnet);
            spawnerMagnet = Spawn();
        }
    }
    GameObject Spawn()
    {
        var magnetObj = Instantiate(magnetPreFab,
                spawnPos.transform.position,
                spawnPos.transform.rotation);
        MagnetPole[] poles = magnetObj.GetComponent<BaseMagnet>().GetPoles();

        foreach (var pole in poles)
        {
            pole.SetPower(polePower);
            pole.SetFieldRadius(fieldRadius);
        }

        poles[0].ChangePolarity(spawnMagnetPolarityNorth);
        poles[1].ChangePolarity(spawnMagnetPolaritySouth);

        return magnetObj;
    }
}
