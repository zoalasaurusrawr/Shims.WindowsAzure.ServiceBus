using Azure.Core;
using Azure.Identity;

namespace Microsoft.ServiceBus;
public class TokenProvider
{
    public static TokenProvider CreateTokenProvider()
    {
        return new TokenProvider();
    }

    public TokenCredential GetToken()
    {
        return new DefaultAzureCredential();
    }
}
