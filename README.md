<a id="readme-top"></a>
<!--
*** Thanks for checking out the ProductCatalog. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Unlicense License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">

  <h3 align="center">Product Catalog</h3>

  <p align="center">
    Quick links
    <br />
    <a href="https://github.com/mlojko/ProductCatalog"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/mlojko/ProductCatalog">View Demo</a>
    &middot;
    <a href="https://github.com/mlojko/ProductCatalog/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    &middot;
    <a href="https://github.com/mlojko/ProductCatalog/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

A little project I was assigned as part of an interview

Backend-end:
* API accessing a DB
* The API can retrieve list of products, retrieve a product by ID, create, edit and delete a product
* The API is secured by JWT Role-Based Authentication

Front-end:
* Simple UI letting you to perform the same operations as BE.
* UI has log in screen, and won't show anything unless you authenticate
* UI will displays error when adding, editting or deleting products as a non-administrator

Containerization
* BE, FE and DB run in docker containers

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With

This section should list any major frameworks/libraries used to bootstrap your project. Leave any add-ons/plugins for the acknowledgements section. Here are a few examples.

* [![.NET 9][.NET-9]][NET-url]
* [![EF Core][ef-Core]][ef-url]
* [![MSSQL][MSSQL]][sql-url]
* [![Docker][Docker]][docker-url]
* [![Copilot][Copilot]][copilot-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

Install prerequisites and follow the installation instructions

### Prerequisites

To get started on Windows, you will need the following:
* .NET 9 SDK [https://dotnet.microsoft.com/en-us/download/dotnet/9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* Visual Studio 2022 [https://visualstudio.microsoft.com](https://visualstudio.microsoft.com)
* Git with Git-Bash [https://git-scm.com/downloads](https://git-scm.com/downloads)

Optional tools:
* DBeaver to inspect your SQL DB [https://dbeaver.io/download/](https://dbeaver.io/download/)
* Postman to perform HTTP calls against the API outside of Swagger [https://www.postman.com/downloads/](https://www.postman.com/downloads/)

### Installation

_Below is an example of how you can instruct your audience on installing and setting up your app. This template doesn't rely on any external dependencies or services._

1. Start by creating a working directory and cd into it
2. Clone the repo
   ```sh
   git clone git@github.com:mlojko/ProductCatalog.git
   ```
3. CD into ProductCatalog directory
   ```sh
   cd ProductCatalog
   ```
4. Run Docker compose up command
   ```sh
   docker-compose up
   ```
5. Wait for the process to finish and visit [http://localhost:5000/](http://localhost:5000/)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

UI
* The UI is available on port 5000 [http://localhost:5000/](http://localhost:5000/).
* You will be asked to log in, use either 'admin:admin' or 'viewer:viewer' username:password combination.
* After you login you will land on product listing page, with each product linking to a product detail page.
* The 'admin' user can view, add, edit and delete products, 'viewer' can only view.
* Viewer will receive error messages when trying to add, edit or delete products.

API
* The API is available on port 8080, visit Swagger link to view the endpoints [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html).
* The 'GetProduct', 'GetProducts' and 'Login' endpoints do not require authentication.
* You will need to authenticate as an 'admin' if you want to succesfully AddProduct, UpdateProduct or DeleteProduct.
* To authenticate, make a call to the 'Login' endpoint with 'admin:admin' username:password combination.

DB
* If you want to inspect the database with DBeaver, use 'localhost' server name and port 1433
* The DB password can be found API's [appsettings.json](https://github.com/mlojko/ProductCatalog/blob/master/Api/appsettings.json)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ROADMAP -->
## Roadmap

- [ ] Add health check endpoint
- [ ] Add unit tests
- [ ] Add pagination for product listing
- [ ] Add API rate limiting
- [ ] Add Redis for caching

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Top contributors:

<a href="https://github.com/mlojko/ProductCatalog/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=mlojko/ProductCatalog" alt="contrib.rocks image" />
</a>

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Martin Lojkovic - [LinkedIn](https://www.linkedin.com/in/martin-lojkovic-5056a259/)

Project Link: [https://github.com/mlojko/ProductCatalog](https://github.com/mlojko/ProductCatalog)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

Use this space to list resources you find helpful and would like to give credit to. I've included a few of my favorites to kick things off!

* [Choose an Open Source License](https://choosealicense.com)
* [Tutorial: Code First Approach in ASP.NET Core MVC with EF](https://medium.com/c-sharp-programming/tutorial-code-first-approach-in-asp-net-core-mvc-with-ef-5baf5af696e9)
* [JWT Authentication in .NET 8: A Complete Guide for Secure and Scalable Applications](https://medium.com/@solomongetachew112/jwt-authentication-in-net-8-a-complete-guide-for-secure-and-scalable-applications-6281e5e8667c)
* [.NET 8.0 Web API JWT Authentication and Role-Based Authorization](https://dev.to/shahed1bd/net-80-web-api-jwt-authentication-and-role-based-authorization-42f1)
* [Use cookie authentication without ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-9.0)
* [API Versioning in ASP.NET Core](https://code-maze.com/aspnetcore-api-versioning/)
* [ASP.NET Core API Versioning](https://weblogs.asp.net/ricardoperes/asp-net-core-api-versioning)
* [React Icons](https://react-icons.github.io/react-icons/search)
* [Img Shields](https://shields.io)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/mlojko/ProductCatalog.svg?style=for-the-badge
[contributors-url]: https://github.com/mlojko/ProductCatalog/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/mlojko/ProductCatalog.svg?style=for-the-badge
[forks-url]: https://github.com/mlojko/ProductCatalog/network/members
[stars-shield]: https://img.shields.io/github/stars/mlojko/ProductCatalog.svg?style=for-the-badge
[stars-url]: https://github.com/mlojko/ProductCatalog/stargazers
[issues-shield]: https://img.shields.io/github/issues/mlojko/ProductCatalog.svg?style=for-the-badge
[issues-url]: https://github.com/mlojko/ProductCatalog/issues
[license-shield]: https://img.shields.io/github/license/mlojko/ProductCatalog.svg?style=for-the-badge
[license-url]: https://github.com/mlojko/ProductCatalog/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/othneildrew
[product-screenshot]: images/screenshot.png
[.NET-9]: https://img.shields.io/badge/.NET%209-0078D7?style=for-the-badge&logoColor=4FC08D
[NET-url]: https://dotnet.microsoft.com/en-us/download/dotnet/9.0
[ef-Core]: https://img.shields.io/badge/EF%20Core-0078D7?style=for-the-badge
[ef-url]: https://learn.microsoft.com/en-us/ef/core/
[MSSQL]: https://img.shields.io/badge/MSSQL-CC2927?style=for-the-badge
[sql-url]: https://www.microsoft.com/en-ca/sql-server/
[Docker]: https://img.shields.io/badge/Docker-2496ED?style=for-the-badge
[docker-url]: https://www.docker.com/
[Copilot]: https://img.shields.io/badge/GitHub%20Copilot-7E3FFF?style=for-the-badge
[copilot-url]: https://copilot.microsoft.com/
