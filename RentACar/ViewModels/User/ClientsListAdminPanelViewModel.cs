namespace RentACar.ViewModels.User
{
    public class ClientsListAdminPanelViewModel
    {
       public List<ClientAdminPanelViewModel> Clients { get; set; }
        public string SearchQuery { get; set; } // Holds the search query
        public int UsersPerPage { get; set; }  // Number of users per page
        public int CurrentPage { get; set; }   // Current page number
        public int TotalPages { get; set; }    // Total number of pages
    }
}
