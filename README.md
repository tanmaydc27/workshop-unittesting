# workshop-unittesting

# Run the solution

Step -:1 Run the below docker command from the location where infrastructure.docker-compose.yml file is present . This will install SQL server on the docker environment.
  docker-compose -f infrastructure.docker-compose.yml up -d
  
Step -:2 Run "Add-Migration InitialCreate" from package manager console by selecting "EBroker.DAL" project in the Default project and Api as the startup project .
This will add migration folder in the DAL layer . The code first approach is used in DAL layer

Step -:3 Run the API project . On running default data with respect to Equity , Broker will be  inserted in database automatically. Refer postman collection folder for the request body EndPoints


# Code Coverage

Step -:1 If you already executed the solution , then delete the migration folder from the EBroker.DAL else the migration file will get included in code coverage report. And after deleting just build the whole solution .

Step -:2 Open the cmd from the path "EBroker.UnitTests" 

Step -:3 Considering report generator tool already installed on the machine . Run below command from cmd.

  This will generate the code covergae report in xml format

->   dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\

This will convert the xml report into html

->   reportgenerator -reports:".\TestResults\coverage\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

Step -:4 Refer the "index.html" file in "coveragereport" folder for detailed summary  presnt at  "EBroker.UnitTests" PATH.

Step -:5 You can refer the existing coveragereport.png file for the Coverage report present at  the root of the repository .    
