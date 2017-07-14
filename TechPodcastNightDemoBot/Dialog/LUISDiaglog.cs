using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TechPodcastNightDemoBot.Controllers
{

    [Serializable]
    internal class LUISDiaglog : LuisDialog<object>
    {
        private const string AskQuestionIntentName = "AskQuestion";

        public LUISDiaglog(ILuisService luis)
            : base(luis)
        {
         
        }


        [LuisIntent(AskQuestionIntentName)]
        public async Task AskQuestionIntentAsync(IDialogContext context ,IAwaitable<IMessageActivity> activity ,LuisResult result)
        {
            var entities = new List<EntityRecommendation>(result.Entities);

            var item = entities.Where((entity) => entity.Type == "功能").FirstOrDefault();

            if (item!=null)
            {
                await this.HandleQuery(context, item.Entity);
            }
            else
            {
                await context.PostAsync("請問是甚麼問題???");
            }
           
        }

        private async Task HandleQuery(IDialogContext context, string query)
        {
            string responseString = string.Empty;

            var knowledgebaseId = "91985dec-88a6-4b9c-bd12-dcee4d04b65f"; // Use knowledge base id created.
            var qnamakerSubscriptionKey = "9f68bd838bb0431a9a64c802f9e0274e"; //Use subscription key assigned to you.

            //Build the URI
            Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");
            var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");

            //Add the question as part of the body
            var postBody = $"{{\"question\": \"{query}\"}}";

            //Send the POST request
            using (WebClient client = new WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;

                //Add the subscription key header
                client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                client.Headers.Add("Content-Type", "application/json");
                responseString = client.UploadString(builder.Uri, postBody);
            }

            //De-serialize the response
            QnAMakerResult response;
            try
            {
                response = JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
            }
            catch
            {
                throw new Exception("Unable to deserialize QnA Maker response string.");
            }

            await context.PostAsync(response.Answer);
        }

        private async Task AfterQnAMaker(IDialogContext context, IAwaitable<bool> result)
        {
            var successed = await result;

            if (successed)
            {
                await context.PostAsync("感謝您的詢問");
            }
        }

        [LuisIntent("None")]
        [LuisIntent("")]
        public async Task NotIntentAsync(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {

        }
    }
}