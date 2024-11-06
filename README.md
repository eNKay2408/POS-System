<div align="center">

  <h1> 🖥️ POS System - Milestone 1</h1>

  *A comprehensive point-of-sale application built using WinUI and .NET.*

  ![WinUI](https://img.shields.io/badge/WinUI-blue?style=for-the-badge)
  ![.NET](https://img.shields.io/badge/.NET-blue?style=for-the-badge)
  ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-blue?style=for-the-badge)

  <img width=500 src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730905586/POSSystem/3-Products.png">

</div>

## Table of Contents 📘
1. [UI/UX Design](#uiux-design-)
2. [Design Patterns / Architecture](#design-patterns--architecture-)
3. [Key Features](#key-features-)
4. [Advanced Topics](#advanced-topics-)
5. [Teamwork / Git Flow](#teamwork--git-flow-)
6. [Quality Assurance](#quality-assurance-)

## UI/UX Design 🎨
The UI/UX design of the POS System is inspired by the modern design of Windows 11. The application is built using WinUI, which is a native user interface (UI) framework for Windows Desktop applications. The application is designed to be user-friendly and intuitive, with a focus on simplicity and ease of use.

Highlights:
- Modern design inspired by Windows 11
- Intuitive and user-friendly interface
- Easy navigation and accessibility
- Consistent design across all screens

Here are some screenshots of the project showcasing its features and design:

<img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730905585/POSSystem/1-Login.png"> <img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730905584/POSSystem/2-Register.png">

<img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730905586/POSSystem/3-Products.png"> <img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730905585/POSSystem/4-AddProduct.png">

<img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730906633/POSSystem/9-Google.png"> <img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730905585/POSSystem/10-Stripe.png">

<img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730905585/POSSystem/5-Categories.png"> <img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730905585/POSSystem/6-AddCategory.png">

<img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730905585/POSSystem/7-Brands.png"> <img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730905585/POSSystem/8-AddBrand.png">

## Design Patterns / Architecture 🧩
This project applies widely adopted software design patterns to ensure maintainability, scalability, and separation of concerns.

#### Design Patterns:
- MVVM (Model-View-ViewModel): Separation of concerns between the UI, business logic, and data.
- Repository Pattern: Abstraction of data access logic from the rest of the application.

#### Architecture:
- The application is structured using the MVVM design pattern, with separate folders for Models, Views, and ViewModels.
- The Views are responsible for the UI elements and layout of the application.
- The Models represent the data entities used in the application, such as employees, products, categories, and brands.
- The ViewModels interact with the repositories to fetch and update data from the database.

## Key Features 🗝️

#### User Management:
- Users can register and log in to the application using email and password.
- Password strength is checked during registration to ensure security.
- Email validation is performed to verify the format of the email address.
- Users can choose to save their login credentials for future sessions using the "Remember Me" feature.
- Users can log out of the application to securely end their session.

#### CRUD Operations:
- Users can perform CRUD (Create, Read, Update, Delete) operations on products, categories, and brands.
- Products, categories, and brands are displayed in a list format with details information. 
- Products can be added with details such as name, price, stock, category, and brand.
- Categories and brands can be added with a name.
- Products, categories, and brands can be updated with new information.
- Products, categories, and brands can be deleted from the database.

#### Search, Filter, and Sort:
- Users can search for products by name using the search bar.
- Products can be filtered by category to narrow down the search results.
- Products can be filtered by price to display products with a maximum price.
- Products can be sorted by price in ascending or descending order.

## Advanced Topics 🚀
The POS system integrates advanced features to improve functionality and user experience.

#### Google Authentication:
- Google authentication provides a secure and convenient way for users to log in to the application.
- The application uses OAuth 2.0 to authenticate users with Google and obtain user information.

#### Stripe Integration:
- The application supports online payments using the Stripe payment gateway.
- Users can make payments for their orders using VISA cards securely through Stripe.

## Teamwork / Git Flow 🤝
The project follows a collaborative workflow using Git and GitHub to manage code changes and contributions.

#### Teamwork:
- The team collaborates on feature development, bug fixes, and code reviews.
- Each team member is assigned tasks and works collaboratively to achieve project milestones.
- Regular meetings are held to discuss progress, challenges, and next steps.

#### Git Flow:
- Source code management with Git and GitHub for version control.
- Regular commit messages follow a consistent format and provide context for changes.
- Feature branches are created for new features and merged into the main branch after code review.

<div align="center">

  *Here is an example of the Git flow used in the project:*
  
  <img width=500 src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730909718/POSSystem/11-PullRequest.png">
  
  <img width=500 src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1730909717/POSSystem/12-Commits.png">
</div>


## Quality Assurance 🛡️
Currently, quality assurance for the POS system project has focused on manual testing through UI navigation. Formalized testing processes, such as unit and integration tests, are planned for future milestones.