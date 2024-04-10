using NepaliRiddle.Models;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace NepaliRiddle.States
{
    public class QuestionsHolder
    {
        private readonly IServiceProvider _serviceProvider;
        private bool _loaded = false;

        public List<QuestionBank> Questions { get; private set; }
        public int TotalQuestions {  get; private set; }
        public QuestionsHolder(IServiceProvider serviceProvider)
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
            if(Questions != null)
            {
                Questions = [.. Questions.OrderBy(x => Guid.NewGuid())];
            }
        }
        public async Task Reset()
        {
            _loaded = false;
            await LoadQuestions();
        }
    }
}
