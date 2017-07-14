using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net;
using Newtonsoft.Json;

namespace TechPodcastNightDemoBot.Controllers
{
    [Serializable]
    internal class QnAMakerDialog : IDialog<bool>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceieved);
        }

        private async Task MessageReceieved(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            string responseString = string.Empty;

            var message = await result;

            var query = message.Text;
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
            context.Done(true);
        }
    }
}