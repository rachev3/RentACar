namespace RentACar.ViewModels.User
{
    public class ClientsListAdminPanelViewModel
    {
       public List<ClientAdminPanelViewModel> Clients { get; set; }
        public string SearchQuery { get; set; } 
        public int UsersPerPage { get; set; }  
        public int CurrentPage { get; set; }   
        public int TotalPages { get; set; }    
    }
}
