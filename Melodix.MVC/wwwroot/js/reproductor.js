
document.addEventListener('DOMContentLoaded', function () {
    const audio = document.getElementById('song-player');
    const source = document.getElementById('song-src');
    const playPauseBtn = document.getElementById('play-pause-btn');
    const progressBar = document.getElementById('progress-bar');
    const currentTimeText = document.getElementById('current-time');
    const totalTimeText = document.getElementById('total-time');
    const volumeBar = document.getElementById('volume-bar');
    const downloadLink = document.getElementById('download-link');

    const rows = Array.from(document.querySelectorAll('.cancion-row'));
    let currentIndex = 0;

    function formatTime(seconds) {
        const min = Math.floor(seconds / 60);
        const sec = Math.floor(seconds % 60).toString().padStart(2, '0');
        return `${min}:${sec}`;
    }

    function reproducir(index) {
        if (index < 0 || index >= rows.length) return;
        const row = rows[index];

        document.getElementById('song-name').innerText = row.dataset.nombre;
        document.getElementById('song-artist').innerText = row.dataset.artista;
        document.getElementById('song-genre').innerText = row.dataset.genero;
        document.getElementById('cover-image').src = row.dataset.imagen;
        source.src = row.dataset.archivo;
        if (downloadLink) downloadLink.href = row.dataset.archivo;
        audio.load();
        audio.play();
        playPauseBtn.innerText = "⏸";
        currentIndex = index;
    }

    audio.addEventListener('timeupdate', () => {
        progressBar.value = audio.currentTime;
        currentTimeText.innerText = formatTime(audio.currentTime);
    });

    audio.addEventListener('loadedmetadata', () => {
        progressBar.max = audio.duration;
        totalTimeText.innerText = formatTime(audio.duration);
    });

    audio.addEventListener('ended', () => {
        reproducir(currentIndex + 1);
    });

    progressBar.addEventListener('input', () => {
        audio.currentTime = progressBar.value;
    });

    volumeBar.addEventListener('input', () => {
        audio.volume = volumeBar.value;
    });

    playPauseBtn.addEventListener('click', () => {
        if (audio.paused) {
            audio.play();
            playPauseBtn.innerText = "⏸";
        } else {
            audio.pause();
            playPauseBtn.innerText = "▶️";
        }
    });

    document.getElementById('prev-btn').addEventListener('click', () => {
        reproducir(currentIndex - 1);
    });

    document.getElementById('next-btn').addEventListener('click', () => {
        // Si es canción aleatoria (por ejemplo en vista Index de canciones)
        const random = document.getElementById('song-player')?.dataset.random === 'true';
        if (random) {
            const aleatorio = Math.floor(Math.random() * rows.length);
            reproducir(aleatorio);
        } else {
            reproducir(currentIndex + 1);
        }
    });

    rows.forEach((row, index) => {
        row.addEventListener('click', () => reproducir(index));
        if (row.dataset.archivo === source.getAttribute('src')) currentIndex = index;
    });
});
