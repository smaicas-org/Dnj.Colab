var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
export var RecorderComponentModule;
(function (RecorderComponentModule) {
    class RecorderComponentJsClass {
        RecordAudio() {
            return new Promise((resolve) => __awaiter(this, void 0, void 0, function* () {
                this._currentStream = yield navigator.mediaDevices.getUserMedia({ audio: true });
                const mediaRecorder = new MediaRecorder(this._currentStream);
                let audioChunks = [];
                mediaRecorder.addEventListener('dataavailable', event => {
                    audioChunks.push(event.data);
                });
                const start = () => {
                    audioChunks = [];
                    mediaRecorder.start();
                };
                const stop = () => new Promise(resolve => {
                    mediaRecorder.addEventListener('stop', () => {
                        const audioBlob = new Blob(audioChunks, { type: 'audio/mpeg' });
                        const audioUrl = URL.createObjectURL(audioBlob);
                        const audio = new Audio(audioUrl);
                        const play = () => audio.play();
                        resolve({ audioChunks, audioBlob, audioUrl, play });
                    });
                    mediaRecorder.stop();
                });
                resolve({ start, stop });
            }));
        }
        StartRecording() {
            return __awaiter(this, void 0, void 0, function* () {
                if (!this._recorder) {
                    this._recorder = yield this.RecordAudio();
                }
                this._recorder.start();
            });
        }
        StopRecording() {
            return __awaiter(this, void 0, void 0, function* () {
                this._audio = yield this._recorder.stop();
                return this._audio;
            });
        }
        SetAudioSource(element) {
            return __awaiter(this, void 0, void 0, function* () {
                element.src = this._audio.audioUrl;
                return this._audio.audioUrl;
            });
        }
        VisualizeCanvas(canvasElement) {
            return __awaiter(this, void 0, void 0, function* () {
                let stream = this._currentStream;
                const canvasCtx = canvasElement.getContext("2d");
                let audioCtx = new AudioContext();
                //canvasElement.style.backgroundColor
                const source = audioCtx.createMediaStreamSource(stream);
                const analyser = audioCtx.createAnalyser();
                analyser.fftSize = 2048;
                const bufferLength = analyser.frequencyBinCount;
                const dataArray = new Uint8Array(bufferLength);
                source.connect(analyser);
                //analyser.connect(audioCtx.destination);
                draw();
                function draw() {
                    const WIDTH = canvasElement.width;
                    const HEIGHT = canvasElement.height;
                    requestAnimationFrame(draw);
                    analyser.getByteTimeDomainData(dataArray);
                    // Retrieve real background color of canvas to apply for canvas fill.
                    // Required to apply canvas color from element class. Use class="bg-whatever" to apply
                    // color to canvas. Default color white if not specified.
                    let canvasBgColor = getComputedStyle(canvasElement).getPropertyValue('background-color');
                    canvasCtx.fillStyle = canvasBgColor || 'rgb(255,255,255)';
                    canvasCtx.fillRect(0, 0, WIDTH, HEIGHT);
                    canvasCtx.lineWidth = 2;
                    canvasCtx.strokeStyle = 'rgb(0, 0, 0)';
                    canvasCtx.beginPath();
                    let sliceWidth = WIDTH * 1.0 / bufferLength;
                    let x = 0;
                    for (let i = 0; i < bufferLength; i++) {
                        let v = dataArray[i] / 128.0;
                        let y = v * HEIGHT / 2;
                        if (i === 0) {
                            canvasCtx.moveTo(x, y);
                        }
                        else {
                            canvasCtx.lineTo(x, y);
                        }
                        x += sliceWidth;
                    }
                    canvasCtx.lineTo(canvasElement.width, canvasElement.height / 2);
                    canvasCtx.stroke();
                }
            });
        }
    }
    RecorderComponentModule.RecorderComponentJsClass = RecorderComponentJsClass;
    function getCSS(element) {
        var css_data = '';
        var css_obj = getComputedStyle(element);
        for (var i = 0; i < css_obj.length; i++) {
            css_data +=
                css_obj[i] + ':' +
                    css_obj.getPropertyValue(css_obj[i])
                    + ';<br>';
        }
        return css_data;
    }
    RecorderComponentModule.getCSS = getCSS;
})(RecorderComponentModule || (RecorderComponentModule = {}));
//# sourceMappingURL=RecorderComponentJs.js.map