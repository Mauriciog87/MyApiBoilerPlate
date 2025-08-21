# MyApiBoilerPlate

A modern **.NET 9 Web API** boilerplate implementing **Clean Architecture** with CQRS, advanced error handling, and comprehensive documentation tools.

[![.NET 9](https://img.shields.io/badge/.NET-9-blue.svg)](https://dotnet.microsoft.com/)
[![C# 13](https://img.shields.io/badge/C%23-13.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 🏗️ Architecture

This solution follows **Clean Architecture** principles with clear separation of concerns across 4 layers:
├── MyApiBoilerPlate.API          # 🎯 Presentation Layer
├── MyApiBoilerPlate.Application  # 💼 Business Logic Layer
├── MyApiBoilerPlate.Domain       # 🏛️ Core Domain Layer
└── MyApiBoilerPlate.Infrastructure # 🔧 Data Access Layer

## ✨ Features

### 🚀 **Modern .NET Stack**
- **.NET 9** with **C# 13.0**
- **Minimal APIs** ready architecture
- **Nullable reference types** enabled
- **ImplicitUsings** for cleaner code

### 🏛️ **Clean Architecture & Patterns**
- **CQRS** implementation with Mediator pattern
- **ErrorOr** functional error handling
- **Pipeline Behaviors** for cross-cutting concerns
- **Repository pattern** ready infrastructure

### 🔧 **Key Libraries**
- **Mediator** - CQRS and request/response handling
- **FluentValidation** - Declarative input validation
- **Mapster** - High-performance object mapping
- **Serilog** - Structured logging
- **ErrorOr** - Railway-oriented programming

### 📚 **API Documentation**
- **OpenAPI 3.0** specification
- **Swagger UI** - Interactive API explorer
- **ReDoc** - Beautiful API documentation
- **Scalar** - Modern API reference

### 🛡️ **Exception Handling**
- **Global exception handling** with Problem Details RFC
- **Validation exception handling** with detailed field errors
- **Structured error responses** across all endpoints

## 🚀 Quick Start

### Prerequisites
- **.NET 9 SDK** or later
- **Visual Studio 2022** or **VS Code**

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **Clean Architecture** by Robert C. Martin
- **.NET Community** for amazing libraries
- **ErrorOr** by Amichai Mantinband
- **Mediator** by Martin Costello

## 🔗 Links

- [.NET 9 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Clean Architecture Guide](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- [CQRS Pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)

---

**Built with ❤️ using .NET 9**
