namespace FlightAwareJobApp
{
    public class JobApplication
    {
        public JobApplication(string name, string email, string website)
        {
            this.name = name;
            this.email = email;
            this.website = website;
        }

        public string name { get; set; }
        public string email { get; set; }
        public string website { get; set; }
    }
}
