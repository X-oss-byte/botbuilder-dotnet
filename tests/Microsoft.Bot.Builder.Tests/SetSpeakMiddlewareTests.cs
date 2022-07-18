﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Xunit;

namespace Microsoft.Bot.Builder.Tests
{
    public class SetSpeakMiddlewareTests
    {
        [Fact]
        public async Task NoFallback()
        {
            var adapter = new TestAdapter(CreateConversation("NoFallback"))
                .Use(new SetSpeakMiddleware("male", false));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var activity = MessageFactory.Text("OK");

                await context.SendActivityAsync(activity);
            })
                .Send("foo")
                .AssertReply(obj =>
                {
                    var activity = obj.AsMessageActivity();
                    Assert.Null(activity.Speak);
                })
                .StartTestAsync();
        }

        // fallback is true, for any ChannelId other than emulator, directlinespeech, or telephony should
        // just set Activity.Speak to Activity.Text if Speak is empty. 
        [Fact]
        public async Task FallbackNullSpeak()
        {
            var adapter = new TestAdapter(CreateConversation("Fallback"))
                .Use(new SetSpeakMiddleware("male", true));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var activity = MessageFactory.Text("OK");

                await context.SendActivityAsync(activity);
            })
                .Send("foo")
                .AssertReply(obj =>
                    {
                        var activity = obj.AsMessageActivity();
                        Assert.Equal(activity.Text, activity.Speak);
                    })
                .StartTestAsync();
        }

        // fallback is true, for any ChannelId other than emulator, directlinespeech, or telephony should
        // leave a non-empty Speak unchanged.
        [Fact]
        public async Task FallbackWithSpeak()
        {
            var adapter = new TestAdapter(CreateConversation("Fallback"))
                .Use(new SetSpeakMiddleware("male", true));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var activity = MessageFactory.Text("OK");
                activity.Speak = "speak value";

                await context.SendActivityAsync(activity);
            })
                .Send("foo")
                .AssertReply(obj =>
                {
                    var activity = obj.AsMessageActivity();
                    Assert.Equal("speak value", activity.Speak);
                })
                .StartTestAsync();
        }

        // Speak is added with no voice tag
        [Theory]
        [InlineData(Channels.Emulator)]
        [InlineData(Channels.DirectlineSpeech)]
        [InlineData("telephony")]
        public async Task AddSpeak_NullVoice(string channelId)
        {
            var adapter = new TestAdapter(CreateConversation("Fallback", channelId: channelId))
                .Use(new SetSpeakMiddleware("male", true));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var activity = MessageFactory.Text("<speak>OK</speak>");

                await context.SendActivityAsync(activity);
            })
                .Send("foo")
                .AssertReply(obj =>
                {
                    var activity = obj.AsMessageActivity();
                    var speakTag = "<speak>OK</speak>";

                    Assert.Equal(speakTag, activity.Speak);
                })
                .StartTestAsync();
        }

        // Speak is added with special or invalid characters
        [Theory]
        [InlineData(Channels.Emulator)]
        [InlineData(Channels.DirectlineSpeech)]
        [InlineData("telephony")]
        public async Task AddSpeak_Malformed_ReturnsXmlException(string channelId)
        {
            var adapter = new TestAdapter(CreateConversation("Fallback", channelId: channelId))
                .Use(new SetSpeakMiddleware("male", true));

            await Assert.ThrowsAsync<XmlException>(async () =>
            {
                await new TestFlow(adapter, async (context, cancellationToken) =>
                {
                    var activity = MessageFactory.Text("<speak version=\"1.0 & 2.0\" xml:lang=\"en-<&&>&us\"><voice name=\"male & female\">OK</voice></speak>");

                    await context.SendActivityAsync(activity);
                })
                .Send("foo")
                .AssertNoReply()
                .StartTestAsync();
            });
        }

        // Voice is added to Speak property.
        [Theory]
        [InlineData(Channels.Emulator)]
        [InlineData(Channels.DirectlineSpeech)]
        [InlineData("telephony")]
        public async Task AddVoice(string channelId)
        {
            var adapter = new TestAdapter(CreateConversation("Fallback", channelId: channelId))
                .Use(new SetSpeakMiddleware("male", true));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var activity = MessageFactory.Text("OK");

                await context.SendActivityAsync(activity);
            })
                .Send("foo")
                .AssertReply(obj =>
                {
                    var activity = obj.AsMessageActivity();
                    var speakTag = "<speak version=\"1.0\" xml:lang=\"en-us\" xmlns=\"http://www.w3.org/2001/10/synthesis\"><voice name=\"male\">OK</voice></speak>";

                    Assert.Equal(speakTag, activity.Speak);
                }).StartTestAsync();
        }

        // Voice is added with no speak tag
        [Theory]
        [InlineData(Channels.Emulator)]
        [InlineData(Channels.DirectlineSpeech)]
        [InlineData("telephony")]
        public async Task AddVoice_NullSpeak(string channelId)
        {
            var adapter = new TestAdapter(CreateConversation("Fallback", channelId: channelId))
                .Use(new SetSpeakMiddleware("male", true));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var activity = MessageFactory.Text("<voice name=\"male\">OK</voice>");

                await context.SendActivityAsync(activity);
            })
                .Send("foo")
                .AssertReply(obj =>
                {
                    var activity = obj.AsMessageActivity();
                    var speakTag = "<speak version=\"1.0\" xml:lang=\"en-us\" xmlns=\"http://www.w3.org/2001/10/synthesis\"><voice name=\"male\">OK</voice></speak>";

                    Assert.Equal(speakTag, activity.Speak);
                })
                .StartTestAsync();
        }

        // Multiple voice tags added to Speak property.
        [Theory]
        [InlineData(Channels.Emulator)]
        [InlineData(Channels.DirectlineSpeech)]
        [InlineData("telephony")]
        public async Task AddMultiVoice(string channelId)
        {
            var adapter = new TestAdapter(CreateConversation("Fallback", channelId: channelId))
                .Use(new SetSpeakMiddleware("male", true));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var activity = MessageFactory.Text("<voice name=\"male\">OK</voice><voice name=\"female\">OK</voice>");

                await context.SendActivityAsync(activity);
            })
                .Send("foo")
                .AssertReply(obj =>
                {
                    var activity = obj.AsMessageActivity();
                    var speakTag = "<speak version=\"1.0\" xml:lang=\"en-us\" xmlns=\"http://www.w3.org/2001/10/synthesis\"><voice name=\"male\">OK</voice><voice name=\"female\">OK</voice></speak>";

                    Assert.Equal(speakTag, activity.Speak);
                })
                .StartTestAsync();
        }

        // With no 'voice' specified, the Speak property is unchanged.
        [Theory]
        [InlineData(Channels.Emulator)]
        [InlineData(Channels.DirectlineSpeech)]
        [InlineData("telephony")]
        public async Task AddNoVoice(string channelId)
        {
            var adapter = new TestAdapter(CreateConversation("Fallback", channelId: channelId))
                .Use(new SetSpeakMiddleware(null, true));

            await new TestFlow(adapter, async (context, cancellationToken) =>
            {
                var activity = MessageFactory.Text("OK");

                await context.SendActivityAsync(activity);
            })
                .Send("foo")
                .AssertReply(obj =>
                {
                    var activity = obj.AsMessageActivity();
                    Assert.Equal(
                        "OK",
                        activity.Speak);
                })
                .StartTestAsync();
        }

        private static ConversationReference CreateConversation(string name, string user = "User1", string bot = "Bot", string channelId = "test")
        {
            return new ConversationReference
            {
                ChannelId = channelId,
                ServiceUrl = "https://test.com",
                Conversation = new ConversationAccount(false, name, name),
                User = new ChannelAccount(id: user.ToLowerInvariant(), name: user),
                Bot = new ChannelAccount(id: bot.ToLowerInvariant(), name: bot),
                Locale = "en-us"
            };
        }
    }
}
