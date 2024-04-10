using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using NepaliRiddle.Constants;

namespace NepaliRiddle.States
{
    public class GameStateManager
    {
        public event Action? GameStateChanged;
        public bool GameStarted { get; private set; }
        public bool AudioMuted { get; private set; }
        private List<string> _duckImages  =
        [
            "/images/duck1.jpeg",
            "/images/duck2.jpeg",
            "/images/duck3.jpeg",
        ];
        public string GetRandomDuckImage => _duckImages[Random.Shared.Next(_duckImages.Count)];
        public bool AllowMultipleSlection { get; set; }
        private readonly ILocalStorageService _localStorage;
        public GameStateManager(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }
        public void StartGame()
        {
            GameStarted = true;
            GameStateChanged?.Invoke();
        }

        public async Task CheckLocalStorageAsync()
        {
            if (_localStorage != null)
            {
                if (await _localStorage.ContainKeyAsync(LocalStorageConstant.AudioState))
                {
                    AudioMuted = await _localStorage.GetItemAsync<bool>(LocalStorageConstant.AudioState);
                }
            }
        }
        public async Task MuteAudioAsync(bool muteOrNot)
        {
            if (_localStorage != null)
            {
                AudioMuted = muteOrNot;
                await _localStorage.SetItemAsync(LocalStorageConstant.AudioState, AudioMuted);
            }
        }
    }
}
