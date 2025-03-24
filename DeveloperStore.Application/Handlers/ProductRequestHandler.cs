using AutoMapper;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Application.Notifications;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Services;
using DeveloperStore.Domain.Interfaces.Cache;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class ProductRequestHandler :
    IRequestHandler<ProductCreateCommand, ProductQuery>,
    IRequestHandler<ProductDeleteCommand, ProductQuery>,
    IRequestHandler<ProductUpdateCommand, ProductQuery>
{
    private readonly IProductDomainService _productDomainService;
    private readonly IProductCache _productCache;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ProductRequestHandler(
        IProductDomainService productDomainService,
        IProductCache productCache,
        IMapper mapper,
        IMediator mediator)
    {
        _productDomainService = productDomainService;
        _productCache = productCache;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<ProductQuery> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = _mapper.Map<Product>(request);

            // SQL Server
            await _productDomainService.AddAsync(product).ConfigureAwait(false);

            // MongoDB (cache)
            await _productCache.AddAsync(product).ConfigureAwait(false);

            // Notification
            var notification = new ProductNotification(product, ActionNotification.AddAsync);
            await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);

            return _mapper.Map<ProductQuery>(product);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<ProductQuery> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productDomainService.GetByIdAsync(request.Id).ConfigureAwait(false);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            // Remove from SQL Server
            await _productDomainService.DeleteAsync(product).ConfigureAwait(false);

            // Remove from MongoDB (cache)
            await _productCache.DeleteAsync(product).ConfigureAwait(false);

            // Notification
            var notification = new ProductNotification(product, ActionNotification.DeleteAsync);
            await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);

            return _mapper.Map<ProductQuery>(product);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<ProductQuery> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productDomainService.GetByIdAsync(request.Id).ConfigureAwait(false);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            product.Name = request.Name;
            product.UnitPrice = request.UnitPrice;
            product.Photo = request.Photo;
            product.IsDeleted=request.IsDeleted;

            // Update in SQL Server
            await _productDomainService.UpdateAsync(product).ConfigureAwait(false);

            // Update in MongoDB (cache)
            await _productCache.UpdateAsync(product).ConfigureAwait(false);

            // Notification
            var notification = new ProductNotification(product, ActionNotification.UpdateAsync);
            await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);

            return _mapper.Map<ProductQuery>(product);
        }
        catch (Exception)
        {

            throw;
        }
    }
}
