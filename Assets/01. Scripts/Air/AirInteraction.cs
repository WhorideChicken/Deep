using Polyperfect.Common;
using UnityEngine;

public class AirInteraction : InteractionObject
{
    [SerializeField] private Material _airOnMat;
    [SerializeField] private Material _airOffMat;
    [SerializeField] private Material _airClearMat;
    [SerializeField] private MeshRenderer _airMesh;

    private AirImteractionManager _airMng;

    private Material[] airMats;
    public bool _isOn = false;
    private bool _isClear = false;

    private void Start()
    {
        airMats = _airMesh.sharedMaterials;
    }

    public void Initlaize(AirImteractionManager manager)
    {
        _airMng = manager;
    }

    public override void Interaction()
    {
        base.Interaction();

        if (_isClear)
            return;

        if(_isOn)
        {
            airMats[0] = _airOffMat;
            airMats[1] = _airOffMat;
        }
        else
        {
            airMats[0] = _airOffMat;
            airMats[1] = _airOnMat;
        }

        _airMesh.sharedMaterials = airMats;
        _isOn = airMats[1] != _airOffMat ? true : false;

        _airMng.CheckAirCondition();
    }

    public void AirClear()
    {
        airMats[0] = _airOffMat;
        airMats[1] = _airClearMat;

        _airMesh.sharedMaterials = airMats;
        this.GetComponent<BoxCollider>().enabled = false;
    }

}
