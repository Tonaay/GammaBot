// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.6.2

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Net.Http.Headers;

namespace TestBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        public string chatOne = "One";

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var reply = turnContext.Activity.Text;

            // ---- Ticket Guide Dialog
            switch (reply)
            {
                case "Ticketing":
                    await SendTicketGuideAsync(turnContext, cancellationToken);
                    break;
                case "How to create a ticket":
                    var step = MessageFactory.Text("First click on the Create Ticket button located at the top of the page.\n\nYou should be greeted with the following screen. This text field is used to enter the summary of the ticket:");
                    step.Attachments = new List<Attachment>() { GetCreate1Attachment() };
                    await turnContext.SendActivityAsync(step, cancellationToken);
                    step = MessageFactory.Text("The second box below it will display a description field in which you can provide full details of the issues you are having:");
                    step.Attachments = new List<Attachment>() { GetCreate2Attachment() };
                    await turnContext.SendActivityAsync(step, cancellationToken);
                    step = MessageFactory.Text("Lastly there are a couple of dropboxes below the summary and description field. " +
                        "If left blank the ticket will be automatically assigned to the provisioning queue.\n\n" +
                        "Otherwise, select the appropriate team and click the submit button when you are finished"); ;
                    step.Attachments = new List<Attachment>() { GetCreate3Attachment() };
                    await turnContext.SendActivityAsync(step, cancellationToken);
                    await SendTicketGuideAsync(turnContext, cancellationToken);
                    break;
                case "How to update a ticket":
                    var step2 = MessageFactory.Text("To update a ticket, select the ticket and click on 'Ticket Actions' -> 'Add New Event'");
                    step2.Attachments = new List<Attachment>() { GetUpdate1Attachment() };
                    await turnContext.SendActivityAsync(step2, cancellationToken);
                    step2 = MessageFactory.Text("This process is similar to creating a ticket. Enter the summary and description of the update comment.");
                    step2.Attachments = new List<Attachment>() { GetUpdate2Attachment() };
                    await turnContext.SendActivityAsync(step2, cancellationToken);
                    step2 = MessageFactory.Text("Lastly click on the Add Event button at the bottom of the form.");
                    step2.Attachments = new List<Attachment>() { GetUpdate3Attachment() };
                    await turnContext.SendActivityAsync(step2, cancellationToken);
                    await SendTicketGuideAsync(turnContext, cancellationToken);
                    break;
                case "How to close a ticket":
                    var step3 = MessageFactory.Text("To close a ticket, select the ticket you wish to close and click on 'Ticket Actions' -> 'Add New Event'");
                    step3.Attachments = new List<Attachment>() { GetClose1Attachment() };
                    await turnContext.SendActivityAsync(step3, cancellationToken);
                    step3 = MessageFactory.Text("Lastly, change the first drop down box and select resolution complete, leave the next dropbox as it is and click close ticket.");
                    step3.Attachments = new List<Attachment>() { GetClose2Attachment() };
                    await turnContext.SendActivityAsync(step3, cancellationToken);
                    await SendTicketGuideAsync(turnContext, cancellationToken);
                    break;
            }


            //// ---- Create Ticket Dialog
            //switch (reply)
            //{
            //    case "Create Ticket":
            //        await turnContext.SendActivityAsync($"Please enter the system you are having issues with? E.g. CRM System.");
            //        break;
            //    case "Cancel":
            //        await turnContext.SendActivityAsync($"Okay I'll take you back to the ticketing menu!");
            //        await SendMenuOptionsAsync(turnContext, cancellationToken);
            //        break;
            //    case "CRM System":
            //        await turnContext.SendActivityAsync($"Please enter the summary (Title) of the issue.");
            //        break;
            //    case "Unable to access system":
            //        await turnContext.SendActivityAsync($"Please enter a description of the issue.");
            //        break;
            //    case "I have tried logging into the application, but an error message keep appearing saying that my account has been disabled. Please can you look into this?":
            //        await turnContext.SendActivityAsync($"Team: Direct - Summary: Unable to access system");
            //        await turnContext.SendActivityAsync($"Description: Direct - Summary: I have tried logging into the application, but an error message keep appearing saying that my account has been disabled. Please can you look into this?");
            //        await turnContext.SendActivityAsync($"Are you sure you want to submit this? Yes / No");
            //        break;
            //    case "Yes":
            //        await turnContext.SendActivityAsync($"I've just posted your ticket for you. Taking you back to the ticket menu.");
            //        await SendMenuOptionsAsync(turnContext, cancellationToken);
            //        break;
            //    case "No":
            //        await turnContext.SendActivityAsync($"Okay I'll take you back to the ticket menu.");
            //        await SendMenuOptionsAsync(turnContext, cancellationToken);
            //        break;
            //}


            //// ---- Feedback Dialog
            //switch (reply)
            //{
            //    case "Send Feedback":                  
            //        await turnContext.SendActivityAsync($"Please enter a rating from 1-10 of how helpful I was in assisting you");
            //        break;
            //    case "7":
            //        await turnContext.SendActivityAsync($"Please enter additional notes you would like to add to your feedback? Say 'No' to skip!");
            //        break;
            //    case "No":
            //        await turnContext.SendActivityAsync($"Okay, I'll just save the rating. Thanks for giving us feedback, I'll take you back to the menu now.");
            //        await SendMenuOptionsAsync(turnContext, cancellationToken);
            //        break;
            //    case "Testing testing":
            //        await turnContext.SendActivityAsync($"Okay thanks for giving us feedback! I'll take you back to the menu now.");
            //        await SendMenuOptionsAsync(turnContext, cancellationToken);
            //        break;
            //}


            //// ---- Support Rota
            //switch (reply)
            //{
            //    case "Support Availability":
            //        await turnContext.SendActivityAsync($"Please type a system or choose a team.");
            //        await SendTeamOptionsAsync(turnContext, cancellationToken);
            //        break;
            //    case "Direct":
            //        await turnContext.SendActivityAsync($"Currently, John Smith is available today to support any queries for the CRM System." );
            //        break;
            //}

            //// ---- Telecom Glossary
            //switch (reply)
            //{
            //    case "Telecom Glossary":
            //        await turnContext.SendActivityAsync($"Please input a glossary term. E.g. CLI (Caller Identifier Record).");
            //        break;
            //    case "CLI":
            //        await turnContext.SendActivityAsync($"​When a call (signal) is received on a switch," +
            //            $" an electronic record (CDR) is created on that switch in respect of the leg of the call in which that switch is involved." +
            //            $" Such electronic record will contain call metadata (including, typically, starttime, duration, calling party number, " +
            //            $"called party number) designed to enable billing of the call, as well as, typically, various parameters designed to enable" +
            //            $" QoS analysis and measurement. A given call may pass through many devices in the process of traversing a given network" +
            //            $" (such as Gamma's) and therefore result in many different CDRs. The contents of the CDR will vary according to the" +
            //            $" transmission protocol used, the switch vendor, the configuration of the switch, and so on.");
            //        break;
            //}

        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        //private static async Task SendMenuOptionsAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        //{

        //    var menuPrompt = MessageFactory.Text("How would you like to manage your tickets? Please select an option" +
        //        ":\n\n1 - Create a ticket\n\n2 - Update a ticket\n\n3 - Close a ticket");

        //    menuPrompt.SuggestedActions = new SuggestedActions()
        //    {
        //        Actions = new List<CardAction>()
        //        {
        //            new CardAction() { Title = "Create a ticket", Type = ActionTypes.ImBack, Value = "Create a ticket" },
        //            new CardAction() { Title = "Update a ticket", Type = ActionTypes.ImBack, Value = "Update a ticket" },
        //            new CardAction() { Title = "Close a ticket", Type = ActionTypes.ImBack, Value = "Close a ticket" },
        //        },
        //    };

        //    await turnContext.SendActivitiesAsync(
        //            new Activity[] {
        //        new Activity { Type = ActivityTypes.Typing },
        //        new Activity { Type = "delay", Value= 2000 },
        //        menuPrompt,
        //            },
        //            cancellationToken);
        //}

        //private static async Task SendTeamOptionsAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        //{

        //    var menuPrompt = MessageFactory.Text("List of example systems" +
        //        ":\n\nCRM System\n\nProvisioning Portal\n\nDirect Portal");

        //    menuPrompt.SuggestedActions = new SuggestedActions()
        //    {
        //        Actions = new List<CardAction>()
        //        {
        //            new CardAction() { Title = "Direct", Type = ActionTypes.ImBack, Value = "Direct" },
        //            new CardAction() { Title = "Provisioning", Type = ActionTypes.ImBack, Value = "Provisioning" },
        //            new CardAction() { Title = "Database Admins", Type = ActionTypes.ImBack, Value = "Database Admins" },
        //            new CardAction() { Title = "Billing", Type = ActionTypes.ImBack, Value = "Billing" },

        //        },
        //    };

        //    await turnContext.SendActivitiesAsync(
        //            new Activity[] {
        //        new Activity { Type = ActivityTypes.Typing },
        //        new Activity { Type = "delay", Value= 2000 },
        //        menuPrompt,
        //            },
        //            cancellationToken);
        //}

        private static async Task SendTicketGuideAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {

            var menuPrompt = MessageFactory.Text("Please choose a guide for ticket management:" +
                "\n\n1 - How to create a ticket\n\n2 - How to update a ticket\n\n3 - How to close a ticket");

            menuPrompt.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction() { Title = "How to create a ticket", Type = ActionTypes.ImBack, Value = "How to create a ticket" },
                    new CardAction() { Title = "How to update a ticket", Type = ActionTypes.ImBack, Value = "How to update a ticket" },
                    new CardAction() { Title = "How to close a ticket", Type = ActionTypes.ImBack, Value = "How to close a ticket" },
                },
            };

            await turnContext.SendActivitiesAsync(
                    new Activity[] {
                new Activity { Type = ActivityTypes.Typing },
                new Activity { Type = "delay", Value= 2000 },
                menuPrompt,
                    },
                    cancellationToken);
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

