using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance2.Models;

namespace CarInsurance2.Controllers
{
    public class InsureeController : Controller
    {
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = master; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewQuote(string FirstName, string LastName, string EmailAddress, int CarYear, string CarMake, string CarModel,
            bool DUI, int SpeedingTickets, bool CoverageType)
        {
            Insuree insuree = new Insuree();
            var quote = 50m;
            var age = Convert.ToInt32(insuree.DateOfBirth);

            if (age < 18)
            {
                quote += 25;
            }
            else if (age >= 19 && age <= 25)
            {
                quote += 25;
            }
            else if (age > 25)
            {
                quote += 25;
            }
            else if (CarYear < 2000)
            {
                quote += 25;
            }
            else if (CarYear > 2015)
            {
                quote += 25;
            }
            else if (CarMake.ToLower() == "porsche")
            {
                quote += 25;
            }
            else if (CarMake.ToLower() == "porsche" && CarModel == "911 carrera")
            {
                quote += 25;
            }


            quote = quote + (SpeedingTickets * 10);
            if (DUI == true) { quote *= 1.25m; }
            else if (CoverageType == true) { quote *= 1.5m; }

            decimal Quote = Convert.ToDecimal(quote);

            string queryString = @"INSERT INTO Insuree (FirstName, LastName, EmailAddress, CarYear, CarMake, CarModel, DUI, SpeedingTickets, CoverageType, Quote)
                                   VALUES (@FirstName, LastName, EmailAddress,CarYear, @CarMake, @CarModel, @Dui, @SpeedingTickets, @CoverageType, @Quote)";
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("FirstName", SqlDbType.VarChar);
                command.Parameters.Add("LastName", SqlDbType.VarChar);
                command.Parameters.Add("EmailAddress", SqlDbType.VarChar);
                command.Parameters.Add("DateOfBirth", SqlDbType.Date);
                command.Parameters.Add("CarYear", SqlDbType.VarChar);
                command.Parameters.Add("CarMake", SqlDbType.VarChar);
                command.Parameters.Add("CarModel", SqlDbType.VarChar);
                command.Parameters.Add("DUI", SqlDbType.Bit);
                command.Parameters.Add("SpeedingTickets", SqlDbType.Int);
                command.Parameters.Add("CoverageType", SqlDbType.Bit);
                command.Parameters.Add("Quote", SqlDbType.Money);

                command.Parameters["FirstName"].Value = FirstName;
                command.Parameters["LastName"].Value = LastName;
                command.Parameters["EmailAddress"].Value = EmailAddress;
                command.Parameters["DateOfBirth"].Value =  age;
                command.Parameters["CarYear"].Value = CarYear;
                command.Parameters["CarMake"].Value = CarMake;
                command.Parameters["DUI"].Value = DUI;
                command.Parameters["CoverageType"].Value = CoverageType;
                command.Parameters["Quote"].Value = Quote;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            return View(insuree);
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Insurees.Add(insuree);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(insuree);
        //}

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
        
        //public ActionResult Admin()
        //{
        //    string queryString = @"SELECT Id, firstName, lastName, emailAddress, dateOfBirth, carYear, carMake, carModel, Dui, speedingTickets, CoverageType, Quote FROM Insurees";
        //    Insuree insuree = new Insuree();

        //}
    }
}
