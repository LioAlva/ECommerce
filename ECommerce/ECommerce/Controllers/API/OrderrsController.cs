using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ECommerce.Models;
using ECommerce.Models.ApiModels;

namespace ECommerce.Controllers.API
{
    [RoutePrefix("api/Orderrs")]
    public class OrderrsController : ApiController
    {
        private ECommerceContext db = new ECommerceContext();

        // GET: api/Orderrs
        public IQueryable<Orderr> GetOrderss()
        {
            return db.Orderss;
        }

        // GET: api/Orderrs/5
        [ResponseType(typeof(Orderr))]
        public IHttpActionResult GetOrderr(string id)
        {
            Orderr orderr = db.Orderss.Find(id);
            if (orderr == null)
            {
                return NotFound();
            }

            return Ok(orderr);
        }

        // PUT: api/Orderrs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderr(string id, Orderr orderr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderr.Id)
            {
                return BadRequest();
            }

            db.Entry(orderr).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderrExists(id))
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

        // POST: api/Orderrs
        [ResponseType(typeof(Orderr))]
        public IHttpActionResult PostOrderr(Orderr orderr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orderss.Add(orderr);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrderrExists(orderr.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = orderr.Id }, orderr);
        }

        // DELETE: api/Orderrs/5
        [ResponseType(typeof(Orderr))]
        public IHttpActionResult DeleteOrderr(string id)
        {
            Orderr orderr = db.Orderss.Find(id);
            if (orderr == null)
            {
                return NotFound();
            }

            db.Orderss.Remove(orderr);
            db.SaveChanges();

            return Ok(orderr);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderrExists(string id)
        {
            return db.Orderss.Count(e => e.Id == id) > 0;
        }
    }
}