
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System.Linq;
using System.Net;
using System.Net.Http;
using NetGraphWithAI.AIHelper;

namespace NetGraphWithAI.Graph
{
    public class GraphEmailClient
    {
        private readonly ILogger<GraphEmailClient> _logger;
        private readonly GraphServiceClient _graphServiceClient;

        public GraphEmailClient(ILogger<GraphEmailClient> logger, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
        }

        public async Task<IEnumerable<Message>> GetUserMessages()
        {
            try
            {
                var emails = await _graphServiceClient.Me.MailFolders.Inbox.Messages
                            .Request()
                            .Select(msg => new
                            {
                                msg.Subject,
                                msg.BodyPreview,
                                msg.ReceivedDateTime,
                                msg.HasAttachments,
                                msg.Attachments
                            })
                            .OrderBy("receivedDateTime desc")
                            .Top(10)
                            .GetAsync();

                foreach (var item in emails)
                {
                    if (item.HasAttachments == true)
                        item.Attachments = await _graphServiceClient.Me.Messages[item.Id].Attachments.Request().GetAsync();
                        
                }

                return emails.CurrentPage;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling Graph /me/messages: {ex.Message}");
                throw;
            }
        }

      public async Task<IEnumerable<Message>> GetUserSendMessages()
        {
            try
            {
                var emails = await _graphServiceClient.Me.MailFolders.SentItems.Messages
                            .Request()
                            .Select(msg => new
                            {
                                msg.Subject,
                                msg.BodyPreview,
                                msg.ReceivedDateTime,
                                msg.HasAttachments,
                                msg.Attachments
                            })
                            .OrderBy("receivedDateTime desc")
                            .Top(10)
                            .GetAsync();

                foreach (var item in emails)
                {
                    if (item.HasAttachments == true)
                        item.Attachments = await _graphServiceClient.Me.Messages[item.Id].Attachments.Request().GetAsync();
                        
                }

                return emails.CurrentPage;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling Graph /me/messages: {ex.Message}");
                throw;
            }
        }



        

    }
}