using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
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
            if (result.Query == "系統有問題")
            {
                await context.PostAsync("請問是甚麼問題?");
            }
            else
            {
                context.Call(new QnAMakerDialog(), this.AfterQnAMaker);
            }
           
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