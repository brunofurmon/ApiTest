using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Web.Http;
using System.Web.Http.Description;
using ApiTest.Models;
using ApiTest.Services;
using System.Linq;
using ApiTest.Dto;
using ApiTest.Exceptions;
using System.Net;
using System.Text;
using System;

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

        [HttpGet]
        [Route("available")]
        [ResponseType(typeof(List<Sku>))]
        public List<Sku> Available(decimal minPrice = 10.00M, decimal maxPrice = 40.00M)
        {
            // First, we get Disponibilidades ordered by Price ascending
            List<Disponibilidade> disponibilidades = dispService.Search(
                // Predicates
                filter: d => d.Disponivel == true && d.Preco >= minPrice && d.Preco <= maxPrice, 
                orderBy: q => q.OrderBy(d => d.Preco)
                )
                .ToList();

            List<Sku> skus = new List<Sku>();
            foreach (Disponibilidade d in disponibilidades)
            {
                Sku availableSku = skuService.Search(
                    filter: s => s.Id == d.SkuId,
                    orderBy: null).FirstOrDefault();
                if (!skus.Exists(s => s.Id == availableSku.Id))
                {
                    Sku expandedSku = ExpandedSku(availableSku);
                    skus.Add(expandedSku);
                }
            }
            
            return skus;
        }
        #endregion Skus

        [HttpPost]
        [Route("processOrder")]
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

        // Generic call to API. Currently, there is only returns the XHR response from epicom's API
        [HttpGet]
        [Route("callEpicom")]
        public IHttpActionResult CallEpicom()
        {
            WebRequest webRequest = WebRequest.Create("https://sandboxmhubapi.epicom.com.br/v1");

            string username = "897F8D21A9F5A";
            string password = "Ip15q6u7X15EP22GS36XoNLrX2Jz0vqq";

            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));

            webRequest.Headers.Add("Authorization", "Basic " + credentials);
            WebResponse webResp = webRequest.GetResponse();

            return Ok(webResp);
        }

        // In OData, this is similar to the ?$expand=[prop1,prop2,...] call
        private Sku ExpandedSku(Sku sku)
        {
            if (sku == null)
            {
                return null;
            }
            Sku expandedSku = sku;

            // Disponibilidades
            List<Disponibilidade> disponibilidades = dispService.Search(d => d.SkuId == sku.Id).ToList();
            expandedSku.Disponibilidades = disponibilidades;

            // Imagens
            List<Imagem> imagens = imagemService.Search(i => i.SkuId == sku.Id).ToList();
            expandedSku.Imagens = imagens;

            // Grupos
            List<Grupo> grupos = grupoService.Search(g => g.SkuId == sku.Id).ToList();
            List<Grupo> completeGrupos = new List<Grupo>();
            foreach (Grupo g in grupos)
            {
                List<AtributoDoGrupo> atributos = atributoDoGrupoService.Search(a => a.GrupoId == g.Id).ToList();
                Grupo completeGrupo = g;
                completeGrupo.Atributos = atributos;
                completeGrupos.Add(completeGrupo);
            }
            expandedSku.Grupos = completeGrupos;

            // Dimensoes
            Dimensoes dimensoes = dimensoesService.Get(sku.Id);
            expandedSku.Dimensoes = dimensoes;

            // Marketplace
            List<SkuMarketplaceGetResponse> marketplaces = marketplaceService.Search(m => m.SkuId == sku.Id).ToList();
            expandedSku.Marketplaces = marketplaces;

            return expandedSku;
        }
    }
}