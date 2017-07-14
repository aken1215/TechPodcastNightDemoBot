using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace TechPodcastNightDemoBot.Controllers
{
    public class MessageController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity ,CancellationToken token)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new RootDialog());
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
       
    }
}
