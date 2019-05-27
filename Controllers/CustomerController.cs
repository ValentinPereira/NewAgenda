using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using newagenda.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using PagedList;

namespace newagenda.Controllers
{
    public class CustomerController : Controller
    {
        private agendaEntities db = new agendaEntities();
        // GET: Customer
        public ActionResult GoCustomer(int id = 0) // Methode qui permet l'affichage de la vue
        {
            customers customer = new customers();
            return View("AddCustomer"); //Affiche la vue
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GoCustomer(customers customer) // création de la méthode
        {
            db.customers.Add(customer);
            db.SaveChanges(); //Enregistre les données  
            ModelState.Clear();
            ViewBag.MessageSuccess = "Succés"; // Envoi un message succés 
            return View("AddCustomer", new customers()); // Affiche une vue
        }

        public ActionResult ListCustomers()
        {
            agendaEntities db = new agendaEntities();
            string query =
                "SELECT [idCustomers], [lastname], [firstname], [mail], [phoneNumber], [budget]"
                + "FROM [dbo].[customers]"
                + "ORDER BY [lastname];";
            IEnumerable<newagenda.Models.customers> data = db.Database.SqlQuery<newagenda.Models.customers>(query);// Enumére une à une les données 
            return View("ListCustomers", data.ToList()); // Affiche la vue 
            // A la place de ienumerable et tolist ==> ViewData.Model = db.customers.SqlQuery(query);
        }
        public ActionResult ProfilCustomer(int id = 0)
        {
            return View("ProfilCustomer", db.customers.Find(id)); //renvoi la valeur du premier élément trouvé.
        }

        public ActionResult EditProfilCustomer(int id)
        {
            return View("EditCustomer", db.customers.Find(id));
        }
        [HttpPost]
        public ActionResult ListCustomers(int? page)
        {
            //On stoke,la liste des clients trier par nom avec sqlQuery. On initialize les variable pour la pagination, puisnon affiche la vue
            string queryListCustomer =
                "SELECT [id], [lastname], [firstname], [mail], [phonenumber], [budget]"
                + "FROM [dbo].[customers] "
                + "ORDER BY [lastName];";
            var customerList = db.customers.SqlQuery(queryListCustomer);
            int elementByPage = 7;
            int pageNumber;
            if (page <= 0)
            {
                page = 1;
            }
            pageNumber = (page ?? 1);
            return View("ListCustomers", customerList.ToPagedList(pageNumber, elementByPage));
        }
        /// <summary>
        /// ListCustomer en POST
        /// Permet d'afficher la liste des client avec une recherche et une pagination
        /// </summary>
        /// <param name="page">Numéro de la page de la pagination</param>
        /// <param name="searchCustomer">Stock la recherche</param>
        /// <returns>Vue ListCustomer en fonction de la recherche et avec une pagination</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListCustomers(int? page, string searchCustomer = "")
        {
            //On stoke,la liste des clients rechercher avec sqlQuery, On initialize les variable pour la pagination, puisnon affiche la vue
            string queryListCustomer =
                "SELECT [id], [lastname], [firstname], [mail], [phonenumber], [budget]"
                + "FROM [dbo].[customers] "
                + "WHERE [lastName] LIKE '%" + searchCustomer + "%'"
                + "OR [firstName] LIKE '%" + searchCustomer + "%'"
                + "ORDER BY [firstName];";
            var customerList = db.customers.SqlQuery(queryListCustomer);
            int pageSize = 7;
            if (page <= 0)
            {
                page = 1;
            }
            int pageNumber = (page ?? 1);
            return View("ListCustomers", customerList.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult EditProfilCustomer(customers EditToCustomer)
        {
            try // relève une exception
            {
                db.Entry(EditToCustomer).State = EntityState.Modified;
                db.SaveChanges(); // enregistre 
                return RedirectToAction("ProfilCustomer/" + EditToCustomer.idCustomers); // renvoi au profil du client plus l'edit de son id
            }
            catch // fonctionne avec try relève une exception
            {
                return View(); //
            }
        }
        public ActionResult DeleteCustomer(int? id) //Création de la méthode DeleteCustomer
        {
            var customerToFind = db.customers.Find(id); // appelle la variable customerToFind et renvoi à l'id 
            if (customerToFind == null || id == null)
            {
                return HttpNotFound(); // retourne sur une page d'erreur
            }

            return View("DeleteCustomer", customerToFind); //Affiche la vue DeleteCustomer de notre db qui à l'id du parametre
        }
        [HttpPost] //Methode Post 
        public ActionResult DeleteCustomer(customers DeleteToCustomer) // objet DeleteToCustomer
        {
            try // Si les données sont valides 
            {
                customers CustomerToDelete = db.customers.Find(DeleteToCustomer.idCustomers);
                db.customers.Remove(CustomerToDelete); // Supprime
                db.SaveChanges(); // Enregistre
                return RedirectToAction("ListCustomers"); // Renvoi à la liste des clients
            }
            catch
            {
                return View("DeleteCustomer"); //Retourne la vue DeleteCustomer
            }
        }
    }
}

//customer customerToDisplay = db.customers.Find(DeleteToCustomer);
//if (customerToDisplay == null)
//{
//    return View("Erreur");
//}
//return View("deleteProfilCustomer", customerToDisplay);