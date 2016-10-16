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
    public class DisponibilidadesController : ApiController
    {
        private IAbstractService<Sku> skuService { get; set; }
        private IAbstractService<Disponibilidade> dispService { get; set; }
        private IOrderService orderService { get; set; }

        public DisponibilidadesController() : base()
        {
            this.skuService = new SkuService();
            this.dispService = new DisponibilidadeService();
        }

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

            List<Disponibilidade> disps = dispService.Search(d => d.SkuId == skuId);

            return Ok(disps);
        }

        // Get skuId/disponibilidades/dispId
        [HttpGet]
        [Route("{skuId}/disponibilidades/{dispId}")]
        [ResponseType(typeof(Disponibilidade))]
        public IHttpActionResult GetDispobilidade(int skuId, int dispId)
        {
            Sku sku = skuService.Get(skuId);
            if (sku == null)
            {
                return NotFound();
            }

            List<Disponibilidade> disp = dispService.Search(d => d.SkuId == skuId && d.Id == dispId);

            if (disp.Count == 0)
            {
                return NotFound();
            }

            return Ok(disp[0]);
        }

        // Delete
        [HttpDelete]
        [Route("{skuId}/disponibilidades/{dispId}")]
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

            return Created("api", "");
        }

        // Patch skuId/disponibilidades
        [HttpPatch]
        [Route("{skuId}/disponibilidades/{dispId}")]
        [ResponseType(typeof(Disponibilidade))]
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

            return Ok();
        }
        #endregion Disponibilidades
    }
}