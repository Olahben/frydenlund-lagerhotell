namespace Lagerhotell.Shared.Lib;
using System.Collections.Immutable;

public static class StorageUnitStaticData
{
    public enum StorageUnitInfoTexts
    {
        XSmall, //1-3M2
        Small, //4-8M2
        Medium, //9-12M2
        Large, //13-18M2
        XLarge //19-24M2
    }

    public static readonly ImmutableDictionary<StorageUnitInfoTexts, string> StorageUnitSizeDescriptions = new Dictionary<StorageUnitInfoTexts, string>
{
    { StorageUnitInfoTexts.XSmall, "I dette lageret vil du kunne få plass til møbler og annet diverse fra en liten/normal enromsleilighet eller fra ett vanlig soverom." },
    { StorageUnitInfoTexts.Small, "I dette lageret vil du kunne få plass til innholdet til en liten normalt møblert leilighet inkludert noen andre eiendeler." },
    { StorageUnitInfoTexts.Medium, "I dette lageret vil du kunne få plass til innholdet til en medium stor leilighet som er normalt møblert." },
    { StorageUnitInfoTexts.Large, "I dette lageret vil du kunne få plass til innholdet til en større leilighet som er normalt møblert." },
    { StorageUnitInfoTexts.XLarge, "I dette lageret vil du kunne få plass til innholdet til en større enmannsbolig som er normalt møblert i tillegg til fritidsutstyr." }
}.ToImmutableDictionary();
}