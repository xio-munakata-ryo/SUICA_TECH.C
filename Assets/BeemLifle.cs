using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeemLifle : MonoBehaviour
{

    [SerializeField]
    private float _offsetSec = 5f;

    [SerializeField]
    private float _beemLifeTimeSec = 3f;

    [SerializeField]
    private GameObject _prefadBeem = null;

    private GameObject _instanseBeem = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] arrHits 
            = Physics.RaycastAll(this.transform.position, this.transform.transform.forward);
        
        float yLong = 500f;
        if(arrHits != null && arrHits.Length > 0)
        {
            // ’·‚³
            yLong = (arrHits[0].point = this.transform.position).magnitude;

        }

        // •ûŒü
        Quaternion rot = this.transform.rotation * Quaternion.Euler(90f, 0f, 0f);

        if (_instanseBeem == null)
        {
            _instanseBeem = Instantiate(_prefadBeem);
        }

        // ’·‚³‚ğ•Ï‚¦‚é
        _instanseBeem.transform.localEulerAngles = new Vector3(2f, yLong, 2f);
        // •ûŒü‚ğ•Ï‚¦‚é
        _instanseBeem.transform.rotation = rot;
        // “V’¸•ûŒü‚ÉŒü‚©‚Á‚ÄA’·‚³‚Ì”¼•ªˆÚ“®
        _instanseBeem.transform.position 
            = this.transform.position + _instanseBeem.transform.up * yLong * 0.5f;
    }
}
