#nullable enable
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SmartCarWashTest.Common.DataContext.Sqlite;
using SmartCarWashTest.WebApi.DTO.Interfaces;
using SmartCarWashTest.WebApi.Repositories.Interfaces;

namespace SmartCarWashTest.WebApi.Repositories.Repositories.Base
{
    public abstract class BaseCacheRepository<TCreateDto, TReadDto, TUpdateDto, TEntity> :
        IRepository<TCreateDto, TReadDto, TUpdateDto>
        where TCreateDto : class, ICreateModel
        where TReadDto : class, IReadModel, IHaveIdentifier
        where TUpdateDto : class, IUpdateModel
        where TEntity : class, Common.EntityModels.Sqlite.Interfaces.IHaveIdentifier
    {
        private static ConcurrentDictionary<int, TReadDto>? _cache;

        private readonly SmartCarWashContext _smartCarWashContext;
        private readonly IMapper _mapper;

        protected BaseCacheRepository(SmartCarWashContext smartCarWashContext, IMapper mapper)
        {
            _smartCarWashContext = smartCarWashContext;
            _mapper = mapper;

            var collection = _smartCarWashContext
                .Set<TEntity>()
                .AsNoTracking()
                .ProjectTo<TReadDto>(_mapper.ConfigurationProvider)
                .ToDictionary(dto => dto.Id);

            _cache ??= new ConcurrentDictionary<int, TReadDto>(collection);
        }

        /// <summary>
        /// Attempts to get the comparison value associated with <paramref name="id" /> from the <see cref="_cache" />.
        /// Updates the value associated with <paramref name="id" /> to <paramref name="updateModel" />
        /// if the existing value with <paramref name="id" /> is equal to comparison value.
        /// </summary>
        /// <param name="id"> The ID of the value that is compared with comparison value and possibly replaced. </param>
        /// <param name="updateModel">
        /// The value that replaces the value of the element that has the specified ID if the comparison results in
        /// equality.
        /// </param>
        /// <returns></returns>
        private TReadDto UpdateCache(int id, TReadDto updateModel)
        {
            if (_cache is null)
            {
                return null!;
            }

            if (!_cache.TryGetValue(id, out var comparisonValue))
            {
                return null!;
            }

            if (_cache.TryUpdate(id, updateModel, comparisonValue))
            {
                return updateModel;
            }

            return null!;
        }

        public virtual async Task<TReadDto?> CreateAsync(TCreateDto createModel, CancellationToken cancellationToken)
        {
            // create new entity from model
            var entity = _mapper.Map<TEntity>(createModel);

            // add to data set, id should be generated
            await _smartCarWashContext
                .Set<TEntity>()
                .AddAsync(entity, cancellationToken);

            // save to database
            var affected = await _smartCarWashContext
                .SaveChangesAsync(cancellationToken);

            if (affected == 1)
            {
                var createEntity = _mapper.Map<TReadDto>(entity);
                
                // if the model is new, add it to cache, else
                // call UpdateCache method
                _cache.AddOrUpdate(entity!.Id, createEntity, UpdateCache);
                
                // convert to model
                var readModel = await RetrieveAsync(entity.Id, cancellationToken);
                
                return readModel;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<TReadDto>> RetrieveAllAsync(CancellationToken cancellationToken)
        {
            // for performance, get from cache
            return await Task.FromResult(_cache?.Values ?? Enumerable.Empty<TReadDto>());
        }

        public async Task<TReadDto?> RetrieveAsync(int id, CancellationToken cancellationToken)
        {
            // for performance, get from cache
            if (_cache is null)
            {
                return null;
            }

            _cache.TryGetValue(id, out var model);

            return await Task.FromResult(model);
        }

        public async Task<TReadDto?> UpdateAsync(int id, TUpdateDto updateModel, CancellationToken cancellationToken)
        {
            var keyValues = new object[] { id };

            // find entity to update by id, not model id
            var entity = await _smartCarWashContext
                .Set<TEntity>()
                .FindAsync(keyValues, cancellationToken);

            if (entity == null)
            {
                return null;
            }

            // copy updates from model to entity
            _mapper.Map(updateModel, entity);

            // save updates
            var affected = await _smartCarWashContext.SaveChangesAsync(cancellationToken);

            if (affected == 1)
            {
                var updateEntity = _mapper.Map<TReadDto>(updateModel);
                // update in cache
                UpdateCache(id, updateEntity);
                
                var readModel = await RetrieveAsync(entity.Id, cancellationToken);
                
                return readModel;
            }

            return null;
        }

        public async Task<bool?> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var entities = _smartCarWashContext.Set<TEntity>();
            var keyValues = new object[] { id };

            // find entity to delete by id
            var entity = await entities.FindAsync(keyValues, cancellationToken);

            if (entity == null)
            {
                if (_cache is null)
                {
                    return null;
                }

                // remove from cache
                return _cache.TryRemove(id, out _);
            }

            // delete entry
            entities.Remove(entity);

            // save 
            var affected = await _smartCarWashContext.SaveChangesAsync(cancellationToken);

            if (affected == 1)
            {
                if (_cache is null)
                {
                    return null;
                }

                // remove from cache
                return _cache.TryRemove(id, out _);
            }
            else
            {
                return null;
            }
        }
    }
}