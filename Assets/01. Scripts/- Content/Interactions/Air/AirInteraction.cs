using Polyperfect.Common;
using UnityEngine;

public class AirInteraction : InteractionObject
{
    [SerializeField] private Material _airOnMat;
    [SerializeField] private Material _airOffMat;
    [SerializeField] private Material _airClearMat;
    [SerializeField] private MeshRenderer _airMesh;

    private AirImteractionManager _airManager;
    private Material[] _airMats;
    public bool IsOn { get; private set; } = false;
    private bool _isClear = false;

    private void Start()
    {
        _airMats = _airMesh.sharedMaterials;
    }

    public void Initialize(AirImteractionManager manager)
    {
        _airManager = manager;
    }

    public override void Interaction()
    {
        base.Interaction();

        if (_isClear) 
            return;

        if (IsOn)
        {
            _airMats[0] = _airOffMat;
            _airMats[1] = _airOffMat;
        }
        else
        {
            _airMats[0] = _airOffMat;
            _airMats[1] = _airOnMat;
        }

        _airMesh.sharedMaterials = _airMats;
        IsOn = _airMats[1] != _airOffMat;
        _airManager.CheckAirCondition();
    }

    public void AirClear()
    {
        _airMats[0] = _airOffMat;
        _airMats[1] = _airClearMat;

        _airMesh.sharedMaterials = _airMats;
        GetComponent<BoxCollider>().enabled = false;
        _isClear = true;
    }
}
