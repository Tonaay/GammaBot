using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.AI.QnA;

namespace GammaBot.Dialogs
{
    public class SupportGlossaryDialog : ComponentDialog
    {
        public QnAMaker TelecomGlossaryQnA { get; private set; }

        public SupportGlossaryDialog(QnAMakerEndpoint endpoint) : base(nameof(SupportGlossaryDialog)){

            TelecomGlossaryQnA = new QnAMaker(endpoint);

            AddDialog(new TextPrompt(nameof(TextPrompt)));

            // This array defines how the Waterfall will execute.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                QuestionStepAsync,
                ResponseStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);

        }

        private static async Task<DialogTurnResult> QuestionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter a telecommunicaions term you would" +
                " like to know about. E.g. CLI (Type 'quit' to finish)")
            };
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);

        }

        private async Task<DialogTurnResult> ResponseStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string question = (string)stepContext.Result;

            if (question == "quit")
            {
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }
            else
            {
                var results = await TelecomGlossaryQnA.GetAnswersAsync(stepContext.Context);
                if (results.Any())
                {
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(results.First().Answer), cancellationToken);
                }
                else
                {
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Sorry, could not find an answer in the Q and A system."), cancellationToken);
                }
                return await stepContext.ReplaceDialogAsync(nameof(SupportGlossaryDialog), null, cancellationToken);
            }
        }
    }
}
