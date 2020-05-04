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
    public class MenuDialog : ComponentDialog
    {
        private const string MenuChoiceStepMsg = "Please select an option:\n\n1 - Telecom Glossary\n\n2 - System Support\n\n3 - Support Availability\n\n4 - Ticketing\n\n5 - Send Feedback";
        //private string MenuChoice = "";
        private readonly UserState _userState;
        private readonly ConversationState _conversationState;

        public MenuDialog(UserState userState, ConversationState conversationState) : base(nameof(MenuDialog))
        {
            _userState = userState;
            _conversationState = conversationState;
            AddDialog(new UserInfoDialog());
            AddDialog(new SupportGlossaryDialog());
            AddDialog(new SupportProcessesDialog());
            AddDialog(new SupportRotaDialog());
            AddDialog(new TicketingDialog());
            AddDialog(new FeedbackDialog());

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                UserInfoStepAsync,
                UserValidationAsync,
                MenuChoiceStepAsync,
                ChoiceValidationAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }


        //Run dialog to get User's name and Team
        private async Task<DialogTurnResult> UserInfoStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var _userProfileAccessor = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);
            if (string.IsNullOrEmpty(userProfile.Name))
            {
                return await stepContext.BeginDialogAsync(nameof(UserInfoDialog), null, cancellationToken);
            }
            return await stepContext.NextAsync(stepContext, cancellationToken);
        }

        //Save user details to the userState
        private async Task<DialogTurnResult> UserValidationAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var _userProfileAccessor = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);

            //var _conversationStateAccessor = _conversationState.CreateProperty<ConversationData>("ConversationData");
            //var conversationData = await _conversationStateAccessor.GetAsync(stepContext.Context, () => new ConversationData(), cancellationToken);

            if (!string.IsNullOrEmpty(userProfile.Name))
            {
                var userInfo = (UserProfile)stepContext.Result;
                var accessor = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
                await accessor.SetAsync(stepContext.Context, userInfo, cancellationToken);
            }

            return await stepContext.NextAsync(stepContext, cancellationToken);
        }

        private async Task<DialogTurnResult> MenuChoiceStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.NextAsync(stepContext, cancellationToken);

        }

        private async Task<DialogTurnResult> ChoiceValidationAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.NextAsync(stepContext, cancellationToken);
        }

        private async Task<DialogTurnResult> FeatureStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.NextAsync(stepContext, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }


        



    }
}

