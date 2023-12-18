using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PresentationWebApp.Models.ViewModels;
using System.Xml.Linq;

namespace PresentationWebApp.Controllers
{
    public class UIGeneratedController : Controller
    {


        [HttpGet] //loading the page and generating dynamically the controls 
        public IActionResult SeatingPlan(//int flightNo
            )
        {
            //from the database query/get flightinfo in particular max rows & max cols

            SeatingPlanViewModel model = new SeatingPlanViewModel();
            model.MaxRows = 6;
            model.MaxCols = 20;

            //an alternative way how to do it:
            //get all the booked seats for that flight info
            //loop within the list of seats got
            //form the SEatingPlanViewModel list of seats

            //get all the booked tickets for the flightNo
            //var bookedTickets;

            model.Seats = new List<Seat>();

            for (int row = 1; row <= model.MaxRows; row++)
            {
                for (int col = 1; col <= model.MaxCols; col++)
                {
                    var seatId = row + "," + col;
                    Seat mySeat = new Seat(); mySeat.Id = seatId;
                    //bool isBooked= bookedTickets.SingleOrDefault(x=>x.Row == row && x.Col == col) == null? false:true;
                    // mySeat.Booked = isBooked;

                    if (row == 5 && col == 2) mySeat.Booked = true;

                    model.Seats.Add(mySeat);
                }
            }
                return View(model);
            }

        [HttpPost]
        public IActionResult SeatingPlan(SeatingPlanViewModel data )
        {

            //proceed with booking here after the seat clicked on is identified
            SeatingPlanViewModel model = new SeatingPlanViewModel();
            int row = Convert.ToInt16(data.SelectedSeat.Split(new char[] { ',' })[0]);
            int col = Convert.ToInt16(data.SelectedSeat.Split(new char[] { ',' })[1]);


            model.MaxRows = 6;
            model.MaxCols = 20;
            return View(model); 
        }



    }
}
