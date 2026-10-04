## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) 10.0 or later
- [Node.js](https://nodejs.org/) 22 or later
- Clone the repo

### Run the backend

```bash
cd backend/GreenhouseGuard.Api
dotnet run
```

### Run the frontend

```bash
cd frontend
npm install
npm start
```

### Assumptions

- In the provided modal definition, none of the properties was marked optional. Therefore assuming there shouldn't be any missing sensor readings despite it is still being handled on the server
- I was not sure what the status color for each sensor reading should be... green/yellow/red so this wasn't implemented



### Could be improved

- I don't have any experience with Angular and it was quite challenging to pick up the basics in a short period of time. Many practices and libraries simply do not exist in React ecosystem. I've only finished the bare minimum requirements from the document.
- Many things I would improve such as UI designs, adding chart.js, offline queue, more error handling, more unit tests... I wish I can in general just do more testing. However, I just don't have enough Angular knowledge to continue trying to implement more features that I already have.
- I would have a very clear picture of what I need to do if I were to implement this in React and I believe if that was the case, I should be able to meet all requirements... As I mentioned, this is the best I can do for now.



### Testing

- Personally I used Postman. I've attached a test.json file in the repo which can be used for testing.

