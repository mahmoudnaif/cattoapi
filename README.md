![API Management](https://geekflare.com/wp-content/uploads/2022/06/apimanagement.png)

# CattoAPI

CattoAPI is a robust and versatile API solution designed to facilitate the management of posts, comments, likes, and email services in your applications. Built with .NET 8.0, it offers comprehensive features for user authentication, administrative actions, and seamless integration with various services. Whether you're building a social media platform, a blogging site, or any application requiring user interactions, CattoAPI provides the necessary endpoints and tools to get started quickly and efficiently.


## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [API Endpoints](#api-endpoints)
- [Security](#security) <!-- - [Database Context Diagram](#database-context-diagram) -->
- [CustomResponse Class](#customresponse-class)
- [Blacklisting Tokens in Cache](#blacklisting-tokens-in-cache)
- [Dependencies](#dependencies)
- [Contact](#contact)

## Overview

CattoAPI is built using ASP.NET Core and utilizes various libraries and frameworks to provide robust functionality for managing users, posts, comments, likes, and email services. It includes features for token management, password reset via email, and role-based access control.

## Features

- **Multi-language Support:** Available in various languages including English, Spanish, French, and more.
- **Comprehensive API Endpoints:** Manage posts, comments, likes, and email services.
- **Blacklist Token Management:** Secure your API with token blacklisting.
- **Email Services:** Send and manage email notifications.
- **Advanced Interaction Handling:** Features for managing user interactions and post engagements.

## Technologies Used

- **.NET 8.0:** Framework for building and running the API.
- **C#:** Programming language used for development.
- **SQL Server:** Database management for data persistence.
- **GitHub Actions:** CI/CD workflows for automated testing and deployment.

## Installation

To use CattoAPI, follow these steps:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/cattoapi.git
   cd cattoapi
   ```

2. **Configure the application:**
   - Set up your database connection in `appsettings.json`.
   - Configure any environment-specific settings in `appsettings.{Environment}.json`.

3. **Build and run the application:**
   ```bash
   dotnet build
   dotnet run
   ```
   The API will be accessible at `https://localhost:5001` (or `http://localhost:5000`).

4. **Explore the API endpoints described below.**

## API Endpoints

To use the API, make requests to the endpoints provided below. Here are some example requests:

### Accounts

- **Get All Accounts:**

  ```http
  GET /api/Accounts
  ```

- **Get Account by ID:**

  ```http
  GET /api/Accounts/id/{strId}
  ```

- **Get Account by Email:**

  ```http
  GET /api/Accounts/email/{email}
  ```

- **Search Accounts:**

  ```http
  GET /api/Accounts/search
  ```

### Admin

- **Change Password:**

  ```http
  PUT /api/Admin/Changepassword
  ```

- **Change Email:**

  ```http
  PUT /api/Admin/changeEmail
  ```

- **Change Username:**

  ```http
  PUT /api/Admin/ChangeUserName
  ```

- **Change Role:**

  ```http
  PUT /api/Admin/ChangeRole
  ```

- **Verify Account:**

  ```http
  PUT /api/Admin/VerifyAccount
  ```

- **Remove Profile Picture:**

  ```http
  DELETE /api/Admin/RemovePFP
  ```

- **Delete Account:**

  ```http
  DELETE /api/Admin/DeleteAccount
  ```

### Comment

- **Get User Comments:**

  ```http
  GET /api/User/Comments
  ```

- **Post a Comment:**

  ```http
  POST /api/User/Comments
  ```

- **Edit a Comment:**

  ```http
  PUT /api/User/Comments
  ```

- **Delete a Comment:**

  ```http
  DELETE /api/User/Comments
  ```

- **Get Post Comments:**

  ```http
  GET /api/Posts/Comments
  ```

### Email Services

- **Send Verification Email:**

  ```http
  POST /api/EmailServices/SendVerficationEmail
  ```

- **Verify Email:**

  ```http
  PUT /api/EmailServices/VerifyEmail
  ```

- **Send Change Password Email:**

  ```http
  POST /api/EmailServices/SendChangePasswordEmail
  ```

- **Change Password:**

  ```http
  PUT /api/EmailServices/ChangePassword
  ```

### Likes

- **Get Post Likes:**

  ```http
  GET /api/Likes/post
  ```

- **Get User Likes:**

  ```http
  GET /api/Likes/user
  ```

- **Post a Like:**

  ```http
  POST /api/Likes
  ```

- **Delete a Like:**

  ```http
  DELETE /api/Likes
  ```

### Login

- **User Login:**

  ```http
  POST /api/login
  ```

### Posts

- **Get User Posts:**

  ```http
  GET /api/User/Posts/GetPosts
  ```

- **Post a New Post:**

  ```http
  POST /api/User/Posts/PostaPost
  ```

- **Edit a Post:**

  ```http
  POST /api/User/Posts/EditaPost
  ```

- **Delete a Post:**

  ```http
  DELETE /api/User/Posts/DeletePost
  ```

### Signup

- **User Signup:**

  ```http
  POST /api/Siqnup
  ```

### User

- **Get User Data:**

  ```http
  GET /api/User/GetData
  ```

- **Change Password:**

  ```http
  PUT /api/User/ChangePassword
  ```

- **Change Profile Picture:**

  ```http
  PUT /api/User/ChangePFP
  ```

- **Get User by ID or Username:**

  ```http
  GET /api/User/{strIdOrUsername}
  ```

- **Get User Posts by ID or Username:**

  ```http
  GET /api/User/{strIdOrUsername}/posts
  ```


### Schemas

#### AccountDTO

```json
{
  "accountId": "integer($int64)",
  "email": "string",
  "userName": "string",
  "pfp": "string($byte)",
  "role": "string",
  "verified": "boolean"
}
```

#### AdminChangeModel

```json
{
  "email": "string",
  "probertyChange": "string"
}
```

#### ChangePasswordModel

```json
{
  "oldPassword": "string",
  "newPassword": "string"
}
```

#### EditCommentModel

```json
{
  "commentId": "integer($int32)",
  "commentText": "string"
}
```

#### EditPostModel

```json
{
  "postId": "integer($int32)",
  "data": "PostaPostModel"
}
```

#### PostCommentModel

```json
{
  "postId": "integer($int32)",
  "commentText": "string"
}
```

#### PostaPostModel

```json
{
  "postText": "string",
  "postPictrue": "string"
}
```

#### Siqninmodel

```json
{
  "emailOrUserName": "string",
  "password": "string"
}
```

#### SiqnupModel

```json
{
  "email": "string",
  "userName": "string",
  "password": "string",
  "repeatPassword": "string"
}
```

## Security

CattoAPI implements various security measures:

- **Authorization:** Endpoints are protected using JWT (JSON Web Token) authentication.
- **Token Blacklisting:** Tokens are blacklisted in memory cache to prevent reuse after a certain time.
- **Password Management:** Secure password storage and change processes are enforced.

<!-- ## Database Context Diagram

The database schema is managed using Entity Framework Core. Here is a simplified context diagram:

![Database Context Diagram](path/to/diagram.png) -->

## CustomResponse Class
The `CustomResponse<T>` class standardizes the shape of API responses across the application. It includes fields for response code, message, and optional data.

```csharp
namespace cattoapi.CustomResponse
{
    public class CustomResponse<T>
    {
        public CustomResponse()
        {
            responseCode = 0;
            responseMessage = string.Empty;
        }

        public CustomResponse(int responseCode, string responseMessage)
        {
            this.responseCode = responseCode;
            this.responseMessage = responseMessage;
        }

        public CustomResponse(int responseCode, string responseMessage, T data)
        {
            this.responseCode = responseCode;
            this.responseMessage = responseMessage;
            this.data = data;
        }

        public int responseCode { get; set; }
        public string responseMessage { get; set; }
        public T? data { get; set; }
    }
}

```


## Blacklisting Tokens in Cache
Tokens are blacklisted in a cache after a certain time to prevent their reuse, ensuring that tokens issued before a password change are promptly invalidated.

Implementation Details
The `BlackListTokensRepo` class manages token blacklisting using an in-memory cache (`IMemoryCache`). It provides methods to blacklist tokens based on the account ID, timestamp, and token type (e.g., login token or email verification token).

```csharp
using cattoapi.Interfaces.BlackListTokens;
using Microsoft.Extensions.Caching.Memory;
using static cattoapi.utlities.Utlities; // Assuming this includes TokenType enum

namespace cattoapi.Repos.BlackListTokens
{
    public class BlackListTokensRepo : IBlackListTokensRepo
    {
        private readonly IMemoryCache _memoryCache;

        public BlackListTokensRepo(IMemoryCache memoryCache) {
            _memoryCache = memoryCache;
        }

        public Task BlacklistTokensAsync(int accountId, DateTime timestamp, TokenType tokenType)
        {
            int timeCached = 60; // default value
            switch (tokenType)
            {
                case TokenType.Login:
                    timeCached = 3 * 60; // 3 hours
                    break;

                case TokenType.EmailToken:
                    timeCached = 10; // 10 minutes
                    break;
            }

            _memoryCache.Set(accountId, timestamp, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(timeCached)
            });
            return Task.CompletedTask;
        }

        public Task<bool> IsTokenBlacklisted(int accountId, DateTime issuedAt)
        {
            if (_memoryCache.TryGetValue(accountId, out DateTime blacklistTime))
            {
                return Task.FromResult(issuedAt <= blacklistTime);
            }
            return Task.FromResult(false);
        }
    }
}

```

## Dependencies

CattoAPI relies on several key dependencies:

- **ASP.NET Core**: Web framework
- **Entity Framework Core**: ORM for database operations
- **Microsoft.Extensions.Caching.Memory**: Memory caching for token management
- **Newtonsoft.Json**: JSON serialization
- **Microsoft.Identity.Client**: Authentication library for JWT

## Contact

For questions or inquiries, please contact us at [mahmoudnaif788@gmail.com](mailto:support@cattoapi.com).

## Note

> [!NOTE]  
> Please note that the code in this repository will not work out-of-the-box because `appsettings.json` is in `.gitignore`. This file typically contains sensitive information such as passwords, JWT keys, connection strings, and other configuration details specific to your environment. Before running the application, ensure you have set up your `appsettings.json` file correctly with appropriate values for your environment.
