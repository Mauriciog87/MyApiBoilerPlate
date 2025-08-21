# MyApiBoilerPlate

## ⚠️ **DISCLAIMER - PROJECT UNDER DEVELOPMENT** ⚠️

**This project is still under development and should not be used in production environments.**

### 🚨 Important Warnings:

- **🔧 Under Development**: This boilerplate is in active development and may undergo significant changes without prior notice.
- **📚 External Dependencies**: The project uses multiple third-party libraries that may change or be updated.
- **🧪 Missing Tests**: Currently lacks a complete suite of unit tests, integration tests, or end-to-end tests.

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
- **Minimal APIs** ready architecture, now using Controllers.
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
- **Dapper** - High-performance micro ORM for data access

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

### 🚧 Current Project Status

- ✅ Basic Clean Architecture structure
- ✅ CQRS implementation with Mediator
- ✅ Validation with FluentValidation
- ✅ Global exception handling
- ✅ Structured logging with Serilog
- ❌ **Unit tests**
- ❌ **Integration tests**
- ❌ **Complete documentation**
- ❌ **CI/CD configuration**

### 🔮 Possible Future Changes

- New libraries may be added based on project needs
- Structure may be modified to improve architecture
- Additional patterns will be implemented as the project evolves

### 📝 Use at Your Own Risk

This boilerplate is provided "as is" without warranties of any kind. Use of this code in any project is at your own risk and responsibility.

*Last updated: August 2025*

**Built with ❤️ using .NET 9**
