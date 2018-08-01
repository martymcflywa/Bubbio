using System.ComponentModel;

namespace Bubbio.Core.Contracts.Enums
{
    public enum UnitType
    {
        // metric
        [Description("g")]
        Gram,
        [Description("kg")]
        Kilogram,
        [Description("mm")]
        Millimetre,
        [Description("cm")]
        Centimetre,
        [Description("ml")]
        Millilitre,
        [Description("l")]
        Litre,
        // imperial
        [Description("oz")]
        Ounce,
        [Description("lb")]
        Pound,
        [Description("in")]
        Inch,
        [Description("ft")]
        Feet,
        [Description("fl oz")]
        FluidOunce
    }
}