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
        private IAbstractService<Disponibilidade> dispService { get; set; }
        private IOrderService orderService { get; set; }

        public SkusController() : base()
        {
            this.skuService = new SkuService();
            this.dispService = new DisponibilidadeService();
            this.orderService = new OrderService();
        }

        #region Skus
        // GET: api/skus
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<Sku>))]
        public IHttpActionResult List()
        {
            // For simplicity, Disponibilidades will be added here
            List<Sku> list = skuService.List();
            List<Sku> expandedList = new List<Sku>();
            foreach (Sku s in list)
            {
                Sku newSku = ExpandedSku(s);
                expandedList.Add(newSku);
            }
            return Ok(expandedList);
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

            Sku newSku = ExpandedSku(sku);
            
            return Ok(sku);
        }

        // PATCH: api/skus/5
        [HttpPatch]
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateSku(int id, Sku sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Sku returnedSku;
            Sku alteredSku = sku;
            try
            {
                alteredSku.Id = id;
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

            return Ok();
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
            Sku sku = skuService.Get(skuId);
            if (sku == null)
            {
                return NotFound();
            }

            Disponibilidade disp = dispService.Get(dispId);
            if (disp == null)
            {
                return NotFound();
            }

            return Ok(disp);
        }

        // Delete
        [HttpDelete]
        [Route("{skuId}/disponibilidades/{dispId}")]
        [ResponseType(typeof(Sku))]
        public IHttpActionResult DeleteDisponibilidade(int skuId, int dispId)
        {
            Sku sku = skuService.Get(skuId);
            if (sku == null)
            {
                return NotFound();
            }

            Disponibilidade disp = dispService.Get(dispId);
            if (disp == null)
            {
                return NotFound();
            }

            dispService.Delete(disp.Id);

            return Ok();
        }

        // Post skuId/disponibilidades
        [HttpPost]
        [Route("{skuId}/disponibilidades")]
        public IHttpActionResult CreateDisponibilidade(int skuId, Disponibilidade disp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Sku sku = skuService.Get(skuId);
            if (sku == null)
            {
                return NotFound();
            }

            disp.SkuId = skuId;
            dispService.Create(disp);

            sku.Disponibilidades.Add(disp);
            skuService.Update(sku);

            return Ok(sku);
        }

        // Patch skuId/disponibilidades
        [HttpPatch]
        [Route("{skuId}/disponibilidades/{dispId}")]
        public IHttpActionResult UpdateDisponibilidade(int skuId, int dispId, Disponibilidade disp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Sku sku = skuService.Get(skuId);
            if (sku == null)
            {
                return NotFound();
            }

            Disponibilidade returnedDisp;
            Disponibilidade alteredDisp = disp;
            try
            {
                alteredDisp.Id = dispId;
                returnedDisp = dispService.Update(alteredDisp);
            }
            // Occurs whenever an user attaches an instance (with valid identifier) that was already attached.
            // Usually happens when you try to attach an altered entity on the top of an existing one
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            if (returnedDisp == null)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
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

        private Sku ExpandedSku(Sku sku)
        {
            if (sku == null)
            {
                return null;
            }

            Sku expandedSku = sku;
            List<Disponibilidade> disponibilidades = dispService.Search(d => d.SkuId == sku.Id);
            expandedSku.Disponibilidades = disponibilidades;

            return expandedSku;
        }
    }
}