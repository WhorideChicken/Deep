using UnityEngine;
using DG.Tweening;

public class LaserLine : MonoBehaviour
{
    [SerializeField] private Transform _origin;
    [SerializeField] private Transform _laserPort;
    [SerializeField] private Transform _laserArm;
    [SerializeField] private float _maxDistance = 10f;
    [SerializeField] private GameObject _hitParticle;
    [SerializeField] private LineRenderer _lineRenderer;

    private Sequence _laserSequence;
    private bool _isWorking = true;

    private void Start()
    {
        StartLaserPort();
    }

    private void Update()
    {
        if (!_isWorking) return;

        RaycastHit hit;
        if (Physics.Raycast(_origin.position, _origin.forward, out hit, _maxDistance))
        {
            _hitParticle.transform.position = hit.point;
            _hitParticle.SetActive(true);
            DrawLine(_origin.position, hit.point);

            if (hit.collider.gameObject.GetComponent<CommonCharacterController>() != null)
            {
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            ClearLine();
            _hitParticle.SetActive(false);
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }

    private void ClearLine()
    {
        _lineRenderer.positionCount = 0;
        _hitParticle.SetActive(false);
    }

    private void StartLaserPort()
    {
        float portX = Random.Range(-45, -150);
        float portY = Random.Range(-90, 90);
        float time = Random.Range(1, 4);

        if (_laserSequence != null)
        {
            _laserSequence.Kill();
        }

        _laserSequence = DOTween.Sequence()
            .Join(_laserArm.DOLocalRotate(new Vector3(portX, 0, 0), time))
            .Join(_laserPort.DOLocalRotate(new Vector3(0, portY, 0), time))
            .OnComplete(StartLaserPort);
    }

    private void OnDestroy()
    {
        if (_laserSequence != null)
        {
            _laserSequence.Kill();
        }
    }

    public void StopLaserSystem()
    {
        _isWorking = false;
        ClearLine();

        if (_laserSequence != null)
        {
            _laserSequence.Kill();
        }
    }
}
