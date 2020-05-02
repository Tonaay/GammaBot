// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.6.2

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace GammaBot.Bots
{
    public class SupportBot : ActivityHandler
    {
        private BotState _conversationState;
        private BotState _userState;


        public SupportBot(ConversationState conversationState, UserState userState)
        {
            _conversationState = conversationState;
            _userState = userState;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (string.Equals(turnContext.Activity.Text, "wait", System.StringComparison.InvariantCultureIgnoreCase))
            {
                await turnContext.SendActivitiesAsync(
                    new Activity[] {
                new Activity { Type = ActivityTypes.Typing },
                new Activity { Type = "delay", Value= 3000 },
                MessageFactory.Text("Finished typing", "Finished typing"),
                    },
                    cancellationToken);
            }
            else
            {
                var replyText = $"Echo: {turnContext.Activity.Text}. Say 'wait' to watch me type.";
                await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
            }
        }

        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync($"Hi there {member.Name}, I'm the Gamma Chatbot.", cancellationToken: cancellationToken);
                    await SendSuggestedActionsAsync(turnContext, cancellationToken);
                }
            }
        }


        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {

            await SendWelcomeMessageAsync(turnContext, cancellationToken);

        }

        private static async Task SendSuggestedActionsAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {

            var menuPrompt = MessageFactory.Text("Please select an option:\n\n1 - Telecom Glossary\n\n2 - System Support\n\n3 - Support Availability\n\n4 - Ticketing\n\n5 - Send Feedback");

            menuPrompt.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
        {
            new CardAction() { Title = "Telecom Glossary", Type = ActionTypes.ImBack, Value = "Knowledge Base" },
            new CardAction() { Title = "System Support", Type = ActionTypes.ImBack, Value = "System Support" },
            new CardAction() { Title = "Support Availablity", Type = ActionTypes.ImBack, Value = "Support Availability" },
            new CardAction() { Title = "Ticketing", Type = ActionTypes.ImBack, Value = "Ticketing" },
            new CardAction() { Title = "Send FeedBack", Type = ActionTypes.ImBack, Value = "Send Feedback" },
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
    }
}
