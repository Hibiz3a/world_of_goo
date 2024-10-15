using TMPro;
using UnityEngine;

public class EndLevelPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FirstGain;
    [SerializeField] private TextMeshProUGUI SecondGain;
    [SerializeField] private TextMeshProUGUI ThirdGain;

    public TextMeshProUGUI _FirstGain
    {
        get { return FirstGain; }
        set { FirstGain = value; }
    }

    public TextMeshProUGUI _SecondGain
    {
        get { return SecondGain; }
        set { FirstGain = value; }
    }

    public TextMeshProUGUI _ThirdGain
    {
        get { return ThirdGain; }
        set { ThirdGain = value; }
    }
}