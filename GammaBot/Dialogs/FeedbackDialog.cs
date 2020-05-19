using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GammaBot.Dialogs
{
    public class FeedbackDialog : ComponentDialog
    {
        private const string FeedbackInfo = "value-FeedbackInfo";
        private readonly UserState _userState;

        public FeedbackDialog(UserState userState) : base(nameof(FeedbackDialog))
        {
            _userState = userState;

            AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>)));
            AddDialog(new TextPrompt(nameof(TextPrompt)));


            // This array defines how the Waterfall will execute.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                RatingStepAsync,
                NoteStepAsync,
                ConfirmationStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);

        }

        private static async Task<DialogTurnResult> RatingStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[FeedbackInfo] = new FeedbackState();

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter a rating (1: lowest, 10: highest).") };
            return await stepContext.PromptAsync(nameof(NumberPrompt<int>), promptOptions, cancellationToken);
        }

        private static async Task<DialogTurnResult> NoteStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var feedbackDetails = (FeedbackState)stepContext.Values[FeedbackInfo];
            feedbackDetails.Rating = (int)stepContext.Result;

            if (feedbackDetails.Rating >= 1 || feedbackDetails.Rating <= 10)
            {               
                var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("How could we improve your experience with the chatbot?") };
                return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);

            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Sorry the rating must be an integer (1-10)!"));

                return await stepContext.ReplaceDialogAsync(nameof(FeedbackDialog), null, cancellationToken);

            }
        }

        private static async Task<DialogTurnResult> ConfirmationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var feedbackDetails = (FeedbackState)stepContext.Values[FeedbackInfo];
            feedbackDetails.Notes = (string)stepContext.Result;

            return await stepContext.NextAsync(stepContext, cancellationToken);
        }


        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var feedbackInfo = (FeedbackState)stepContext.Values[FeedbackInfo];

            var accessor = _userState.CreateProperty<FeedbackState>(nameof(FeedbackState));
            await accessor.SetAsync(stepContext.Context, feedbackInfo, cancellationToken);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Thanks for the feedback!"));

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}

