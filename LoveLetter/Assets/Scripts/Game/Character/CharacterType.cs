using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum CharacterType
{
    Spy,
    Guard,
    Priest,
    Baron,
    Handmaid,
    Prince,
    Chancellor,
    King,
    Countess,
    Princess
}