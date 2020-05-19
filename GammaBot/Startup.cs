<<<<<<< HEAD
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.6.2

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Bot.Builder.AI.QnA;
using GammaBot.Bots;
using GammaBot.Dialogs;

namespace GammaBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, SupportBot<MenuDialog>>();

            //-------------------------------------------------------------------

            //// Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            //services.AddSingleton<IStorage, MemoryStorage>();

            //// Create the User state. (Used in this bot's Dialog implementation.)
            //services.AddSingleton<UserState>();

            //// Create the Conversation state. (Used by the Dialog system itself.)
            //services.AddSingleton<ConversationState>();

            //-------------------------------------------------------------------

            //Previous configuration for states

            var storage = new MemoryStorage();

            // Create the User state passing in the storage layer
            var userState = new UserState(storage);
            services.AddSingleton(userState);

            // Create the Conversation state passing in the storage layer
            var conversationState = new ConversationState(storage);
            services.AddSingleton(conversationState);

            // The Dialog that will be run by the bot.
            services.AddSingleton<MenuDialog>();

            services.AddSingleton(new QnAMakerEndpoint
            {
                KnowledgeBaseId = Configuration.GetValue<string>($"QnAKnowledgebaseId"),
                EndpointKey = Configuration.GetValue<string>($"QnAAuthKey"),
                Host = Configuration.GetValue<string>($"QnAEndpointHostName")
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseWebSockets();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
=======
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.6.2

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Bot.Builder.AI.QnA;
using GammaBot.Bots;
using GammaBot.Dialogs;

namespace GammaBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, SupportBot<MenuDialog>>();

            //-------------------------------------------------------------------

            //// Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            //services.AddSingleton<IStorage, MemoryStorage>();

            //// Create the User state. (Used in this bot's Dialog implementation.)
            //services.AddSingleton<UserState>();

            //// Create the Conversation state. (Used by the Dialog system itself.)
            //services.AddSingleton<ConversationState>();

            //-------------------------------------------------------------------

            //Previous configuration for states

            var storage = new MemoryStorage();

            // Create the User state passing in the storage layer
            var userState = new UserState(storage);
            services.AddSingleton(userState);

            // Create the Conversation state passing in the storage layer
            var conversationState = new ConversationState(storage);
            services.AddSingleton(conversationState);

            // The Dialog that will be run by the bot.
            services.AddSingleton<MenuDialog>();

            services.AddSingleton(new QnAMakerEndpoint
            {
                KnowledgeBaseId = Configuration.GetValue<string>($"QnAKnowledgebaseId"),
                EndpointKey = Configuration.GetValue<string>($"QnAAuthKey"),
                Host = Configuration.GetValue<string>($"QnAEndpointHostName")
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseWebSockets();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
>>>>>>> ee63a129579d63c7f3236c1e710be339704a6105
