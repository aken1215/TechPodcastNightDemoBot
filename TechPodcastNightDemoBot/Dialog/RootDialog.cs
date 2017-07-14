using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace TechPodcastNightDemoBot.Controllers
{
    [Serializable]
    internal class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceieved);
        }

        private async Task MessageReceieved(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("請問是哪個功能");
            context.Call(new QuestionDialog(), this.AfterAskQuestion);
        }

        private async Task AfterAskQuestion(IDialogContext context, IAwaitable<bool> result)
        {
            var resolved = await result;

            if (resolved)
            {
                await context.PostAsync("感謝您的詢問");
            }
        }
    }
}