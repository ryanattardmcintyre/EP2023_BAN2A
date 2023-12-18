namespace PresentationWebApp.Models.ViewModels
{

    public class Seat
    {
        public string Id { get; set; }
        public bool Booked { get; set; }
    }


    public class SeatingPlanViewModel
    {
        public int MaxRows { get; set; }
        public int MaxCols { get;set; }


        public List<Seat> Seats = new List<Seat>();

        public string SelectedSeat { get; set; }
    }
}
