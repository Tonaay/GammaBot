using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GammaBot.Dialogs
{
    public class SupportProcessesDialog: ComponentDialog
    {
        public QnAMaker SupportProcessesQnA { get; private set; }

        public SupportProcessesDialog(QnAMakerEndpoint endpoint) : base(nameof(SupportProcessesDialog))
        { 
            SupportProcessesQnA = new QnAMaker(endpoint);

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

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("How can I help you? - E.g. I have and issue with the CRM System (Type 'quit' to finish)")
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
                var results = await SupportProcessesQnA.GetAnswersAsync(stepContext.Context);
                if (results.Any())
                {
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(results.First().Answer), cancellationToken);
                }
                else
                {
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Sorry, could not find an answer in the Q and A system."), cancellationToken);
                }
                return await stepContext.ReplaceDialogAsync(nameof(SupportProcessesDialog), null, cancellationToken);
            }
        }
    }
}
