using UnityEngine;
using DG.Tweening;

public class LaserLine : MonoBehaviour
{
    public Transform origin;  
    public float maxDistance = 10f;
    public GameObject hitParticle;
    public LineRenderer lineRenderer;
    public Color debugRayColor = Color.red;

    [SerializeField] private Transform _laserPort;
    [SerializeField] private Transform _laserArm;

    private Sequence _laserSequence;
    private bool _isWork = true;
    private void Start()
    {
        StartLaserPort();
    }
    void Update()
    {
        if (!_isWork)
            return;

        RaycastHit hit;

        if (Physics.Raycast(origin.position, origin.forward, out hit, maxDistance))
        {
            hitParticle.transform.position = hit.point;
            hitParticle.SetActive(true);
            DrawLine(origin.position, hit.point);
        
            if(hit.collider.gameObject.GetComponent<CommonCharacterController>() != null ) 
            {
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            ClearLine();
            hitParticle.SetActive(false);
            //DrawLine(origin.position, origin.forward * maxDistance);
        }

        //Debug.DrawRay(origin.position, origin.forward * maxDistance, debugRayColor);
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.positionCount = 2;  
        lineRenderer.SetPosition(0, start);  
        lineRenderer.SetPosition(1, end); 
    }

    private void ClearLine()
    {
        lineRenderer.positionCount = 0;
        hitParticle.SetActive(false);
    }

    //[RND]
    //TODO : N초 동안 port와 arm이 랜덤한 목표로 회전
    // 완료 되면 다시 랜덤한 각도를 주고 회전 시작 : 재귀가 최선일까?
    //틀 : X축 -45 ~ -150
    //암 : Y축 - 90 ~ 90
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
           .OnComplete(() =>
           {
               StartLaserPort();
           });
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
        _isWork = false;
        ClearLine();
        if (_laserSequence != null)
        {
            _laserSequence.Kill();
        }
    }
}
