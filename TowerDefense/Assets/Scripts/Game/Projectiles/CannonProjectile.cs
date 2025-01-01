using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CannonProjectile : MonoBehaviour
{

    private float _elapsedTime = 0f;

    private Vector3 _projectileDirectionXZ;

     private float _v0= 0;

    private float _vx0;

    private float _vy0;

    private float _vy;

    [SerializeField]
    private float _fixedTravelTime = 1.0f;


    public void Initialize(Vector3 projectileDirectionXZ)
    {
        _projectileDirectionXZ = projectileDirectionXZ;
        CalculateInitialVelocity();
        _vy = _vy0;
    }

    private void CalculateInitialVelocity()
    {
        //var _horTravelDistance = Vector3.Magnitude(_projectileDirectionXZ)*1.6f;

        var _horTravelDistance = Vector3.Magnitude(_projectileDirectionXZ)*1.45f;

        _vx0 = _horTravelDistance/_fixedTravelTime;//_horTravelDistance/tw

        var tw = 0.0f;
        
        _vy0 = 1;

        while(tw < _fixedTravelTime/2)
        {
            tw = (_vy0 + Mathf.Sqrt(_vy0 *_vy0 + 2 * 9.81f* 1.2f))/ 9.81f;
            _vy0 += 1;
        }

        Debug.Log("TW = " + tw);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        _vy = -9.81f * _elapsedTime + _vy0;

        var projectileDirectionXZN =_projectileDirectionXZ.normalized;

        var _horVelocity =  projectileDirectionXZN * _vx0;

        GetComponent<Rigidbody>().velocity = new Vector3(_horVelocity.x, _vy,_horVelocity.z);

        //GetComponent<Rigidbody>().velocity = new Vector3(0, _vy,0);
    }
}
