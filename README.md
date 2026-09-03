# PetShop

PetShop is a web application built using ASP.NET Core Razor Pages and C#. The project simulates a pet adoption store where users can browse animals, add pets to a shopping cart, and complete a make-believe checkout process.

The application includes an admin section where animals can be created, edited, deleted, and marked as available or unavailable. Animal and category data is stored using a SQLite database, and session state is used to manage the shopping cart.

## Live Website

PetShop is deployed using Microsoft Azure App Service.

**Live Site:**  
https://ariel-petshop-efb7cqgjbperfbcx.centralus-01.azurewebsites.net/

## Features

- Browse animals available for adoption
- View animal details, including category and availability
- Add animals to a shopping cart
- Remove animals from the shopping cart
- Complete a simulated checkout
- Manage shopping cart data using session state
- Create, edit, and delete animals through the admin section
- Mark animals as available or unavailable
- Store animal and category data using SQLite

## Technologies Used

- C#
- ASP.NET Core
- Razor Pages
- Entity Framework Core
- SQLite
- HTML
- CSS
- Bootstrap
- JavaScript
- Microsoft Azure App Service
- GitHub Actions

## Deployment

PetShop is hosted on Microsoft Azure App Service and connected to this GitHub repository using GitHub Actions.

Changes pushed to the `main` branch are automatically built and deployed to the live application.

## About the Project

PetShop was created to practice developing a database-driven web application with ASP.NET Core and C#. The project demonstrates working with Razor Pages, CRUD operations, databases, session state, shopping cart functionality, and cloud deployment.
