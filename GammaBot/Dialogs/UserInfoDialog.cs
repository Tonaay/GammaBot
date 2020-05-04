using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Serialization;
namespace GammaBot.Dialogs
{
    public class UserInfoDialog : ComponentDialog
    {
        private const string UserInfo = "value-userInfo";
        public UserInfoDialog()
            : base(nameof(UserInfoDialog))
        { 
            AddDialog(new TextPrompt("name"));
            AddDialog(new TextPrompt("team"));
            AddDialog(new TextPrompt("support"));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                NameStepAsync,
                TeamStepAsync,
                AcknowledgementStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private static async Task<DialogTurnResult> NameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[UserInfo] = new UserProfile();

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your name.") };

            // Ask the user to enter their name.
            return await stepContext.PromptAsync("name", promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> TeamStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.Name = (string)stepContext.Result;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("What team are you in?") };

            // Ask the user to enter their team.
            return await stepContext.PromptAsync("team", promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> AcknowledgementStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { 
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.Team = (string)stepContext.Result;

            //var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);
            //userProfile.Name = (string)stepContext.Values["name"];
            //userProfile.Team = (string)stepContext.Values["team"];

            //Aknowledge information gathered
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Nice to meet you {userProfile.Name}. Let's get started!"),
                cancellationToken);

            // Exit the dialog, returning the collected user information.
            return await stepContext.EndDialogAsync(stepContext.Values[UserInfo], cancellationToken);
        }
    }
}
