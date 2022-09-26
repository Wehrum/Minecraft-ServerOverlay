using System.Diagnostics;

public static class Constants
{
    public static readonly string[] Gamemodes =
    {
        "easy",
        "normal",
        "hard",
        "peaceful"
    };

    public static readonly string[] Commands =
    {
        "!help",
        "!restart",
        "!sethome",
        "!delhome",
        "!home",
        "!homes",
        "!difficulty",
        "!tp"
    };

    public static readonly Colors Color = new Colors
    {
        Black = "§0",
        Dark_Blue = "§1",
        Dark_Green = "§2",
        Dark_Aqua = "§3",
        Dark_Red = "§4",
        Dark_Purple = "§5",
        Gold = "§6",
        Gray = "§7",
        Dark_Gray = "§8",
        Blue = "§9",
        Green = "§a",
        Aqua = "§b",
        Red = "§c",
        Purple = "§d",
        Yellow = "§e",
        White = "§f"
    };

    public static readonly Format Format = new Format
    {
        Obfuscated = "§k",
        Bold = "§l",
        Strikethrough = "§m",
        Underline = "§n",
        Italic = "§o",
        Reset = "§r"
    };
}
