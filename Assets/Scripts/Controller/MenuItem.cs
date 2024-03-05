using System;
using UnityEngine.UI;

[Serializable]
public struct MenuItem<TEnum>
{
    public Button Button;
    public TEnum Value;
}
