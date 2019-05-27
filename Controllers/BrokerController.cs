using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using newagenda.Models;
using System.Data.Entity;

namespace newagenda.Controllers
{
    public class BrokerController : Controller
    {
        private agendaEntities db = new agendaEntities();

        public ActionResult Gobroker(int id = 0) // Methode pour aller à la view AddBroker.
        {
            brokers broker = new brokers();
            return View("AddBroker"); //nom de la view.
        }

        [HttpPost]
        public ActionResult GoBroker(brokers Broker)
        {
            using (agendaEntities db = new agendaEntities())
            {
                db.brokers.Add(Broker);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.MessageSuccess = "successful";
            return View("AddBroker", new brokers());
        }
        //return View(db.brokers.Where(x=> x.idBroker == id).FirstOrDefault());
        public ActionResult ProfilBroker(int? id)
        {
            var profilBrokerToFind = db.brokers.Find(id);
            if (profilBrokerToFind == null || id == null)
            {
                return HttpNotFound();
            }
            return View("profilBroker", "profilBrokerToFind");
        }

        public ActionResult EditProfilBroker(int id = 0)
        {
            return View("EditBroker", db.brokers.Find(id));
        }
        [HttpPost]
        public ActionResult EditProfilBroker(brokers EditToBroker)
        {
            try
            {
                db.Entry(EditToBroker).State = EntityState.Modified; // si pas d'eerreur les changement s'effectue
                db.SaveChanges();
                return RedirectToAction("ProfilBroker/" + EditToBroker.idBrokers); //redirige une fois le changement sauvegardé vers ...
            }
            catch
            {
                return View();
            }
        }

         public ActionResult DeleteBroker(int? id) //Création de la méthode DeleteCustomer
            {
                var brokerToFind = db.brokers.Find(id); // appelle la variable customerToFind et renvoi à l'id 
                if (brokerToFind == null || id == null)
                {
                    return HttpNotFound(); // retourne sur une page d'erreur
                }

                return View("DeleteBroker", brokerToFind); //Affiche la vue DeleteCustomer de notre db qui à l'id du parametre
            }
        [HttpPost] //Methode Post 
        public ActionResult DeleteBroker(brokers DeleteToBroker) // objet DeleteToCustomer
        {
            try // Si les données sont valides 
            {
                brokers BrokerToDelete = db.brokers.Find(DeleteToBroker.idBrokers);
                db.brokers.Remove(BrokerToDelete); // Supprime
                db.SaveChanges(); // Enregistre
                return RedirectToAction("ListBrokers"); // Renvoi à la liste des clients
            }
            catch
            {
                return View("DeleteBroker"); //Retourne la vue DeleteCustomer
            }
        }
    }
}