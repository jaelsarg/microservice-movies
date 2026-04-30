using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.MovieEvents;

public sealed record ScheduleMovieEventInput(
    MovieId MovieId,
    DateTime RequestedShowtime,
    RoomId RoomId
);

public sealed class ScheduleMovieEvent(
    IUnitOfWork uow,
    ILogger<ScheduleMovieEvent> logger
) : IUseCase<ScheduleMovieEventInput, Guid>
{
    public async Task<Guid> Execute(ScheduleMovieEventInput input)
    {
        MovieEvent movieEvent; 

        MovieId movieId = input.MovieId;
        Optional<Movie> movieOptional = await uow.Repo<IMovieRepository>().ById(movieId);
        if (!movieOptional.HasValue) throw new NotFoundException($"Movie with id {movieId} not found.");

        RoomId roomId = input.RoomId;
        Optional<Room> roomOptional = await uow.Repo<IRoomRepository>().ById(roomId);
        if (!roomOptional.HasValue) throw new NotFoundException($"Room with id {roomId} not found.");

        IEnumerable<MovieEvent> existingMovieEvents = await uow.Repo<IMovieEventRepository>().ByShowtimeAndRoomID(input.RequestedShowtime, roomId);

        if (existingMovieEvents.Any())
        {
            movieEvent = existingMovieEvents.First();
            movieEvent.UpdateMovieId(movieId);
            await uow.Repo<IMovieEventRepository>().Save(movieEvent);
        } else
        {
            movieEvent = MovieEvent.Create(
                movieId,
                roomId,
                input.RequestedShowtime,
                roomOptional.Value.Capacity
            );

            await uow.Save<IMovieEventRepository>(movieEvent);
        }

        await uow.Do();

        logger.LogInformation($"Movie event with id {movieEvent.Id} scheduled.");

        return movieEvent.Id.Value;
    }
}