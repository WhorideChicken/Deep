using Polyperfect.Common;
using UnityEngine;

public class AirInteraction : InteractionObject
{
    private AirImteractionManager _airManager;

    public bool IsOn { get; private set; } = false;
    private bool _isClear = false;
    private MaterialPropertyBlock _propertyBlock;

    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] Color _airOn;
    [SerializeField] Color _airOff;
    [SerializeField] Color _airClear;
    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_propertyBlock); // 초기 상태 가져오기
        _propertyBlock.SetColor("_Color", _airOff); // 초기 색상 설정
        _renderer.SetPropertyBlock(_propertyBlock); // 설정 적용

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

        _renderer.GetPropertyBlock(_propertyBlock);

        if (IsOn)
        {
            Debug.Log("call on");

            _propertyBlock.SetColor("_BaseColor", _airOff);
            _propertyBlock.SetColor("_EmissionColor", _airOff * 0.5f);
            IsOn = false;
        }
        else
        {
            Debug.Log("call off");

            _propertyBlock.SetColor("_BaseColor", _airOn);
            _propertyBlock.SetColor("_EmissionColor", Color.black);
            IsOn = true;
        }

        _renderer.SetPropertyBlock(_propertyBlock);
        _airManager.CheckAirCondition();
    }

    public void AirClear()
    {
        GetComponent<BoxCollider>().enabled = false;
        _propertyBlock.SetColor("_BaseColor", _airClear);
        _propertyBlock.SetColor("_EmissionColor", Color.black);
        _renderer.SetPropertyBlock(_propertyBlock);
        _isClear = true;
    }
}
