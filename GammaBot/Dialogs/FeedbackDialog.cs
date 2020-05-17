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

        public FeedbackDialog() : base(nameof(FeedbackDialog))
        {

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                RatingStepAsync,
                NoteStepAsync,
                FinalStepAsync,
            };

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);

        }

        private static async Task<DialogTurnResult> RatingStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.NextAsync(stepContext, cancellationToken);

        }

        private static async Task<DialogTurnResult> NoteStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            return await stepContext.NextAsync(stepContext, cancellationToken);
        }


        private static async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
}
