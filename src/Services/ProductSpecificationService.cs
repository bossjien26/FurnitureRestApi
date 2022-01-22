using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories;
using Repositories.Interface;
using Services.Dto;
using Services.Interface;

namespace Services
{
    public class ProductSpecificationService : IProductSpecificationService
    {

        private readonly IProductSpecificationRepository _repository;

        private readonly ISpecificationRepository _specificationRepository;

        private readonly ISpecificationContentRepository _specificationContentRepository;

        private readonly IInventorySpecificationRepository _inventorySpecificationRepository;

        public ProductSpecificationService(DbContextEntity contextEntity)
        {
            _repository = new ProductSpecificationRepository(contextEntity);
            _specificationRepository = new SpecificationRepository(contextEntity);
            _specificationContentRepository = new SpecificationContentRepository(contextEntity);
            _inventorySpecificationRepository = new InventorySpecificationRepository(contextEntity);
        }

        public ProductSpecificationService(IProductSpecificationRepository genericRepository)
            => _repository = genericRepository;

        public async Task Insert(ProductSpecification instance)
        => await _repository.Insert(instance);

        public IQueryable<ProductSpecification> GetMany(int productId)
        => _repository.GetAll().Where(x => x.ProductId == productId);

        public IQueryable<ProductSpecification> GetByNextSpecification(int productId, int? id)
        {
            return (id == null) ? _repository.GetAll().Where(x => x.ProductId == productId) 
            : _repository.GetAll().Where(x => x.ProductId == productId && x.Id > id);
        }

        public IQueryable<ProductSpecificationJoinSpecification> GetManyJoinSpecification(int productId)
        => _repository.GetAll().Where(x => x.ProductId == productId)
            .Join(
                _specificationRepository.GetAll(),
                productSpecification => productSpecification.SpecificationId,
                specification => specification.Id,
                (x, y) => new { ProductSpecification = x, Specification = y }
            )
            .Where(x => x.Specification.IsDelete == false)
            .Select(
                x => new ProductSpecificationJoinSpecification()
                {
                    Id = x.Specification.Id,
                    Name = x.Specification.Name
                }
            );

        public IQueryable<int> GetOneJoinSpecificationByProductId(int productId, List<int> specificationContents)
        {
            var IQueryable = _repository.GetAll().Where(x => x.ProductId == productId)
            .Join(
                _specificationRepository.GetAll(),
                productSpecification => productSpecification.SpecificationId,
                specification => specification.Id,
                (x, y) => new { ProductSpecification = x, Specification = y }
            ).Join(
                _specificationContentRepository.GetAll(),
                productSpecification => productSpecification.Specification.Id,
                specificationContent => specificationContent.SpecificationId,
                (ProductSpecification, SpecificationContent)
                    => new { ProductSpecification, SpecificationContent }
            ).Join(
                _inventorySpecificationRepository.GetAll(),
                productSpecification => productSpecification.SpecificationContent.Id,
                inventorySpecification => inventorySpecification.SpecificationContentId,
                (ProductSpecification, InventorySpecification) =>
                    new { ProductSpecification, InventorySpecification }
            );

            if (specificationContents.Count() > 0)
            {
                IQueryable = IQueryable.Where(x => specificationContents.Any(z => z == x.InventorySpecification.SpecificationContentId));
            }

            return IQueryable.Select(
                x => x.InventorySpecification.InventoryId
            );
        }

        public IQueryable<InventoryIdBySpecifications> GetBySpecificationContent(int productId, List<int> specifications)
        {
            var IQueryable = _repository.GetAll().Where(x => x.ProductId == productId)
            .Join(
                _specificationRepository.GetAll(),
                productSpecification => productSpecification.SpecificationId,
                specification => specification.Id,
                (x, y) => new { ProductSpecification = x, Specification = y }
            ).Join(
                _specificationContentRepository.GetAll(),
                productSpecification => productSpecification.Specification.Id,
                specificationContent => specificationContent.SpecificationId,
                (ProductSpecification, SpecificationContent)
                    => new { ProductSpecification, SpecificationContent }
            ).Join(
                _inventorySpecificationRepository.GetAll(),
                productSpecification => productSpecification.SpecificationContent.Id,
                inventorySpecification => inventorySpecification.SpecificationContentId,
                (ProductSpecification, InventorySpecification) =>
                    new { ProductSpecification, InventorySpecification }
            );

            if (specifications.Count() > 0)
            {
                IQueryable = IQueryable.Where(x => specifications.Any(z => z == x.ProductSpecification.ProductSpecification.Specification.Id));
            }

            return IQueryable.Select(
                x => new InventoryIdBySpecifications()
                {
                    Id = x.ProductSpecification.ProductSpecification.Specification.Id,
                    Name = x.ProductSpecification.ProductSpecification.Specification.Name,
                    InventoryIdBySpecificationContent = new InventoryIdBySpecificationContents(){
                        Id = x.ProductSpecification.SpecificationContent.Id,
                        Name = x.ProductSpecification.SpecificationContent.Name
                    }
                }
            );
        }

        public IQueryable<InventoryIdBySpecifications> GetByInventoryIds(List<int> inventoryIds, int productSpecificationId)
        {
            var IQueryable = _repository.GetAll()
            .Join(
                _inventorySpecificationRepository.GetAll(),
                productSpecification => productSpecification.Id,
                inventorySpecification => inventorySpecification.ProductSpecificationId,
                (ProductSpecification, InventorySpecification) => new { ProductSpecification, InventorySpecification }
            ).Join(
                _specificationRepository.GetAll(),
                ProductSpecification => ProductSpecification.ProductSpecification.SpecificationId,
                Specification => Specification.Id,
                (ProductSpecification, Specification) => new { ProductSpecification, Specification }
            ).Join(
                _specificationContentRepository.GetAll(),
                ProductSpecification => ProductSpecification.ProductSpecification.InventorySpecification.SpecificationContentId,
                SpecificationContent => SpecificationContent.Id,
                (ProductSpecification, SpecificationContent) => new { ProductSpecification, SpecificationContent }
            );
            
            if(inventoryIds.Count > 0){
                IQueryable = IQueryable.Where(x => inventoryIds.Any(z => z == x.ProductSpecification.ProductSpecification.InventorySpecification.InventoryId));
            }

            return IQueryable.Where(x => x.ProductSpecification.ProductSpecification.InventorySpecification.ProductSpecificationId == productSpecificationId) 
            .Select(x => new InventoryIdBySpecifications()
            {
                Id = x.ProductSpecification.Specification.Id,
                Name = x.ProductSpecification.Specification.Name,
                InventoryIdBySpecificationContent = new InventoryIdBySpecificationContents(){
                        Id = x.SpecificationContent.Id,
                        Name = x.SpecificationContent.Name
                    }
            });
        }
    }
}