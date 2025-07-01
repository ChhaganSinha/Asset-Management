using Microsoft.JSInterop;

namespace AssetManagement.Client.Services
{
    public class BrowserService
    {
        private readonly IJSRuntime _js;
        BrowserDimension _browserDimension;
        public event Action<BrowserDimension> DimensionsChanged;

        public BrowserService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<BrowserDimension> GetDimensions()
        {
            _browserDimension = await _js.InvokeAsync<BrowserDimension>("getDimensions");
            return _browserDimension;
        }

        public async Task RegisterResizeListener()
        {
            await _js.InvokeVoidAsync("window.registerViewportChangeCallback", DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public void OnResize(int width, int height)
        {
            if (_browserDimension.Width == width && _browserDimension.Height == height) return;
            _browserDimension.Width = width;
            _browserDimension.Height = height;
            DimensionsChanged?.Invoke(_browserDimension);
        }

    }

    public class BrowserDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
