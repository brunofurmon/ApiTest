using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EpicomTest.Models;
using EpicomTest.Daos;

namespace EpicomTest.Controllers
{
    public class SkusController : ApiController
    {
        private SkuDao db = new SkuDao();

        // GET: api/Skus
        public IQueryable<Sku> GetSkus()
        {
            return db.Skus;
        }

        // GET: api/Skus/5
        [ResponseType(typeof(Sku))]
        public async Task<IHttpActionResult> GetSku(int id)
        {
            Sku sku = await db.Skus.FindAsync(id);
            if (sku == null)
            {
                return NotFound();
            }

            return Ok(sku);
        }

        // PUT: api/Skus/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSku(int id, Sku sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sku.SkuId)
            {
                return BadRequest();
            }

            db.Entry(sku).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkuExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Skus
        [ResponseType(typeof(Sku))]
        public async Task<IHttpActionResult> PostSku(Sku sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Skus.Add(sku);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = sku.SkuId }, sku);
        }

        // DELETE: api/Skus/5
        [ResponseType(typeof(Sku))]
        public async Task<IHttpActionResult> DeleteSku(int id)
        {
            Sku sku = await db.Skus.FindAsync(id);
            if (sku == null)
            {
                return NotFound();
            }

            db.Skus.Remove(sku);
            await db.SaveChangesAsync();

            return Ok(sku);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SkuExists(int id)
        {
            return db.Skus.Count(e => e.SkuId == id) > 0;
        }
    }
}