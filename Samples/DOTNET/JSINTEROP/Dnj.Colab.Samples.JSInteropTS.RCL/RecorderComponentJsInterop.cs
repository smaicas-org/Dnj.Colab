using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dnj.Colab.Samples.JSInteropTS.RCL;

public class RecorderComponentJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    private readonly TaskCompletionSource<bool> _isModuleTaskLoaded = new(false);
    private readonly IJSRuntime _jsRuntime;

    public RecorderComponentJsInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Dnj.Colab.Samples.JSInteropTS.RCL/RecorderComponentJsInterop.js").AsTask());
        _isModuleTaskLoaded.SetResult(true);
    }

    public async ValueTask StartRecording()
    {
        await _isModuleTaskLoaded.Task;
        await _moduleTask.Value;
        await _jsRuntime.InvokeAsync<string>("RecorderComponentJs.StartRecording");
    }
    public async ValueTask<object> StopRecording()
    {
        await _isModuleTaskLoaded.Task;
        await _moduleTask.Value;
        return await _jsRuntime.InvokeAsync<object>("RecorderComponentJs.StopRecording");
    }
    public async ValueTask<string> SetAudioSource(ElementReference elRef)
    {
        await _isModuleTaskLoaded.Task;
        await _moduleTask.Value;
        return await _jsRuntime.InvokeAsync<string>("RecorderComponentJs.SetAudioSource", elRef);
    }
    public async ValueTask<string> VisualizeCanvas(ElementReference canvasElementReference)
    {
        await _isModuleTaskLoaded.Task;
        await _moduleTask.Value;
        return await _jsRuntime.InvokeAsync<string>("RecorderComponentJs.VisualizeCanvas", canvasElementReference);
    }
    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            IJSObjectReference module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
