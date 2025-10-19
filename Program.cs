using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

class Program
{
    static async Task Main(string[] args)
    {
        // Initialize Semantic Kernel with Ollama Chat Completion
        var kernel = Kernel.CreateBuilder()
                           .AddOllamaChatCompletion("Gemma3:4b", new Uri("http://localhost:11434"))
                           .Build();

        var chatService = kernel.GetRequiredService<IChatCompletionService>();

        // Create chat history and set system persona
        var history = new ChatHistory();
        history.AddSystemMessage("You are a helpful assistant.");

        Console.WriteLine("💬 Chat with your AI assistant! Type nothing to exit.\n");

        while (true)
        {
            Console.Write("👤: ");
            var userMessage = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(userMessage))
                break;

            history.AddUserMessage(userMessage);

            // Get AI response
            var response = await chatService.GetChatMessageContentAsync(history);

            // Display response with formatting
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"🤖: {response.Content}");
            Console.ResetColor();

            history.AddMessage(response.Role, response.Content ?? string.Empty);
        }

        Console.WriteLine("\n👋 Conversation ended. Goodbye!");
    }
}
