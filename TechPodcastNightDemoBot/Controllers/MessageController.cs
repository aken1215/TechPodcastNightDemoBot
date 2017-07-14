using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
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
        private ILifetimeScope scope;

        public MessageController(ILifetimeScope scope)
        {
            SetField.NotNull(out this.scope, nameof(scope), scope);
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity, CancellationToken token)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //await Conversation.SendAsync(activity, () => new RootDialog());
                using (var beginLifetimeScope = DialogModule.BeginLifetimeScope(this.scope, activity))
                {
                    var postToBot = beginLifetimeScope.Resolve<IPostToBot>();
                    await postToBot.PostAsync(activity, token);
                }

            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

    }
}
