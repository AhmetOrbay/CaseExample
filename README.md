# NeredeKal

This is a simple hotel directory application consisting of two microservices and unit tests. The project utilizes the following technologies:

- Git
- .NET 7
- RabbitMQ
- Elasticsearch (with Serilog)
- PostgreSQL

## Microservices

### Hotel Microservice

The hotel microservice handles the management of hotels, including listing, details, managers, contracts, etc. The `Hotel.Library` project contains DTOs, services, interfaces, migrations, models, and repositories related to the hotel microservice.

### Report Microservice

The report microservice is responsible for generating and managing reports. It includes features for creating reports, listing reports, and retrieving report details. The `Report.Library` project contains models, services, interfaces, and extensions related to the report microservice. When a report creation request is received, it is sent to the hotel microservice via RabbitMQ. The status of the report is initially set to "Waiting" (as an enum value called `ReportStatus`). The background worker in the hotel microservice processes the request and sends the completed report back to the report microservice through RabbitMQ. The report microservice updates the status to "Completed" and stores the report details in the database.

## Logging

The application uses Serilog with Elasticsearch for logging purposes. This allows for centralized log storage and analysis.

## Unit Testing

Unit tests have been implemented using NUnit and Moc. There are 8 test methods covering various functionalities of the application.






