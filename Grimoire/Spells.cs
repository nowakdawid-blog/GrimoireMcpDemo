using System.ComponentModel;
using ModelContextProtocol.Server;

namespace Grimoire;

[McpServerToolType]
public static class Spells
{
    [McpServerTool(Name = "summon_familiar")]
    [Description("Summon a small animal companion. Novices usually get a cat.")]
    public static string SummonFamiliar(
        [Description("Kind of animal, for example cat, owl or toad")] string animal = "cat")
    {
        return $"A sleepy {animal} appears in a puff of smoke. It looks at you and yawns.";
    }

    [McpServerTool(Name = "levitate")]
    [Description("Lift an object into the air. Heavy objects are risky for novices.")]
    public static string Levitate(
        [Description("The object to lift")] string target,
        [Description("Approximate weight of the object in kilograms")] double weightKg)
    {
        return weightKg > 20
            ? $"You try to lift {target}. It does not move. You get a nosebleed and sit down."
            : $"{target} floats up slowly and wobbles in the air.";
    }
}
