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

                //.AddPooledDbContextFactory<ApplicationDbContext>(
                //    (s, o) => o.UseInMemoryDatabase("Data Source=conferences.db"))
                //.AddPooledDbContextFactory<ApplicationDbContext>(
                //    options => options.UseInMemoryDatabase("Data Source=conferences.db"))
                .AddDbContextPool<ApplicationDbContext>(
                    options => options.UseInMemoryDatabase("Data Source=conferences.db"))

                .AddGraphQL()
                //.AddGraphQLServer()

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
        public async Task Register_Attendee_By_Executor()
        {
            // arrange
            IRequestExecutor executor = await new ServiceCollection()

                //.AddPooledDbContextFactory<ApplicationDbContext>(
                //    (s, o) => o.UseInMemoryDatabase("Data Source=conferences.db"))
                //.AddPooledDbContextFactory<ApplicationDbContext>(
                //    options => options.UseInMemoryDatabase("Data Source=conferences.db"))
                .AddDbContextPool<ApplicationDbContext>(
                    options => options.UseInMemoryDatabase("Data Source=conferences.db"))

                .AddGraphQL()
                //.AddGraphQLServer()

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

                .BuildRequestExecutorAsync();

            // act
            IExecutionResult result = await executor.ExecuteAsync(@"
                mutation Register_Attendee {
                    registerAttendee(
                    input: {
                        emailAddress: ""michael@chillicream.com""
                        firstName: ""michael""
                        lastName: ""staib""
                        userName: ""michael1""
                    }
                    ) {
                    attendee {
                        id
                    }
                    }
                }
            ");

            // assert
            result.ToJson().MatchSnapshot();
        }

        [Fact]
        public async Task Register_Attendee_By_Services()
        {
            // arrange
            IServiceProvider services = new ServiceCollection()

                //.AddPooledDbContextFactory<ApplicationDbContext>(
                //    (s, o) => o.UseInMemoryDatabase("Data Source=conferences.db"))
                //.AddPooledDbContextFactory<ApplicationDbContext>(
                //    options => options.UseInMemoryDatabase("Data Source=conferences.db"))
                .AddDbContextPool<ApplicationDbContext>(
                    options => options.UseInMemoryDatabase("Data Source=conferences.db"))
                
                .AddGraphQL()
                //.AddGraphQLServer()

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
                //.AddGlobalObjectIdentification()

                .Services
                .BuildServiceProvider();

            // act
            IExecutionResult result = await services.ExecuteRequestAsync(
                QueryRequestBuilder.New()
                    .SetQuery(@"
                        mutation Register_Attendee {
                            registerAttendee(
                            input: {
                                emailAddress: ""michael@chillicream.com""
                                firstName: ""michael""
                                lastName: ""staib""
                                userName: ""michael1""
                            }
                            ) {
                            attendee {
                                id
                            }
                            }
                        }
                    ")
                    .Create());

            // assert
            result.ToJson().MatchSnapshot();
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