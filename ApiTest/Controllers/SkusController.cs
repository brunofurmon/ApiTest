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
        public IHttpActionResult PutSku(int id, SkuForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Sku newSku = Sku.FromForm(form);
            Sku returnedSku;
            try
            {
                newSku.Id = id;
                returnedSku = skuService.Update(newSku);
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