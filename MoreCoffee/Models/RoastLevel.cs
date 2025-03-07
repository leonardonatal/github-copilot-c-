using System.ComponentModel;

namespace MoreCoffee.Models;

/// <summary>
/// Represents the different levels of coffee roast.
/// </summary>
public enum RoastLevel
{
    /// <summary>
    /// Light Roast
    /// </summary>
    [Description("Light Roast")]
    Light,

    /// <summary>
    /// Medium Light Roast
    /// </summary>
    [Description("Medium Light Roast")]
    MediumLight,

    /// <summary>
    /// Medium Roast
    /// </summary>
    [Description("Medium Roast")]
    Medium,

    /// <summary>
    /// Medium Dark Roast
    /// </summary>
    [Description("Medium Dark Roast")]
    MediumDark,

    /// <summary>
    /// Dark Roast
    /// </summary>
    [Description("Dark Roast")]
    Dark,

    /// <summary>
    /// Italian Roast
    /// </summary>
    [Description("Italian Roast")]
    Italian
}
