using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GammaBot.Dialogs
{
    public class TicketingDialog: ComponentDialog
    {

        public TicketingDialog() : base(nameof(TicketingDialog))
        {

            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            // This array defines how the Waterfall will execute.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ChoiceStepAsync,
                ResponseStepAsync,
                SummaryStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);

        }

        private static async Task<DialogTurnResult> ChoiceStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string[] _menuOptions = new string[]
{
                "How to create a ticket", "How to update a ticket", "How to close a ticket", "Finish",
};

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Please select an option:"),
                RetryPrompt = MessageFactory.Text("Reselect a menu choice"),
                Choices = ChoiceFactory.ToChoices(_menuOptions),
            };

            // Ask the user to select menu option.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);

        }

        private static async Task<DialogTurnResult> ResponseStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["choice"] = ((FoundChoice)stepContext.Result).Value;
            var choice = stepContext.Values["choice"].ToString().ToLower();
    
            if (choice == "1" || choice == "how to create a ticket")
            {
                var step = MessageFactory.Text("First click on the Create Ticket button located at the top of the page.\n\nYou should be greeted with the following screen. This text field is used to enter the summary of the ticket:");
                step.Attachments = new List<Attachment>() { GetCreate1Attachment() };
                await stepContext.Context.SendActivityAsync(step, cancellationToken);
                step = MessageFactory.Text("The second box below will display a description field in which you can provide full details of the issues you are having:");
                step.Attachments = new List<Attachment>() { GetCreate2Attachment() };
                await stepContext.Context.SendActivityAsync(step, cancellationToken);
                step = MessageFactory.Text("Lastly there are a couple of dropboxes below the summary and description field. " +
                    "If left blank the ticket will be automatically assigned to the provisioning queue.\n\n" +
                    "Otherwise, select the appropriate team and click the submit button when you are finished"); ;
                step.Attachments = new List<Attachment>() { GetCreate3Attachment() };
                return await stepContext.ReplaceDialogAsync(nameof(TicketingDialog), null, cancellationToken);

            }
            else if (choice == "2" || choice == "how to update a ticket")
            {
                var step2 = MessageFactory.Text("To update a ticket, select the ticket and click on 'Ticket Actions' -> 'Add New Event'");
                step2.Attachments = new List<Attachment>() { GetUpdate1Attachment() };
                await stepContext.Context.SendActivityAsync(step2, cancellationToken);
                step2 = MessageFactory.Text("This process is similar to creating a ticket. Enter the summary and description of the update comment.");
                step2.Attachments = new List<Attachment>() { GetUpdate2Attachment() };
                await stepContext.Context.SendActivityAsync(step2, cancellationToken);
                step2 = MessageFactory.Text("Lastly click on the Add Event button at the bottom of the form.");
                step2.Attachments = new List<Attachment>() { GetUpdate3Attachment() };
                await stepContext.Context.SendActivityAsync(step2, cancellationToken);
                return await stepContext.ReplaceDialogAsync(nameof(TicketingDialog), null, cancellationToken);
            }
            else if (choice == "3" || choice == "how to close a ticket")
            {
                var step3 = MessageFactory.Text("To close a ticket, select the ticket you wish to close and click on 'Ticket Actions' -> 'Add New Event'");
                step3.Attachments = new List<Attachment>() { GetClose1Attachment() };
                await stepContext.Context.SendActivityAsync(step3, cancellationToken);
                step3 = MessageFactory.Text("Lastly, change the first drop down box and select resolution complete, leave the next dropbox as it is and click close ticket.");
                step3.Attachments = new List<Attachment>() { GetClose2Attachment() };
                await stepContext.Context.SendActivityAsync(step3, cancellationToken);
                return await stepContext.ReplaceDialogAsync(nameof(TicketingDialog), null, cancellationToken);
            }
            else if (choice == "4" || choice == "finish")
            {

                return await stepContext.NextAsync(stepContext, cancellationToken);
            }
            else
            {
                return await stepContext.ReplaceDialogAsync(nameof(TicketingDialog), null, cancellationToken);

            }
        }


        private static async Task<DialogTurnResult> SummaryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private static Attachment GetCreate1Attachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Images\1-Create.png");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Images\1-Create.png",
                ContentType = "image/png",
                ContentUrl = $"data:image/png;base64,{imageData}",
            };
        }


        private static Attachment GetCreate2Attachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Images\2-Create.png");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Images\2-Create.png",
                ContentType = "image/png",
                ContentUrl = $"data:image/png;base64,{imageData}",
            };
        }

        private static Attachment GetCreate3Attachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Images\3-Create.png");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Images\3-Create.png",
                ContentType = "image/png",
                ContentUrl = $"data:image/png;base64,{imageData}",
            };
        }

        private static Attachment GetUpdate1Attachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Images\4-Update.png");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Images\4-Update.png",
                ContentType = "image/png",
                ContentUrl = $"data:image/png;base64,{imageData}",
            };
        }

        private static Attachment GetUpdate2Attachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Images\5-Update.png");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Images\5-Update.png",
                ContentType = "image/png",
                ContentUrl = $"data:image/png;base64,{imageData}",
            };
        }
        private static Attachment GetUpdate3Attachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Images\6-Update.png");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Images\6-Update.png",
                ContentType = "image/png",
                ContentUrl = $"data:image/png;base64,{imageData}",
            };
        }

        private static Attachment GetClose1Attachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Images\7-Close.png");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Images\7-Close.png",
                ContentType = "image/png",
                ContentUrl = $"data:image/png;base64,{imageData}",
            };
        }

        private static Attachment GetClose2Attachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Images\8-Close.png");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Images\8-Close.png",
                ContentType = "image/png",
                ContentUrl = $"data:image/png;base64,{imageData}",
            };
        }
    }
}
