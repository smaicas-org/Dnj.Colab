/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dnj.Colab.Samples.JSInteropTS.RCL;

public class RecorderComponentJsInterop : IAsyncDisposable
{
    private readonly TaskCompletionSource<bool> _isModuleTaskLoaded = new(false);
    private readonly IJSRuntime _jsRuntime;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public RecorderComponentJsInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Dnj.Colab.Samples.JSInteropTS.RCL/RecorderComponentJsInterop.js").AsTask());
        _isModuleTaskLoaded.SetResult(true);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            IJSObjectReference module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
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
}