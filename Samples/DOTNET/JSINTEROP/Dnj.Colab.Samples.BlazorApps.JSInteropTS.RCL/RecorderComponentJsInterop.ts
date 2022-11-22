// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.
import { RecorderComponentModule } from './RecorderComponentJs.js';

namespace DnjNetVoiceToTextComponents {

    export function Load(): void {
        window['RecorderComponentJs'] = new RecorderComponentModule.RecorderComponentJsClass();
    }
}

DnjNetVoiceToTextComponents.Load();
