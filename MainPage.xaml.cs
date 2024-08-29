using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIChatGPT
{
    public partial class MainPage : ContentPage
    {
        private static readonly HttpClient client = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var question = UserQuestionEntry.Text;

            if (string.IsNullOrWhiteSpace(question))
            {
                ResponseLabel.Text = "Please enter a question.";
                return;
            }

            var response = await GetChatGPTResponseAsync(question);
            ResponseLabel.Text = response;
        }

        private async Task<string> GetChatGPTResponseAsync(string question)
        {
            // Define your API key and endpoint
            string apiKey = "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";
            string apiUrl = "https://api.openai.com/v1/chat/completions";

            // Set up the request
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "gpt-4",  // You can use "gpt-3.5-turbo" or other available models
                messages = new[]
                {
                new { role = "user", content = question }
            }
                };
            try
            {
                // Send the request and get the response
                var response = await client.PostAsJsonAsync(apiUrl, requestBody);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonDocument.Parse(jsonResponse);
                var content = result.RootElement
                                    .GetProperty("choices")[0]
                                    .GetProperty("message")
                                    .GetProperty("content")
                                    .GetString();

                return content.Trim();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }







        }
    }

}
