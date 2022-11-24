// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.
import { RecorderComponentModule } from './RecorderComponentJs.js';
var DnjNetVoiceToTextComponents;
(function (DnjNetVoiceToTextComponents) {
    function Load() {
        window['RecorderComponentJs'] = new RecorderComponentModule.RecorderComponentJsClass();
    }
    DnjNetVoiceToTextComponents.Load = Load;
})(DnjNetVoiceToTextComponents || (DnjNetVoiceToTextComponents = {}));
DnjNetVoiceToTextComponents.Load();
//# sourceMappingURL=RecorderComponentJsInterop.js.map