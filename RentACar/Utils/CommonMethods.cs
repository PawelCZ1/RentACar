namespace RentACar.Utils;

public static class CommonMethods
{
    public static T StringToEnum<T>(string value) where T : struct
    {
        return Enum.TryParse<T>(value, out var result) ? result : default;
    }
    
}