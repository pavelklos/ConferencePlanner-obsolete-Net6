using ConferencePlanner.GraphQL.Attendees;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using ConferencePlanner.GraphQL.Sessions;
using ConferencePlanner.GraphQL.Speakers;
using ConferencePlanner.GraphQL.Tracks;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Snapshooter.Xunit;

namespace GraphQL.Tests
{
    public class AttendeeTests
    {
        [Fact]
        public async Task Attendee_Schema_Changed()
        {
            // arrange
            // act
            ISchema schema = await new ServiceCollection()

                .AddPooledDbContextFactory<ApplicationDbContext>(
                    (s, o) => o
                        .UseSqlite("Data Source=conferences.db"))

                .AddGraphQLServer()

                .AddQueryType()
                .AddMutationType()
                .AddSubscriptionType()

                .AddTypeExtension<AttendeeQueries>()
                .AddTypeExtension<AttendeeMutations>()
                .AddTypeExtension<AttendeeSubscriptions>()
                .AddTypeExtension<AttendeeNode>()
                .AddDataLoader<AttendeeByIdDataLoader>()

                .AddTypeExtension<SessionQueries>()
                .AddTypeExtension<SessionMutations>()
                .AddTypeExtension<SessionSubscriptions>()
                .AddTypeExtension<SessionNode>()
                .AddDataLoader<SessionByIdDataLoader>()
                .AddDataLoader<SessionBySpeakerIdDataLoader>()

                .AddTypeExtension<SpeakerQueries>()
                .AddTypeExtension<SpeakerMutations>()
                .AddTypeExtension<SpeakerNode>()
                .AddDataLoader<SpeakerByIdDataLoader>()
                .AddDataLoader<SessionBySpeakerIdDataLoader>()

                .AddTypeExtension<TrackQueries>()
                .AddTypeExtension<TrackMutations>()
                .AddTypeExtension<TrackNode>()
                .AddDataLoader<TrackByIdDataLoader>()

                .AddType<UploadType>()

                // In this section we are adding extensions like relay helpers,
                // filtering and sorting.
                .AddFiltering()
                .AddSorting()

                // .EnableRelaySupport()
                .AddGlobalObjectIdentification()

                .BuildSchemaAsync();

            // assert
            schema.Print().MatchSnapshot();
        }

        [Fact]
        public async Task Attendee_Schema_Changed_Version_HotChocolate_11()
        {
            //ISchema schema = await new ServiceCollection()
            //    .AddPooledDbContextFactory<ApplicationDbContext>(
            //        options => options.UseInMemoryDatabase("Data Source=conferences.db"))
            //    .AddGraphQL()
            //    .AddQueryType(d => d.Name("Query"))
            //        .AddTypeExtension<AttendeeQueries>()
            //    .AddMutationType(d => d.Name("Mutation"))
            //        .AddTypeExtension<AttendeeMutations>()
            //    .AddType<AttendeeType>()
            //    .AddType<SessionType>()
            //    .AddType<SpeakerType>()
            //    .AddType<TrackType>()
            //    .EnableRelaySupport()
            //    .BuildSchemaAsync();

            //schema.Print().MatchSnapshot();
        }
    }
}