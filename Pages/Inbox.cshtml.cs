
using NetGraphWithAI.Graph;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using NetGraphWithAI.AIHelper;

namespace NetGraphWithAI.Pages
{
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public class InboxModel : PageModel
    {
        private readonly GraphEmailClient _graphEmailClient;
        private readonly IAIHelperService _helperService;

        [BindProperty(SupportsGet = true)]
        public string NextLink { get; set; }
        public IEnumerable<Message> Messages  { get; private set; }
        public string Translate {get;set;}

        public InboxModel(GraphEmailClient graphEmailClient, IAIHelperService helperService)
        {
            _graphEmailClient = graphEmailClient;
            _helperService = helperService;
        }

        public async Task OnGetAsync()
        {
            var messagesPagingData = await _graphEmailClient.GetUserMessages(); 
            Messages = messagesPagingData;
          
        }
    }
}
