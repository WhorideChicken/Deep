using UnityEngine;

public class AirImteractionManager : MonoBehaviour
{
    [SerializeField] private AirInteraction[] _airs;
    [SerializeField] private Dialog _airDialog;
    [SerializeField] private Dialog _successDialog;
    private bool _isFirstInteraction = false;

    private void Awake()
    {
        foreach (var air in _airs)
        {
            air.Initialize(this);
        }
    }

    public void CheckAirCondition()
    {
        if (!_isFirstInteraction)
        {
            CanvasManager.Instance.StartDialog(_airDialog);
            _isFirstInteraction = true;
            return;
        }

        if ((_airs[0].IsOn && _airs[2].IsOn) && (!_airs[1].IsOn && !_airs[3].IsOn))
        {
            foreach (var air in _airs)
            {
                air.AirClear();
            }

            GameManager.Instance.OxygenDone = true;
            CanvasManager.Instance.StartDialog(_successDialog);
        }
    }
}
