using newagenda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newagenda.Controllers
{
    public class AppointmentController : Controller
    {
        //On instencie la DB
        private agendaEntities db = new agendaEntities();

        public ActionResult GoAppointment(int? idBrokers, int? idCustomers)
        {
            {
                // Déclaration des variables suivantes avec la valeur null
                int? SelectedBroker = null;
                int? SelectedCustomer = null;
                // Si le paramètre idBroker n'est pas null, on ajoute idBroker du paramètre a la variable SelectedBroker
                if (idBrokers != null)
                {
                    SelectedBroker = idBrokers;
                }
                // Si le paramètre idCustomer n'est pas null, on ajoute idCustomer du paramètre a la variable SelectedCustomer
                if (idCustomers != null)
                {
                    SelectedCustomer = idCustomers;
                }
                // on créer une nouvelle liste avec SelectedCustomer ou SelectedBroker
                ViewBag.idCustomer = new SelectList(db.customers, "idCustomers", "concatDescCustomer", SelectedCustomer);
                ViewBag.idBroker = new SelectList(db.brokers, "idBrokers", "concatDescBroker", SelectedBroker);
                return View("AddAppointment");
            }
        }
        /// <summary>
        /// Méthode pour créer un nouveau rendez-vous
        /// </summary>
        /// <param name="appointmentToAdd"></param>
        /// <returns>Vue AddAppointment avec un nouveau objet</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAppointment(appointments appointmentToAdd)
        {
            ModelState.Clear();
            // Déclaration des variables
            int startTime = 9;
            int endTime = 19;
            // Convertir le string de l'heure de l'utilisateur en Int
            var dateTimeUser = Int32.Parse(appointmentToAdd.dateHour.ToString("HH"));
            // Récuperer de la ligne dans la db du courtier selectionner
            var thisBroker = db.brokers.FirstOrDefault(x => x.idBrokers == appointmentToAdd.idBrokers);
            // Vérification que idBroker ne soit pas inférieur ou égal a 0
            if (appointmentToAdd.idBrokers <= 0)
            {
                ModelState.AddModelError("idBroker", "Veuillez selectionner un courtier");
            }
            // Vérification que idCustomer ne soit pas inférieur ou égal a 0
            if (appointmentToAdd.idCustomers <= 0)
            {
                ModelState.AddModelError("idCustomer", "Veuillez selectionner un client");
            }
            // Ajout +1 heure par rapport a la date initiale
            DateTime dateAppointmentMore1Hour = appointmentToAdd.dateHour.AddHours(1);
            // Récuperation par rapport a la date saisie par utilisateur si le courtier posséde déjà un rendez-vous
            var dateListingByBroker = db.appointments.Where(x => x.idBrokers== appointmentToAdd.idBrokers)
                .Where(x => x.dateHour >= appointmentToAdd.dateHour & x.dateHour <= dateAppointmentMore1Hour).SingleOrDefault();
            // Si dateListingByBroker n'est pas égal null alors le courtier n'est pas disponnible
            if (dateListingByBroker != null)
            {
                ModelState.AddModelError(string.Empty, "Le courtier " + thisBroker.lastname + " " + thisBroker.firstname + " n'est pas disponible le " + appointmentToAdd.dateHour.ToString("dd/MM/yyyy") + " de " + dateListingByBroker.dateHour.ToString("HH:mm") + " jusqu'a " + dateListingByBroker.dateHour.AddHours(1).ToString("HH:mm"));
            }
            // Vérification de la plage horaire
            if (startTime > dateTimeUser)
            {
                ModelState.AddModelError(string.Empty, "Le rendez-vous doit débuter 9H");
            }
            else if (endTime < dateTimeUser)
            {
                ModelState.AddModelError(string.Empty, "Le rendez-vous doit finir 19H");
            }
            // Vérification de la date
            DateTime dateAppointementUser = appointmentToAdd.dateHour;
            DateTime dateToday = DateTime.Today;
            int daysDiff = ((TimeSpan)(dateToday - dateAppointementUser)).Days;
            if (daysDiff >= 0)
            {
                ModelState.AddModelError(string.Empty, "La date du rendez-vous doit être supérieur à " + dateToday.ToString("dd/MM/yyyy"));
            }
            // Si le formulaire est valide
            if (ModelState.IsValid)
            {
                // Add : Inséretion de l'objet newBroker dans la base de donnée dans la table Brokers
                db.appointments.Add(appointmentToAdd);
                db.SaveChanges();
                // Création d'un message success si le requete est execute
                ViewBag.SuccessMessage = "Le rendez-vous " + appointmentToAdd.dateHour.ToString("dd/MM/yyyy HH:mm") + " du courtier " + thisBroker.lastname + " " + thisBroker.firstname + " a été ajouté";
                //ViewBag.idCustomer = new SelectList(db.customers, "idCustomer", "concatDescCustomer", ViewBag.SelectedCustomer);
                //ViewBag.idBroker = new SelectList(db.brokers, "idBroker", "concatDescBroker", ViewBag.SelectedBroker);
                //return View("AddAppointment", new appointments());
            }
            ViewBag.idCustomer = new SelectList(db.customers, "idCustomers", "concatDescCustomer");
            ViewBag.idBroker = new SelectList(db.brokers, "idBrokers", "concatDescBroker");
            // Retourne la vue AddBroker avec un nouveau objet broker
            return View("AddAppointment", appointmentToAdd);
        }
    }
}

