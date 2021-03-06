﻿using FluentAssertions;
using Pubg.Net.Tests.Util;
using Pubg.Net.Extensions;
using System.Linq;
using Xunit;
using Pubg.Net.Exceptions;
using System;
using pubg.net.Tests;

namespace Pubg.Net.Tests.Matches
{
    public class MatchTests : TestBase
    {
        [Fact]
        public void Can_Retrieve_Match()
        {
            var region = PubgRegion.PCEurope;
            var samples = Storage.GetSamples(region);
            var matchService = new PubgMatchService(Storage.ApiKey);

            var match = matchService.GetMatch(region, samples.MatchIds.FirstOrDefault());

            match.ShardId.Should().Equals(region.Serialize());
            match.Rosters.Should().NotBeNull();

            var participants = match.Rosters.SelectMany(x => x.Participants);

            participants.Should().NotBeNullOrEmpty();
            
            Assert.All(participants, p => p.Stats.Should().NotBeNull());
            Assert.All(participants, p => p.ShardId.Should().Equals(region.Serialize()));
            Assert.All(participants, p => p.Id.Should().NotBeNullOrWhiteSpace());
        }

        [Fact]
        public void Throws_Exception_When_NotFound()
        {
            Assert.Throws<PubgNotFoundException>(() => new PubgMatchService(Storage.ApiKey).GetMatch(PubgRegion.PCEurope, Guid.Empty.ToString()));
        }
    }
}
