using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using Microsoft.Graph;

namespace NetGraphWithAI.AIHelper {
    public class AIHelperService : IAIHelperService {
        private readonly HttpClient _client;
        private readonly OpenAIAPI _api;

        public AIHelperService(HttpClient client, OpenAIAPI api)
        {
            _client = client;
            _api = api;
        }
        

            
        public async Task<string> Transcription(Attachment file){
            
            ByteArrayContent f = new ByteArrayContent(((FileAttachment)file).ContentBytes);

            using (var multipartFormContent = new MultipartFormDataContent())
            {
            	
                multipartFormContent.Add(new StringContent("whisper-1"), name: "model");
            	multipartFormContent.Add(f, name: "file", fileName: "file.m4a");

            	var responseTest = await _client.PostAsync("https://api.openai.com/v1/audio/transcriptions", multipartFormContent);
            	var wow = await responseTest.Content.ReadAsStringAsync();
                var text = JsonNode.Parse(wow)["text"].GetValue<string>();
                return text;
            }
        }

        public async Task<CompletionResult> Translate(string promt){
           
          var result = await _api.Completions.CreateCompletionAsync(new CompletionRequest("Translate into English and correct this to standard English:"+promt, model: Model.DavinciText, max_tokens: 3000));
          System.Console.WriteLine(result);
          return result;

        }
       
    }
}