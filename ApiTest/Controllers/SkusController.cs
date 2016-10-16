using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ApiTest.Models;
using ApiTest.Services;


namespace ApiTest.Controllers
{
    [RoutePrefix("api/skus")]
    public class SkusController : ApiController
    {
        private IAbstractService<Sku> skuService { get; set; }
        private IAbstractService<Disponibilidade> dispService { get; set; }
        private IAbstractService<Imagem> imagemService { get; set; }
        private IAbstractService<Dimensoes> dimensoesService { get; set; }
        private IAbstractService<Grupo> grupoService { get; set; }
        private IAbstractService<AtributoDoGrupo> atributoDoGrupoService { get; set; }
        private IAbstractService<SkuMarketplaceGetResponse> marketplaceService { get; set; }
        private IOrderService orderService { get; set; }

        public SkusController() : base()
        {
            this.skuService = new SkuService();
            this.dispService = new DisponibilidadeService();
            this.dimensoesService = new DimensoesService();
            this.imagemService = new ImagemService();
            this.grupoService = new GrupoService();
            this.atributoDoGrupoService = new AtributoDoGrupoService();
            this.marketplaceService = new MarketplaceService();
            //this.orderService = new OrderService();
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

            return Ok();
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

            return Created("api", new{});
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
            // Here, Dimensoes share a common Id, so it needs to be explicitly erased in the same scope
            dimensoesService.Delete(id);
            skuService.Delete(id);

            return Ok();
        }
        #endregion Skus

        //[HttpPost]
        //[Route("order")]
        //public IHttpActionResult ProcessOrder(OrderForm[] orders)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    foreach (OrderForm order in orders)
        //    {
        //        try
        //        {
        //            orderService.ProcessOrder(order);
        //        }
        //        catch (OrderException ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    return Ok(orders);
        //}

        // In OData, this is similar to the ?$expand=[prop1,prop2,...] call
        private Sku ExpandedSku(Sku sku)
        {
            if (sku == null)
            {
                return null;
            }
            Sku expandedSku = sku;

            // Disponibilidades
            List<Disponibilidade> disponibilidades = dispService.Search(d => d.SkuId == sku.Id);
            expandedSku.Disponibilidades = disponibilidades;

            // Imagens
            List<Imagem> imagens = imagemService.Search(i => i.SkuId == sku.Id);
            expandedSku.Imagens = imagens;

            // Grupos
            List<Grupo> grupos = grupoService.Search(g => g.SkuId == sku.Id);
            List<Grupo> completeGrupos = new List<Grupo>();
            foreach (Grupo g in grupos)
            {
                List<AtributoDoGrupo> atributos = atributoDoGrupoService.Search(a => a.GrupoId == g.Id);
                Grupo completeGrupo = g;
                completeGrupo.Atributos = atributos;
                completeGrupos.Add(completeGrupo);
            }
            expandedSku.Grupos = completeGrupos;

            // Dimensoes
            Dimensoes dimensoes = dimensoesService.Get(sku.Id);
            expandedSku.Dimensoes = dimensoes;

            // Marketplace
            List<SkuMarketplaceGetResponse> marketplaces = marketplaceService.Search(m => m.SkuId == sku.Id);
            expandedSku.Marketplaces = marketplaces;

            return expandedSku;
        }
    }
}