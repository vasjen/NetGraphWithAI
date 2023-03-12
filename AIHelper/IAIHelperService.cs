using Microsoft.Graph;
using OpenAI_API.Completions;

namespace NetGraphWithAI.AIHelper
{
    public interface IAIHelperService
    {
        Task<string> Transcription(Attachment file);

        Task<CompletionResult> Translate(string promt);
    }
}