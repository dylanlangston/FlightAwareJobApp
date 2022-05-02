FlightAwareJobApp.HttpClientWithLogging client = new();

var flightAwareClient = new FlightAwareJobApp.FlightAwareRestClient(client);

var jobApp = new FlightAwareJobApp.JobApplication(
    "Dylan Langston",
    "mail@dylanlangston.com",
    "https://www.dylanlangston.com/"
    );

var response = flightAwareClient.Send("{Application URL}", "{Appliation Title}", jobApp);