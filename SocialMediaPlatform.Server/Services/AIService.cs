using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

public class AIService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public AIService(IConfiguration config)
    {
        _apiKey = config["OpenAI:ApiKey"];
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1"); // Ensure proper OpenAI support
    }

    public async Task<string> GenerateSummaryAsync(string userData)
    {
        var requestData = new
        {
            model = "gpt-3.5-turbo",  // ✅ Use a valid model
            messages = new[]
            {
                new { role = "system", content = "You are an AI that generates 5-10 words user descriptions, in first person." },
                new { role = "user", content = $"Summarize this user's activity: {userData}" }
            },
            max_tokens = 100,
            temperature = 0.7
        };

        var jsonRequest = JsonSerializer.Serialize(requestData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"OpenAI API error: {errorMessage}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseData = JsonSerializer.Deserialize<OpenAiResponse>(jsonResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        return responseData?.Choices?.FirstOrDefault()?.Message?.Content ?? "No description generated.";
    }
    public async Task<string> RecommendSimilarUsersAsync(string currentUserDescription, List<(string UserId, string AiDescription)> allDescriptions)
    {
        // Build the prompt
        var prompt = new StringBuilder();
        prompt.AppendLine("You are an AI that recommends users based on similar descriptions, returning only a string with only the id.");
        prompt.AppendLine($"Current User Description: {currentUserDescription}");
        prompt.AppendLine("Here are the descriptions of other users:");

        foreach (var user in allDescriptions)
        {
            prompt.AppendLine($"User {user.UserId}: {user.AiDescription}");
        }

        prompt.AppendLine("Suggest the most similar user to the current user. Give only the id, nothing else. If none match, just answer 'NA'.");

        // Call OpenAI API
        var requestData = new
        {
            model = "gpt-3.5-turbo", // Use a valid model
            messages = new[]
            {
                new { role = "system", content = "You are an assistant that processes user similarity." },
                new { role = "user", content = prompt.ToString() }
            },
            max_tokens = 500,
            temperature = 0.7
        };

        var jsonRequest = JsonSerializer.Serialize(requestData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"OpenAI API error: {errorMessage}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseData = JsonSerializer.Deserialize<OpenAiResponse>(jsonResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        return responseData?.Choices?.FirstOrDefault()?.Message?.Content ?? "No recommendations generated.";
    }
}

public class OpenAiResponse
{
    public List<OpenAiChoice> Choices { get; set; }
}

public class OpenAiChoice
{
    public OpenAiMessage Message { get; set; }
}

public class OpenAiMessage
{
    public string Role { get; set; }
    public string Content { get; set; }
}
