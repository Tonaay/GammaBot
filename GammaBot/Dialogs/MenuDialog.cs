using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace GammaBot.Dialogs
{
    public class MenuDialog : ComponentDialog
    {
        private UserState _userState;

        public MenuDialog(UserState userState) : base(nameof(MenuDialog))
        {
            _userState = userState;

            //AddDialog(new SupportGlossaryDialog());

            AddDialog(new TicketingDialog());

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

        private static async Task<DialogTurnResult> QuestionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string[] _menuOptions = new string[]
            {
                "Telecom Glossary", "System Support", "Support Availability", "Ticketing", "Send Feedback", "Quit",
            };

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please select an option:" +
                "\n\n1 - Telecom Glossary\n\n2 - System Support\n\n3 - Support Availability\n\n4 - Ticketing\n\n5 - Send Feedback\n\n6 - Quit"),
                RetryPrompt = MessageFactory.Text("Reselect a menu choice"),
                Choices = ChoiceFactory.ToChoices(_menuOptions),
            };

            // Ask the user to select menu option.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);

        }

        private static async Task<DialogTurnResult> ChoiceStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["choice"] = ((FoundChoice)stepContext.Result).Value;
            var choice = stepContext.Values["choice"].ToString().ToLower();
    
            if (choice == "1" || choice == "telecom glossary")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("You chose Telecom Glossary"));
                return await stepContext.ReplaceDialogAsync(nameof(MenuDialog), null, cancellationToken);
            }
            else if (choice == "2" || choice == "system support")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("You chose System Support"));
                return await stepContext.ReplaceDialogAsync(nameof(MenuDialog), null, cancellationToken);
            }
            else if (choice == "3" || choice == "support availability")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("You chose Support Availability"));
                return await stepContext.ReplaceDialogAsync(nameof(MenuDialog), null, cancellationToken);
            }
            else if (choice == "4" || choice == "ticketing")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("You chose Ticketing"));
                return await stepContext.BeginDialogAsync(nameof(TicketingDialog), null, cancellationToken);

            }
            else if (choice == "5" || choice == "send feedback")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("You chose Send Feedback"));
                return await stepContext.ReplaceDialogAsync(nameof(MenuDialog), null, cancellationToken);
            }
            else if (choice == "6" || choice == "quit")
            {
                return await stepContext.NextAsync(stepContext,cancellationToken);
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
