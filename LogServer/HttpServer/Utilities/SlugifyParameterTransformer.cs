using System.Text.RegularExpressions;

namespace HttpServer.Utilities;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object? value)
    {
        string result = string.Empty;

        if (value is not null)
        {
            result = Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower();
        }

        return result;
    }
}