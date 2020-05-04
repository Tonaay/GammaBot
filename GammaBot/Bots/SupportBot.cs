// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.6.2

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GammaBot.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace GammaBot.Bots
{
    public class SupportBot<T> : ActivityHandler where T : Dialog
    {
        private BotState _conversationState;
        private BotState _userState;
        private Dialog _dialog;

        public SupportBot(ConversationState conversationState, UserState userState, Dialog dialog)
        {
            _conversationState = conversationState;
            _userState = userState;
            _dialog = dialog;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occurred during the turn.
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);
            // Setting up Conversation State Data
            var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
            var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());

            //Setting up User Profile
            var userStateAccessors = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            var userProfile = await userStateAccessors.GetAsync(turnContext, () => new UserProfile());

            if (string.IsNullOrEmpty(userProfile.Name))
            {
                // First time around this is set to false, so we will prompt user for name.
                if (conversationData.PromptedUserForName)
                {
                    // Set the name to what the user provided.
                    userProfile.Name = turnContext.Activity.Text?.Trim();

                    // Acknowledge that we got their name.
                    await turnContext.SendActivityAsync($"Nice to meet you {userProfile.Name}!");
                    // Reset the flag to allow the bot to go through the cycle again.
                    conversationData.PromptedUserForName = false;
                }
                else
                {
                    // Prompt the user for their name.
                    await turnContext.SendActivityAsync($"What is your name?");

                    // Set the flag to true, so we don't prompt in the next turn.
                    conversationData.PromptedUserForName = true;
                }
            }
            else
            {
                // Set state to menu so that all responses validate menu dialog only
                // User response within the menu dialog
                string userResponse = turnContext.Activity.Text.ToLower();

                // Validate menu selection input
                switch (userResponse)
                {
                    case "telecom glossary":
                        await turnContext.SendActivityAsync($"You chose Telecom Glossary.");
                        break;
                    case "system support":
                        await turnContext.SendActivityAsync($"You chose System Support.");
                        break;
                    case "support availability":
                        await turnContext.SendActivityAsync($"You chose Support Availability.");
                        break;
                    case "ticketing":
                        await turnContext.SendActivityAsync($"You chose Ticketing.");
                        break;
                    case "send feedback":
                        await turnContext.SendActivityAsync($"You chose Feedback.");
                        break;
                    default:
                        await turnContext.SendActivityAsync($"Sorry, I didn't understand your option!");
                        await SendMenuOptionsAsync(turnContext, cancellationToken);
                        break;
                }
            }
        }

        //When a user joins a channel, greet the user
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {

            await SendWelcomeMessageAsync(turnContext, cancellationToken);

        }

        // Send a welcome message to the user with menu suggested actions
        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync($"Hi there, I'm the Gamma Chatbot.", cancellationToken: cancellationToken);
                    await SendMenuOptionsAsync(turnContext, cancellationToken);
                }
            }
        }

        private static async Task SendMenuOptionsAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {

            var menuPrompt = MessageFactory.Text("Please select an option:\n\n1 - Telecom Glossary\n\n2 - System Support\n\n3 - Support Availability\n\n4 - Ticketing\n\n5 - Send Feedback");

            menuPrompt.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction() { Title = "Telecom Glossary", Type = ActionTypes.ImBack, Value = "Telecom Glossary" },
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
                    },
                    cancellationToken);
        }
    }
}
