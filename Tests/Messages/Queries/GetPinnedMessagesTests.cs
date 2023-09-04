﻿using Application.Models;
using Application.Queries.GetPinnedMessages;
using MongoDB.Driver;
using Tests.Common;

namespace Tests.Messages.Queries
{
    public class GetPinnedMessagesTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            var chatId = Ids.GroupChat3;

            GetPinnedMessagesRequest request = new()
            {
                ChatId = chatId
            };
            GetPinnedMessagesRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            Context.SetToken(CancellationToken);
            List<Message> result = await handler.Handle(request, CancellationToken);

            //Assert
            Assert.True(result.All(message => Context.Messages.FindAsync(message.Id).Result.IsPinned));
        }
    }
}