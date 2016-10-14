using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ApiTest.Models;
using ApiTest.Services;
using ApiTest.Dto;
using ApiTest.Exceptions;


namespace ApiTest.Controllers
{
    [RoutePrefix("api/skus")]
    public class SkusController : ApiController
    {
        private IAbstractService<Sku> skuService { get; set; }
        private IOrderService orderService { get; set; }

        public SkusController() : base()
        {
            this.skuService = new SkuService();
            this.orderService = new OrderService();
        }

        #region Skus
        // GET: api/skus
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<Sku>))]
        public IHttpActionResult List()
        {
            List<Sku> list = skuService.List();
            return Ok(list);
        }

        // GET: api/skus/5
        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(Sku))]
        public IHttpActionResult GetSku(int id)
        {
            Sku sku = skuService.Get(id);
            if (sku == null)
            {
                return NotFound();
            }

            return Ok(sku);
        }

        // PATCH: api/skus/5
        [HttpPatch]
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateSku(int id, Sku alteredSku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Sku returnedSku;
            try
            {
                alteredSku.SkuId = id;
                returnedSku = skuService.Update(alteredSku);
            }
            // Occurs whenever an user attaches an instance (with valid identifier) that was already attached.
            // Usually happens when you try to attach an altered entity on the top of an existing one
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            if (returnedSku == null)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/skus
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Sku))]
        public IHttpActionResult PostSku(Sku sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            skuService.Create(sku);

            return Created("api", sku);
        }

        // DELETE: api/skus/5
        [HttpDelete]
        [Route("{id}")]
        [ResponseType(typeof(Sku))]
        public IHttpActionResult DeleteSku(int id)
        {
            Sku sku = skuService.Get(id);
            if (sku == null)
            {
                return NotFound();
            }

            skuService.Delete(id);

            return Ok(sku);
        }
        #endregion Skus

        #region Disponibilidades
        // Get skuId/disponibilidades
        [HttpGet]
        [Route("{skuId}/disponibilidades")]
        [ResponseType(typeof(List<Disponibilidade>))]
        public IHttpActionResult GetDispobilidades(int skuId)
        {
            Sku sku = skuService.Get(skuId);
            if (sku == null)
            {
                return NotFound();
            }

            return Ok(sku.Disponibilidades);
        }

        // Get skuId/disponibilidades/dispId
        [HttpGet]
        [Route("{skuId}/disponibilidades/{dispId}")]
        public IHttpActionResult GetDispobilidade(int skuId, int dispId)
        {
            return null;
        }

        // Delete
        [HttpDelete]
        [Route("{skuId}/disponibilidades/{dispId}")]
        [ResponseType(typeof(Sku))]
        public IHttpActionResult DeleteDisponibilidade(int skuId, int dispId)
        {
            return null;
        }

        // Post skuId/disponibilidades
        [HttpPost]
        [Route("{skuId}/disponibilidades")]
        public IHttpActionResult CreateDisponibilidade(int skuId)
        {
            return null;
        }

        // Patch skuId/disponibilidades
        [HttpPatch]
        [Route("{skuId}/disponibilidades/{dispId}")]
        public IHttpActionResult UpdateDisponibilidade(int skuId, int dispId)
        {
            return null;
        }
        #endregion Disponibilidades

        [HttpPost]
        [Route("order")]
        public IHttpActionResult ProcessOrder(OrderForm[] orders)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            foreach (OrderForm order in orders)
            {
                try
                {
                    orderService.ProcessOrder(order);
                }
                catch (OrderException ex)
                {
                    throw ex;
                }
            }

            return Ok(orders);
        }
    }
}