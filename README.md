# School Project

## Application Usage

### Installation
1. Clone the project.
2. Open it in Visual Studio 2022.
3. Build the project.

### Startup
- Start the server application by running `dotnet run` within the project directory.
- Or start the server application via Visual Studio:
  - Configure startup projects:
    - Select: Single startup project
    - Select: Server

### Testing
Both Database Integration Tests and E2E tests using playwright can be found in the  `./tests` directory.

Follow these steps to run them:
1. Start the server via the terminal.
2. Navigate to the test directory.
3. Run `dotnet test`.

## Login
The application uses JWT Bearer tokens.

You can log in as one of the following users:

#### Login

There are **2 types of users** to log in as.


| Customer                   | Admin                        | 
| -------------------------- | ---------------------------- | 
| billyBillson1997@gmail.com | darjow@hotmail.com |
| Customer.1                 | Admin.1                      |



## In Case of Errors:
1. Build the project.
2. Clean the solution.
3. Check if the Nugget packages are imported/installed correctly.

## Functionalities
- **as a customer**
  - List Projects
  - List VMs
  - Request a Virtual Machine
  - Details of a Virtual Machine
  - Customer details 
  - Modify Customer
  
- **as an administrator**
  - Query availabilities by date
    - using graphs
  - Query availabilities per day for the next 3 months
    - using graphs
  - List of all virtual machines used within a server
    - With additional graphs per VM showing how much data has been used since the start of virtualization
  - List of physical servers with current available hardware
  - List of customers
  - List of administrators
  - Details of physical server
  - Reporting
  - All functionalities within customer except requesting a VM
