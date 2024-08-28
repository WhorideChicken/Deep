using UnityEngine;

public class AirImteractionManager : MonoBehaviour
{
    [SerializeField] AirInteraction[] airs;
    [SerializeField] Dialog aorDialog;

    private bool IsFirst = false;

    private void Awake()
    {
        for(int i = 0; i < airs.Length; i++) 
        {
            airs[i].Initlaize(this.GetComponent<AirImteractionManager>());
        }
    }

    public void CheckAirCondition()
    {
        if(!IsFirst)
        {
            CanvasManager.instance.ScreenStartDialog(aorDialog);
            IsFirst = true;
            return;
        }

        if ((airs[0]._isOn && airs[2]._isOn) && (!airs[1]._isOn && !airs[3]._isOn))
        {
            Debug.Log("정답");
            for (int i = 0; i < airs.Length; i++)
            {
                airs[i].AirClear();
            }

            //GameManager.Instance._scenarioEnvets.Enqueue(() =>
            //{
            //    CanvasManager.instance.ScreenStartDialog("숨 쉬는게 편해졌다...");
            //});
            //
            //
            //GameManager.Instance.StartScenarioEvents();
        }
        else
        {
            Debug.Log("오답");
        } 

    }
}
