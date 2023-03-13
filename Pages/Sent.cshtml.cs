using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetGraphWithAI.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using NetGraphWithAI.AIHelper;

namespace NetGraphWithAI.Pages
{
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public class SentModel : PageModel
    {
        private readonly GraphEmailClient _graphEmailClient;

        [BindProperty(SupportsGet = true)]
        public IEnumerable<Message> Messages  { get; private set; }
        

        public SentModel(GraphEmailClient graphEmailClient)
        {
            _graphEmailClient = graphEmailClient;
         
        }

        public async Task OnGetAsync()
        {
            var messagesPagingData = await _graphEmailClient.GetUserSendMessages(); 
            Messages = messagesPagingData;
          
        }
    }
}
