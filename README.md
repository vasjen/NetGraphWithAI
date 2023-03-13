# NetGraphWithAI

[![Hack Together: Microsoft Graph and .NET](https://img.shields.io/badge/Microsoft%20-Hack--Together-orange?style=for-the-badge&logo=microsoft)](https://github.com/microsoft/hack-together)

## The idea

Develop a mail client (MVP) with an integrated ChatGPT feature that will help expand the capabilities of a regular mail client.
The goal was to implement two directions.

* For people with limited abilities (deaf) - to transcribe audio content sent as attachments. Recordings of conversations, conferences, podcasts - everything can be transcribed into text and studied right away.
* To remove the language barrier for teams consisting of people from different countries by allowing the translation of sent and received messages into English.

I am confident that integrating a language model will significantly enhance the experience of interacting with many corporate products, but in this project of the hackathon, it was decided to focus only on these two tasks and the mail client.


### Technical Stack
* ASP.NET Core 7.0
* C#
* Graph SDK
* OpenAI API

### Installation guide

1. Sign up for a **[Microsoft 365 Developer Program](https://aka.ms/m365developers)** subscription.
2. Register application for user authentication ([How to do it](https://learn.microsoft.com/en-us/graph/tutorials/dotnet?tabs=aad&tutorial-step=1))
3. Store ClientId, TenantId and Domain in appsetings.json
4. Add credentials to secrets.json

```sh
dotnet user-secrets init
dotnet user-secrets set "AzureAD:ClientId" "Your_Azure_AD_Client_Id"
dotnet user-secrets set "AzureAD:ClientSecret" "Your_Azure_AD_Client_Secret"
```
5. Visit [personal account OpenAI](https://platform.openai.com/account/api-keys) to create and get your secret Key
6. Store this key in secrets.json of project
```sh
dotnet user-secrets set "OpenAIAPI" "Your_OPENAI_Secret"
```
7. Run application
