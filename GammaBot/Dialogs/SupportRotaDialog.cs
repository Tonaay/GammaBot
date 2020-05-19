using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GammaBot.Dialogs
{
    public class SupportRotaDialog: ComponentDialog
    {

        private string[] teamOptions = new string[]
        {
                "Direct", "Provisioning", "Database Admins", "Billing"
        };

        private string ticketPrompt = "Please ensure you have raised a ticket before directly speaking with a support user.";

        public SupportRotaDialog() : base(nameof(SupportRotaDialog))
        {
            AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>)));
            AddDialog(new TextPrompt(nameof(TextPrompt)));


            // This array defines how the Waterfall will execute.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                TeamStepAsync,
                AvailabilityStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> TeamStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Please select an option:"),
                RetryPrompt = MessageFactory.Text("Please reselect a team"),
                Choices = ChoiceFactory.ToChoices(teamOptions),
            };

            // Ask the user to select menu option.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);

        }

        private async Task<DialogTurnResult> AvailabilityStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["choice"] = ((FoundChoice)stepContext.Result).Value;
            var choice = stepContext.Values["choice"].ToString().ToLower();

            switch (choice)
            {
                case "direct":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Here's the support rota for the Direct Team...\n"));
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Tickets and Emails: Tony, Zack, John\n" +
                        "Gamma Hub: John, Ross, James\n" +
                        "Customer Portal: William, Alice, Sam\n" +
                        "Finance: Alice, Sam\n"));
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(ticketPrompt));
                    break;

                case "provisioning":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Here's the support rota for the Provisioning Team...\n"));
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Tickets and Emails: David, Nia, James, Steve, Bob\n" +
                        "Gamma Hub: John, Ross, James\n" +
                        "Alarms & Escalations: Mark, Andrew\n" +
                        "Mobile: Chloe\n" +
                        "Out of hours support: Chloe"));
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(ticketPrompt));

                    break;
                case "database admins":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Here's the support rota for the DBA Team...\n"));
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("DBA Daily Checks & Support: Thomas, Scott, Juno, Jack\n" +
                        "2nd Line DBA Support: Yuno, Shawn, Gillian\n" +
                        "Out of hours support: Jack\n" +
                        "Out of hours standby: Yuno\n"));
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(ticketPrompt));

                    break;
                case "billing":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Here's the support rota for the Billing Team...\n"));
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Tickets & Emails: Kat, Jordan, Geoff\n" +
                        "Nagios Alarms: Kat, Jordan, Geoff\n" +
                        "Revenue Assurance Checks: Claire, Justin, Lisa"));
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(ticketPrompt));

                    break;
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

    }
}
