using Microsoft.JSInterop;

namespace TangyWeb_Client.Helper
{
    public static class IJSRuntimeExtension
    {
        public static async ValueTask ToastrSuccess(this IJSRuntime iJSRuntimeExtension, string message)
        {
            await iJSRuntimeExtension.InvokeVoidAsync("ShowToastr", "success", message);
        }

        public static async ValueTask ToastrError(this IJSRuntime iJSRuntimeExtension, string message)
        {
            await iJSRuntimeExtension.InvokeVoidAsync("ShowToastr", "error", message);
        }
    }
}
