# Webshop - Customer solution

This solution follows the same overall structure as the Category/Product solution.

## Customer properties
- CustomerID
- FirstName
- LastName
- Email
- Address
- City
- Country
- Points

## Supported operations
- AddCustomer
- UpdateCustomer
- DeleteCustomer
- GetAll
- GetByID
- GetByName

The repository is an in-memory repository, matching the pattern used by the existing Category/Product solution. Sample customers are included.

## Project structure
- Webshop.Model/Customer.cs
- Webshop.Model/ICustomerRepository.cs
- Webshop.Model/InMemoryCustomerRepository.cs
- Webshop.UI/ViewModel/CustomerViewModel.cs
- Webshop.UI/View/CustomerView.xaml
- Webshop.UI/View/CustomerView.xaml.cs
- Webshop/Database/03_Create_Customer_Table.sql

## Note about GetByID
The UI includes search controls for Get All, Get By Name and Get By ID.
