using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Diagnostics;
using Talabat.APis.DTOS;
using Talabat.APis.Errors;
using Talabat.APis.Helper;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APis.Controllers
{
    // controlerr=> repo => to ascced DB
    public class ProductController : BaseApiController
    {
    //    private readonly IGenericRepository<Product> _productRepo;
    //    private readonly IGenericRepository<ProductBrand> _brandsRepo;
    //    private readonly IGenericRepository<ProductType> _typesRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(
            //IGenericRepository<Product> ProductRepo,
            //IGenericRepository<ProductBrand> BrandsRepo,
            //IGenericRepository<ProductType> TypesRepo,
            IMapper mapper, IUnitOfWork unitOfWork)// ask clr to creaka aobject of Produvtreo
        {
            //_productRepo = ProductRepo;
            //_brandsRepo = BrandsRepo;
            //_typesRepo = TypesRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        #region Pagination
        // is a topic => GetAll() why as we return a data if the size of s=data is lager (ram will burn )
        // we must apply Pagination (Connecec model (apply pagination in front and back and (many request))disconneetr model(allpy front) )
        // front end send me 2  info (size of data and index of data)
        // size of data deon model if lagrr connected (like E+Commerc)
        //clean code princlpe (if fun take more than  3 paramter we must make a object rprest this paramter)
        #endregion

        //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)] // the schmea of this wnd point is Bearer 
        //[Authorize]
        [CachedAttribute(600)] // ActionFilter
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery] ProductSpecParms productParms)
        {
            // endpoint in api retun a resposne that we nedd to sep the redu 
            var spec = new ProductTypeandBrandSpecification(productParms);
            var products = await _unitOfWork.Repository<Product>().GetALlWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
            // we send a new request to database to know the count of items based on filteration 
            var specCount = new ProductWithFilterationandSpec(productParms);
            var count = await _unitOfWork.Repository<Product>().GetCountWithSPecAsync(specCount);
            return Ok(new Pagination<ProductToReturnDTO>(productParms.PageIndex, productParms.PageSize, data,count));// OK() is a Helpper methodlIke Viewin Mvc that make a object from class OK ObjectResut thmak make a stated code 200
        }

        [CachedAttribute(600)]
        [ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProductById(int id)
        {
            var spec = new ProductTypeandBrandSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityNWithSpecAsync(spec);
            if (product is null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product, ProductToReturnDTO>(product));
        }
        [CachedAttribute(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _unitOfWork.Repository<ProductBrand>().GetALlAsync();
            return Ok(Brands);
        }
        [CachedAttribute(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetALlAsync();
            return Ok(types);
        }
    }
    }

  
