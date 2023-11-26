namespace Updater.Repositories.Technology;

using Shared.Domain.Entities;

public interface ITechnologyRepository
{
    Task<Technology> InsertAsync(Technology technology);
}