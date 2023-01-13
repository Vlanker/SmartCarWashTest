#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartCarWashTest.WebApi.DTOs.Interfaces;

namespace SmartCarWashTest.WebApi.Repositories.Interfaces
{
    public interface IRepository<TCreateDto, TReadDto, TUpdateDto>
        where TCreateDto : ICreateModel
        where TReadDto : IReadModel
        where TUpdateDto : IUpdateModel
    {
        /// <summary>
        /// Add entity in database context.
        /// </summary>
        /// <param name="createModel"> The entity to add. </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns> Added <see cref="createModel" />. </returns>
        Task<TReadDto?> CreateAsync(TCreateDto createModel, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a collection that contains the values.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns></returns>
        Task<IEnumerable<TReadDto>> RetrieveAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to get the value associated with the <paramref name="id" />.
        /// </summary>
        /// <param name="id"> The ID of the value to get. </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns></returns>
        Task<TReadDto?> RetrieveAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the value associated with <paramref name="id"/> to <paramref name="updateModel"/>
        /// if the existing value with key is equal to comparison value.
        /// </summary>
        /// <param name="id"> The ID of the value that is compared with comparison value and possibly replaced. </param>
        /// <param name="updateModel">
        /// The value that replaces the value of the element that has the specified ID if the comparison results in
        /// equality.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns></returns>
        Task<TReadDto?> UpdateAsync(int id, TUpdateDto updateModel, CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to remove the value associated with the <paramref name="id" />.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns></returns>
        Task<bool?> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}