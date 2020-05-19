using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.AI.QnA;

namespace GammaBot.Dialogs
{
    public class MenuDialog : ComponentDialog
    {
        
        private readonly UserState _userState;

        private string[] menuOptions = new string[]
        {
                "Telecom Glossary", "System Support", "Support Availability", "Ticketing", "Send Feedback", "Quit", "Data"
        };

        public MenuDialog(UserState userState, QnAMakerEndpoint endpoint) : base(nameof(MenuDialog))
        {
            _userState = userState;

            var SupportProcessesEndpoint = new QnAMakerEndpoint
            {
                KnowledgeBaseId = "0b9d610f-fe1d-4fb3-a8f7-73bba69098fd",
                EndpointKey = "139c19bb-77e0-4af6-81ca-180d67f25930",
                Host = "https://gammaqnamaker.azurewebsites.net/qnamaker"
            };

            AddDialog(new SupportGlossaryDialog(endpoint));
            AddDialog(new SupportProcessesDialog(SupportProcessesEndpoint));
            AddDialog(new SupportRotaDialog());
            AddDialog(new TicketingDialog());
            AddDialog(new FeedbackDialog(userState));

            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));

            // This array defines how the Waterfall will execute.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                QuestionStepAsync,
                ChoiceStepAsync,
                ConfirmationStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);

        }

        private async Task<DialogTurnResult> QuestionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please select an option:"),
            //var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please select an option:" +
                //"\n\n1 - Telecom Glossary\n\n2 - System Support\n\n3 - Support Availability\n\n4 - Ticketing\n\n5 - Send Feedback\n\n6 - Quit\n\n7 - Data"),
                RetryPrompt = MessageFactory.Text("Please reselect a menu choice"),
                Choices = ChoiceFactory.ToChoices(menuOptions),
            };

            // Ask the user to select menu option.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);

        }

        private async Task<DialogTurnResult> ChoiceStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["choice"] = ((FoundChoice)stepContext.Result).Value;
            var choice = stepContext.Values["choice"].ToString().ToLower();
    
            if (choice == "1" || choice == "telecom glossary")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("You chose Telecom Glossary"));
                return await stepContext.BeginDialogAsync(nameof(SupportGlossaryDialog), null, cancellationToken);
            }
            else if (choice == "2" || choice == "system support")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("You chose System Support"));
                return await stepContext.BeginDialogAsync(nameof(SupportProcessesDialog), null, cancellationToken);
            }
            else if (choice == "3" || choice == "support availability")
            {
                return await stepContext.BeginDialogAsync(nameof(SupportRotaDialog), null, cancellationToken);

            }
            else if (choice == "4" || choice == "ticketing")
            {
                return await stepContext.BeginDialogAsync(nameof(TicketingDialog), null, cancellationToken);

            }
            else if (choice == "5" || choice == "send feedback")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("You chose Send Feedback"));
                return await stepContext.BeginDialogAsync(nameof(FeedbackDialog), null, cancellationToken);

            }
            else if (choice == "6" || choice == "quit")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Thanks for using GammaBot! Type anything to reopen the menu!"));

                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            else if (choice == "7" || choice == "data")
            {
                var accessor = _userState.CreateProperty<FeedbackState>(nameof(FeedbackState));

                var feedbackResults = await accessor.GetAsync(stepContext.Context, () => new FeedbackState(), cancellationToken);

                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Feedback Dialog:\n\n" +
                    "Rating: " + feedbackResults.Rating.ToString() + "\n" +
                    "Note: " + feedbackResults.Notes));

                return await stepContext.ReplaceDialogAsync(nameof(MenuDialog), null, cancellationToken);

            }
            else {
                return await stepContext.ReplaceDialogAsync(nameof(MenuDialog), null, cancellationToken);
            }
        }

        private static async Task<DialogTurnResult> ConfirmationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var messageText = $"Would you like to return to the menu?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);

        }
        private static async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["finalChoice"] = stepContext.Result;

            var choice = stepContext.Values["finalChoice"].ToString().ToLower();

            if (choice == "true")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Sure thing! Taking you back to the menu..."));

                return await stepContext.ReplaceDialogAsync(nameof(MenuDialog), null, cancellationToken);

            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Thanks for using GammaBot! Type anything to reopen the menu!"));

                return await stepContext.EndDialogAsync(null, cancellationToken);
            }


            

        }
    }
}
