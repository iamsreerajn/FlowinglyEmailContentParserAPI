**E-Mail Content Parser** **[ECP App]**
        This is a Web API application developped in .net core(6) with the backend support of SQL Server and Entiry framewor
Current version of **ECP_App** is capable of handling below actions.
1. **Get** list of already added expences and reservations from database
2. **Create** new expenses and reservations from the email content that has XML tags. There are level of validations are integrated to the tool in this activity, as below.
        • Opening tags that have no corresponding closing tag. In this case the entire message must be rejected.
        • Missing <total>. In this case the entire message must be rejected.
        • Missing <cost_centre>. In this case value of the ‘cost centre’ field in the output must default to ‘UNKNOWN’.
3. **Update** the already available expenses and reservations
4. **Delete** the existing records by providing its IDs

**ECP_APP** 
        App is developped by following standard design patterns and coding/design principles. As it is a tiny application, I could follow only Factory Pattern. code is covered with SOLID principles. 
        It is loosely coupled and enhancement and testability is more in current version.

**Below are the APIs available to consume**
<img width="923" alt="image" src="https://github.com/iamsreerajn/FlowinglyEmailContentParserAPI/assets/81985462/b6f72b8f-1b28-4cf8-a0cc-629c05b32e4a">

**Database** 
        Database is configured on the centralized database server which I have access. 

**Testing** 
        I have perfomed a few round of testing with the given validation scenarios and found that it worked.


**Technical Competencies Used**
        • Dot Net Core Web API
        • Entity Framework - Code first model
        • Sql Server





