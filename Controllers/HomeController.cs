using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using newagenda.Models;

namespace newagenda.Controllers
{
    public class HomeController : Controller
    {
        // Connexion a la bdd
        private agendaEntities db = new agendaEntities();
        // Méthode pour affiche la vue index avec la liste des rendez-vous
        public ActionResult Index()
        {
            // Requête  SQL
            string query = "SELECT [idAppointment],[datehour],[subject],[idCustomers],[idBrokers]"
            + "FROM [dbo].[appointments]";
            ViewData.Model = db.appointments.SqlQuery(query);
            // Retourner vers la vue index
            return View("Index");

        }
        // Méthode pour supprimer un rendez-vous
        public ActionResult DeleteAppointment(int? id, appointments delAppointment)
        {
            // Si l'id en paramètre  est null,j'affiche un message d'erreur
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Je cherche l'appointment qui est défini dans le paramètre dans la db appointment
            delAppointment = db.appointments.Where(x => x.idAppointment == id).SingleOrDefault();
            // Exécution  de la méthode remove afin de supprimer le rendez-vous
            db.appointments.Remove(delAppointment);
            //Rend persistantes toutes les mises à jour apportées à la source de données et réinitialise le suivi des modifications dans le contexte de l'objet.
            db.SaveChanges();
            //Rediriger vers une action
            return RedirectToAction("Index");
        }
    }
}