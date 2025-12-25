using System;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using Windows.Security.Cryptography;

namespace MCLauncher
{
    class WUTokenHelper
    {

        private const int WU_ERRORS_START = unchecked((int)0x80040200);
        private const int WU_NO_ACCOUNT = unchecked((int)0x80040200);
        private const int WU_TOKEN_FETCH_ERROR_BASE = unchecked((int)0x80040300);
        private const int WU_TOKEN_FETCH_ERROR_END = unchecked((int)0x80040400);
        private const int WU_ERRORS_END = unchecked((int)0x80040400);

        /// <summary>
        /// Gets a Windows Update token for Microsoft Store authentication.
        /// This is a pure C# implementation using WinRT APIs, replacing the previous C++ DLL.
        /// </summary>
        public static string GetWUToken()
        {
            return GetWUTokenAsync().GetAwaiter().GetResult();
        }

        private static async Task<string> GetWUTokenAsync()
        {
            // Find the Microsoft account provider for consumers
            WebAccountProvider accountProvider = await WebAuthenticationCoreManager.FindAccountProviderAsync(
                "https://login.microsoft.com", "consumers");

            if (accountProvider == null)
            {
                throw new WUTokenException(WU_NO_ACCOUNT);
            }

            // Find all accounts associated with this provider
            FindAllAccountsResult accountsResult = await WebAuthenticationCoreManager.FindAllAccountsAsync(accountProvider);

            if (accountsResult.Status != FindAllWebAccountsStatus.Success || accountsResult.Accounts.Count == 0)
            {
                throw new WUTokenException(WU_NO_ACCOUNT);
            }

            // Use the first available account
            WebAccount account = accountsResult.Accounts[0];
            System.Diagnostics.Debug.WriteLine($"Account ID = {account.Id}");
            System.Diagnostics.Debug.WriteLine($"Account Name = {account.UserName}");

            // Create token request for Windows Update / Microsoft Store
            WebTokenRequest request = new WebTokenRequest(
                accountProvider,
                "service::dcat.update.microsoft.com::MBI_SSL",
                "{28520974-CE92-4F36-A219-3F255AF7E61E}");

            // Get the token silently (without UI)
            WebTokenRequestResult result = await WebAuthenticationCoreManager.GetTokenSilentlyAsync(request, account);

            if (result.ResponseStatus != WebTokenRequestStatus.Success)
            {
                int errorCode = WU_TOKEN_FETCH_ERROR_BASE | (int)result.ResponseStatus;
                throw new WUTokenException(errorCode);
            }

            // Get the token from the response
            string token = result.ResponseData[0].Token;
            System.Diagnostics.Debug.WriteLine($"Token = {token}");

            // Convert to UTF-16LE binary and then to Base64
            var tokenBinary = CryptographicBuffer.ConvertStringToBinary(token, BinaryStringEncoding.Utf16LE);
            string tokenBase64 = CryptographicBuffer.EncodeToBase64String(tokenBinary);
            System.Diagnostics.Debug.WriteLine($"Encoded token = {tokenBase64}");

            return tokenBase64;
        }

        public class WUTokenException : Exception
        {
            public WUTokenException(int exception) : base(GetExceptionText(exception))
            {
                HResult = exception;
            }

            private static String GetExceptionText(int e)
            {
                if (e >= WU_TOKEN_FETCH_ERROR_BASE && e < WU_TOKEN_FETCH_ERROR_END)
                {
                    var actualCode = (byte)e & 0xff;

                    if (!Enum.IsDefined(typeof(WebTokenRequestStatus), actualCode))
                    {
                        return $"WUTokenHelper returned bogus HRESULT: {e} (THIS IS A BUG)";
                    }
                    var status = (WebTokenRequestStatus)Enum.ToObject(typeof(WebTokenRequestStatus), actualCode);
                    switch (status)
                    {
                        case WebTokenRequestStatus.Success:
                            return "Success (THIS IS A BUG)";
                        case WebTokenRequestStatus.UserCancel:
                            return "User cancelled token request (THIS IS A BUG)";
                        case WebTokenRequestStatus.AccountSwitch:
                            return "User requested account switch (THIS IS A BUG)";
                        case WebTokenRequestStatus.UserInteractionRequired:
                            return "User interaction required to complete token request (THIS IS A BUG)";
                        case WebTokenRequestStatus.AccountProviderNotAvailable:
                            return "Xbox Live account services are currently unavailable";
                        case WebTokenRequestStatus.ProviderError:
                            return "Unknown Xbox Live error";
                    }
                }
                switch (e)
                {
                    case WU_NO_ACCOUNT: return "No Microsoft account found";
                    default: return "Unknown " + e;
                }
            }
        }
    }
}
