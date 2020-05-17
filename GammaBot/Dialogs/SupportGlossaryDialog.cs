﻿using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace GammaBot.Dialogs
{
    public class SupportGlossaryDialog : ComponentDialog
    {

        public SupportGlossaryDialog() : base(nameof(SupportGlossaryDialog)){

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                QuestionStepAsync,
                ResponseStepAsync,
                SummaryStepAsync,
            };

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);

        }

        private static async Task<DialogTurnResult> QuestionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.NextAsync(stepContext, cancellationToken);

        }

        private static async Task<DialogTurnResult> ResponseStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            return await stepContext.NextAsync(stepContext, cancellationToken);
        }


        private static async Task<DialogTurnResult> SummaryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
