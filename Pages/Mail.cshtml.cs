
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using NetGraphWithAI.AIHelper;

namespace NetGraphWithAI.Pages
{
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public class MailModel : PageModel
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly IAIHelperService _helperService;
        
        public  Message Message  { get; private set; }
        public IEnumerable<Message> Messages  { get; private set; }
        public IMessageAttachmentsCollectionPage Attachments {get;set;}

        public MailModel (GraphServiceClient graphServiceClient, IAIHelperService helperService)
        {
            _graphServiceClient = graphServiceClient;
            _helperService = helperService;
        }

        public async Task OnGetAsync(string id)
        {
            try {
                var messagesPagingData = await _graphServiceClient.Me.MailFolders.Inbox.Messages
                            .Request()
                            .OrderBy("receivedDateTime desc")
                            .Top(10)
                            .GetAsync();; 
                Messages = messagesPagingData;
                var mail = Messages.Where(p=>p.Id == id).FirstOrDefault();
                Message = mail;

                if (mail.HasAttachments == true) {
                    var attachments = await  _graphServiceClient.Me.Messages[id].Attachments.Request().GetAsync();
                    Attachments = attachments;
                }
                
            }
            catch  (Exception ex) {
                System.Console.WriteLine(ex.Message);
            }
          
        }
       
    }
}
