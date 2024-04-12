
window.PlayAudioFile = (src) => {
    var sound = document.createElement('audio');
    sound.onended = function () { sound.remove(); };
    sound.src = src;
    sound.type = 'audio/mpeg';
    document.body.appendChild(sound);
    sound.load();
    sound.play();
}

