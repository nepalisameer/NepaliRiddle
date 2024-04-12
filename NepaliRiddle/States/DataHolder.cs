using Blazored.LocalStorage;
using NepaliRiddle.Constants;
using NepaliRiddle.Models;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace NepaliRiddle.States
{
    public class DataHolder
    {
        private readonly IServiceProvider _serviceProvider;
        private bool _loaded = false;
        public int CurrentQuestionIndex { get; private set; }
        public List<QuestionBank> Questions { get; private set; }
        public int TotalQuestions { get; private set; }
        private List<string> _duckImages =
[
    "/images/duck1.jpeg",
    "/images/duck2.jpeg",
    "/images/duck3.jpeg",
];
        public string GetRandomDuckImage => _duckImages[Random.Shared.Next(_duckImages.Count)];
        public DataHolder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task LoadQuestions()
        {
            if (!_loaded)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var http = scope.ServiceProvider.GetRequiredService<HttpClient>();
                    Questions = (await http.GetFromJsonAsync<List<QuestionBank>>("nepaliriddle2.json"))!;
                    TotalQuestions = Questions.Count;
                    _loaded = true;
                }
            }
        }
        public async Task LoadAndSuffleQuestions()
        {
            await LoadQuestions();
            if (Questions != null)
            {
                Questions = [.. Questions.OrderBy(x => Guid.NewGuid())];
            }
        }
        public async Task Reset()
        {
            _loaded = false;
            await LoadQuestions();
        }
        /// <summary>
        /// saving locally and data is not not associated with individual user(not ideal for actual game)
        /// instead it is available for all users who access through same browser
        /// 
        /// </summary>
        public async Task SaveData(QuestionDataHolder dataHolder)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
                if (_localStorage != null && dataHolder != null)
                {
                    await _localStorage.SetItemAsync(LocalStorageConstant.SavedGame, dataHolder);
                }
            }
        }
        public async Task<QuestionDataHolder?> LoadData()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
                if (_localStorage != null)
                {
                    if (await _localStorage.ContainKeyAsync(LocalStorageConstant.SavedGame))
                    {
                        var a = await _localStorage.GetItemAsync<QuestionDataHolder>(LocalStorageConstant.SavedGame);
                        if (a != null)
                        {
                            Questions = a.Questions;
                            TotalQuestions = Questions.Count;
                            CurrentQuestionIndex = a.TotalPlayedQuestions;
                            _loaded = true;
                            return a;
                        }
                    }
                }
                return null;
            }
        }
        public async Task<bool> IsSavedDataAvailable()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
                if (_localStorage != null)
                {
                    if (await _localStorage.ContainKeyAsync(LocalStorageConstant.SavedGame))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
    public class QuestionDataHolder
    {
        public List<QuestionBank> Questions { get; set; }
        public int TotalPlayedQuestions { get; set; }
        public int TotalCorrectAnswers { get; set; }
    }
}
