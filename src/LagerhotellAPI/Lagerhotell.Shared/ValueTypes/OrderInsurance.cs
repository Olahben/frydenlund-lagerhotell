using System.ComponentModel;

namespace LagerhotellAPI.Models.ValueTypes;

public enum OrderInsurance
{
    [Description("None")]
    None,
    [Description("50000KR")]
    FiftyThousand,
    [Description("100000KR")]
    OneHundredThousand,
    [Description("200000KR")]
    TwoHundredThousand,
    [Description("300000KR")]
    ThreeHundredThousand,
}
