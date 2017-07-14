using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace TechPodcastNightDemoBot.Controllers
{
    [Serializable]
    internal class QuestionDialog : IDialog<bool>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceieved);
        }

        private async Task MessageReceieved(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text == "報表列印")
            {
                await context.PostAsync("請確認瀏覽器是IE，並關掉相容性");
                context.Done(true);
            }
        }
    }
}