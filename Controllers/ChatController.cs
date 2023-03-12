using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using NetGraphWithAI.AIHelper;

namespace NetGraphWithAI.Controllers{
    [Route("api/ai")]
    [ApiController]
    public class ChatController : ControllerBase {
        private readonly IAIHelperService _helper;
        private readonly GraphServiceClient _graphServiceClient;

        public ChatController(IAIHelperService helper, GraphServiceClient graphServiceClient)
        {
            _helper = helper;
            _graphServiceClient = graphServiceClient;
        }

        [Produces("application/json")]
        [HttpGet("translate")]
        [Route("translate")]
        public async Task<IActionResult> Translate(){
            
            var id = HttpContext.Request.Query["id"].ToString();

            try {
            var message =  _graphServiceClient.Me.Messages[id].Request().GetAsync();
            var text = message.Result.BodyPreview;
            var result = await _helper.Translate(text);
            return Ok(result.ToString());
            }
            catch (Exception ex){

                System.Console.WriteLine( ex.Message);
                return BadRequest($"Can't get a message with id {id}");
            }
            
            
        }

        [Produces("application/json")]
        [HttpGet("convert")]
        [Route("convert")]
        public async Task<IActionResult> Convert(){

            var header = HttpContext.Request.Headers;
            var message_id = header["message_id"].ToString();
            var attachment_id = header["attachment_id"].ToString();
            
            try {
                var attachment = await _graphServiceClient.Me.Messages[message_id].Attachments[attachment_id].Request().GetAsync();
               
               var t = await _helper.Transcription(attachment);
               var ans = await _helper.Translate(t);
                return Ok(ans.ToString());
            }
            catch (Exception ex){
                
                System.Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }
    
       
    }
}