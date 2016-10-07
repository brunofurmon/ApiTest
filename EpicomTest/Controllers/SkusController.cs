using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ApiTest.Models;
using ApiTest.Services;


namespace ApiTest.Controllers
{
    public class SkusController : ApiController
    {
        private IAbstractService<Sku> service { get; set; }

        public SkusController() : base()
        {
            this.service = new SkuService();
        }

        // GET: api/skus
        [HttpGet]
        [Route("api/skus")]
        [ResponseType(typeof(List<Sku>))]
        public IHttpActionResult List()
        {
            List<Sku> list = service.List();
            return Ok(list);
        }

        // GET: api/skus/5
        [HttpGet]
        [Route("api/skus/{id}")]
        [ResponseType(typeof(Sku))]
        public IHttpActionResult GetSku(int id)
        {
            Sku sku = service.Get(id);
            if (sku == null)
            {
                return NotFound();
            }

            return Ok(sku);
        }

        // PUT: api/skus/5
        [HttpPut]
        [Route("api/skus/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSku(int id, Sku sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sku.Id)
            {
                return BadRequest();
            }

            try
            {
                service.Update(id, sku);
            }
            catch (DbUpdateConcurrencyException)
            {
                // TODO: Verificar o que acontece
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/skus
        [HttpPost]
        [ResponseType(typeof(Sku))]
        public IHttpActionResult PostSku(Sku sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            service.Create(sku);

            return CreatedAtRoute("DefaultApi", new { id = sku.Id }, sku);
        }

        // DELETE: api/skus/5
        [HttpDelete]
        [Route("api/skus/{id}")]
        [ResponseType(typeof(Sku))]
        public IHttpActionResult DeleteSku(int id)
        {
            Sku sku = service.Get(id);
            if (sku == null)
            {
                return NotFound();
            }

            service.Delete(id);

            return Ok(sku);
        }
    }
}