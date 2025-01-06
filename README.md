<div align="center">

  <h1> 🖥️ POS System - Milestone 3</h1>

  *A comprehensive point-of-sale application built using WinUI and .NET.*

  ![WinUI](https://img.shields.io/badge/WinUI-blue?style=for-the-badge)
  ![.NET](https://img.shields.io/badge/.NET-blue?style=for-the-badge)
  ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-blue?style=for-the-badge)

  <img width=600 src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733933618/POSSystem/Milestone-02/5-Product.png">

  **Link to Repository**: [GitHub - POS System](https://github.com/eNKay2408/POS-System)

  **Link to Video Demo**: [Google Drive - Video Demo](https://drive.google.com/file/d/1f98XVnMRLQuXRAQuYxd_2PwNM373hLGL/view?usp=sharing)

</div>

## Table of Contents 📘
1. [Team Information](#team-information-)
2. [UI/UX Design](#uiux-design-)
3. [Design Patterns / Architecture](#design-patterns--architecture-)
4. [Key Features](#key-features-)
5. [Advanced Topics](#advanced-topics-)
6. [Teamwork / Git Flow](#teamwork--git-flow-)
7. [Quality Assurance](#quality-assurance-)

## Team Information 🤝

### Number of Members  
- **Total Members**: 2

### Member Information  
| **No.** | **Full Name**        | **Student ID** |
| ------- | -------------------- | -------------- |
| 1       | Ma Cat Huynh         | 22120144       |
| 2       | Nguyen Phan Duc Khai | 22120149       |

### Teamwork Progress Tracking

#### Milestone Progress and Completion Rate  
| **Milestone** | **Start Date** | **End Date** | **Planned Tasks** | **Completed Tasks** | **Completion Rate** | **Remarks**                                                               |
| ------------- | -------------- | ------------ | ----------------- | ------------------- | ------------------- | ------------------------------------------------------------------------- |
| Milestone 1   | 22/10/2024     | 06/11/2024   | 8                 | 8                   | 100%                | All tasks completed on time with minor revisions for CRUD operations.     |
| Milestone 2   | 07/11/2024     | 11/12/2024   | 6                 | 6                   | 100%                | Improve Google and Stripe integration, implement theme switching feature. |
| Milestone 3   | 12/12/2024     | 06/01/2025   | 5                 | 5                   | 100%                | Integration testing highlighted minor adjustments in payment processing.  |

### Task Allocation and Work Hours ⏱️

#### Milestone 1 (22/10/2024 -> 06/11/2024) 
| **No.** | **Full Name**        | **Assigned Tasks**               | **Work Hours** | **Deadline** | Expected Outcome                                                                          |
| ------- | -------------------- | -------------------------------- | -------------- | ------------ | ----------------------------------------------------------------------------------------- |
| 1       | Ma Cat Huynh         | Database and Architecture Design | 1              | 25/10        | Complete database and architecture design, ensuring scalability and good maintainability. |
| 2       | Ma Cat Huynh         | CRUD Category & Brand            | 1.5            | 27/10        | Implement CRUD operations for categories and brands.                                      |
| 3       | Nguyen Phan Duc Khai | User Management                  | 1.5            | 29/10        | Implement user registration and login functionality.                                      |
| 4       | Nguyen Phan Duc Khai | CRUD Product                     | 1.5            | 01/11        | Implement CRUD operations for products.                                                   |
| 5       | Nguyen Phan Duc Khai | Search, Filter, and Sort Product | 1.5            | 03/11        | Implement search, filter, and sort functionality for products.                            |
| 6       | Nguyen Phan Duc Khai | Google Authentication            | 1              | 06/11        | Implement Google authentication for secure login.                                         |
| 7       | Nguyen Phan Duc Khai | Stripe Integration               | 1              | 06/11        | Implement Stripe integration for online payments.                                         |
| 8       | Ma Cat Huynh         | Manual Testing                   | 0.5            | 06/11        | Perform manual testing to validate the application's functionality.                       |

#### Milestone 2 (07/11/2024 -> 11/12/2024) 
| **No.** | **Full Name**        | **Assigned Tasks**                   | **Work Hours** | **Deadline** | **Expected Outcome**                      |
| ------- | -------------------- | ------------------------------------ | -------------- | ------------ | ----------------------------------------- |
| 1       | Ma Cat Huynh         | Complete Google & Stripe Integration | 1              | 12/11        | Complete Google and Stripe integration.   |
| 2       | Ma Cat Huynh         | CRUD Employee                        | 1              | 18/11        | Implement CRUD operations for employees.  |
| 3       | Nguyen Phan Duc Khai | Interface Segregation                | 0.5            | 24/11        | Implement interfaces for services.        |
| 4       | Ma Cat Huynh         | Theme Switching                      | 1.5            | 29/11        | Implement theme switching feature.        |
| 5       | Ma Cat Huynh         | Database Migration and Seeding       | 1              | 05/12        | Implement database migration and seeding. |
| 6       | Nguyen Phan Duc Khai | Unit Testing                         | 1.5            | 11/12        | Write unit tests for ViewModel classes.   |

#### Milestone 3 (12/12/2024 -> 06/01/2025)
| **No.** | **Full Name**        | **Assigned Tasks**  | **Work Hours** | **Deadline** | **Expected Outcome**                      |
| ------- | -------------------- | ------------------- | -------------- | ------------ | ----------------------------------------- |
| 1       | Nguyen Phan Duc Khai | Create Invoice      | 2              | 20/12        | Implement create invoice functionality.   |
| 2       | Ma Cat Huynh         | Edit Invoice        | 1              | 25/12        | Implement edit invoice functionality.     |
| 3       | Nguyen Phan Duc Khai | Visa Payment        | 0.5            | 27/12        | Implement Visa payment integration.       |
| 4       | Ma Cat Huynh         | Print Invoice       | 0.5            | 01/01        | Implement print invoice functionality.    |
| 5       | Ma Cat Huynh         | Integration Testing | 1              | 06/01        | Write integration tests for repositories. |

### Summary
- **Milestone 1**: 9.5 hours
  - **Ma Cat Huynh**: 3 hours
  - **Nguyen Phan Duc Khai**: 6.5 hours
- **Milestone 2**: 6.5 hours
  - **Ma Cat Huynh**: 4.5 hours
  - **Nguyen Phan Duc Khai**: 2 hours
- **Milestone 3**: 5 hours
  - **Ma Cat Huynh**: 2.5 hours
  - **Nguyen Phan Duc Khai**: 2.5 hours
- **Total**: 21 hours
  - **Ma Cat Huynh**: 10 hours
  - **Nguyen Phan Duc Khai**: 11 hours
- **Average**: 10.5 hours per member

## UI/UX Design 🎨
The UI/UX design of the POS System is inspired by the modern design of Windows 11. The application is built using WinUI, which is a native user interface (UI) framework for Windows Desktop applications. The application is designed to be user-friendly and intuitive, with a focus on simplicity and ease of use.

### Highlights:
- Modern design inspired by Windows 11
- Intuitive and user-friendly interface
- Easy navigation and accessibility
- Consistent design across all screens

### Theme Switching:
- Implemented a theme switching feature allowing users to toggle between light and dark modes.
- The light theme provides a bright and clean interface, ideal for well-lit environments.
- The dark theme offers a darker interface, reducing eye strain in low-light conditions.
- Users can switch themes from the settings menu, and the selected theme is saved for future sessions.

#### Light Theme:
<img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733933618/POSSystem/Milestone-02/1-Login.png"> <img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733933618/POSSystem/Milestone-02/2-Register.png">

<img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733933619/POSSystem/Milestone-02/3-Category.png"> <img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733933618/POSSystem/Milestone-02/4-Brand.png">

#### Dark Theme:
<img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733933618/POSSystem/Milestone-02/5-Product.png"> <img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733933618/POSSystem/Milestone-02/6-AddProduct.png">

<img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733933618/POSSystem/Milestone-02/7-Employee.png"> <img width="49%" src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733933618/POSSystem/Milestone-02/8-UpdateEmployee.png">

## Design Patterns / Architecture 🧩
This project applies widely adopted software design patterns to ensure maintainability, scalability, and separation of concerns.

#### Design Patterns:
- MVVM (Model-View-ViewModel): Separation of concerns between the UI, business logic, and data.
- Repository Pattern: Abstraction of data access logic from the rest of the application.

#### Architecture:
- The application is structured using the MVVM design pattern, with separate folders for Models, Views, and ViewModels.
- Repositories and services are defined using interfaces to promote consistency and facilitate unit testing.

## Key Features 🗝️

### Milestone 1

#### User Management:
- Users can register and log in to the application using email and password.
- Password strength, email validation, and remember me features are implemented.

#### CRUD Operations:
- Users can perform CRUD (Create, Read, Update, Delete) operations on products, categories, and brands.
- Products, categories, and brands are displayed in a list format with details information. 
- Products can be added with details such as name, price, stock, category, and brand.

#### Search, Filter, and Sort:
- Users can search for products by name using the search bar.
- Products can be filtered by category or by max price to narrow down the search results.
- Products can be sorted by price in ascending or descending order.

### Milestone 2

#### CRUD Employees:
- Users can view a list of employees and their details, including name and email.
- Employees can be added to the system by logging in with Google or creating an account.
- Employees can be updated and deleted from the system as needed.

#### Interface Segregation:
- Interfaces are used to define contracts for services and repositories to promote testability.
- Services and repositories implement these interfaces to provide concrete implementations.

#### Unit Testing:
- ViewModel and Converter classes are tested using the MSTest framework to ensure correctness.
- Mock objects are used to simulate data access and external dependencies for testing.

### Milestone 3

#### Create Invoice
- Allow users to create an invoice by selecting products and quantities from the list.
- Users can view the total price of the invoice and name of the employee who created it

#### Edit Invoice
- Users can edit the invoice by adding or removing products and updating the quantities.
- The total price is updated dynamically based on the changes made to the invoice.

## Advanced Topics 🚀

### Milestone 1

#### Google Authentication:
- Users can log in to the application using their Google account with OAuth 2.0 authentication for secure access.
- User information is saved to the database upon successful authentication, ensuring a personalized experience.

#### Stripe Integration:
- The application supports online VISA payments using the Stripe payment gateway.
- Users can enter their card details and complete the payment process securely and efficiently.

### Milestone 2

#### Complete Google & Stripe Integration:
- Google authentication and Stripe integration are fully implemented and tested to ensure a seamless user experience.

#### Database Migration and Seeding:
- The application supports database migration to update the schema with new changes.
- A seeding script is provided to populate the database with initial data for testing.

#### Theme Switching:
- Users can switch between light and dark themes to customize the application's appearance.
- The selected theme is saved in the user's settings and persists across sessions.

### Milestone 3

#### Visa Payment Integration with Stripe:
- Migrate the payment method from Product to Invoice to support multiple products in a single transaction.
- Users can add view the total price of the invoice and pay using their VISA card.

#### Integration Testing with TestContainers:
- Use TestContainers to create a PostgreSQL container for integration testing.
- Write integration tests for repositories to verify data access and persistence.

#### Print Invoice:
- Allow users to print the invoice in a PDF format for record-keeping and documentation.

## Teamwork / Git Flow 🤝
The project follows a collaborative workflow using Git and GitHub to manage code changes and contributions.

#### Teamwork:
- The team collaborates on feature development, bug fixes, and code reviews.
- Each team member is assigned tasks and works collaboratively to achieve project milestones.
- Regular meetings are held to discuss progress, challenges, and next steps.

#### Git Flow:
- Regular commit messages follow a consistent format and provide context for changes.
- Feature branches are created for new features and merged into the main branch after code review.
- Issues are tracked on GitHub to manage tasks, bugs, and feature requests.

#### Screenshots:

- Pull Request

  <img width=500 src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733935580/POSSystem/Milestone-02/12-PullRequest.png">

  <img width=500 src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733935580/POSSystem/Milestone-02/9-PullRequest.png">
  
- Code Review

  <img width=500 src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733935579/POSSystem/Milestone-02/10-CodeReview.png">

- Issue
  
  <img width=500 src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1733935580/POSSystem/Milestone-02/11-Issue.png">


## Quality Assurance 🛡️
The project is tested using a combination of manual and automated testing to ensure the application's quality and reliability.

#### Manual Testing:
- Manual testing is performed by team members to verify the application's functionality.
- Test cases are created to validate user interactions, data input, and edge cases.

#### Unit Tests:
- Unit tests are written using the MSTest framework to test individual components.
- Using the Moq library, mock objects are created to simulate data access and external dependencies.
- ViewModel tests are written to verify business logic and data manipulation.
- Converter tests are written to validate data conversion and formatting.

#### Integration Testing:
- Integration tests are written using the TestContainers library to test data access and persistence.
- Repositories are tested to ensure that data is correctly retrieved, updated, and deleted from the database.
- **Note**: You must have Docker installed and running to execute the integration tests.

#### Total: 146 Tests

<img width=600 src="https://res.cloudinary.com/dvzhmi7a9/image/upload/v1736189584/POSSystem/Milestone-03/1-Tests.png">