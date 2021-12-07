# TwilioTests.API
A simple .NET 6 project that tests Twilio's Voice and SMS APIs, using Twilio .NET SDK.

There are an incoming and outgoing call controllers with simple flows - user input, audio playback, recording of part of the call, etc.
There is also the SMS logic, which is not that complicated anyway.

**Register in Twilio with [this link](www.twilio.com/referral/UYpnZT) to get bonus $10 when you upgrade your account.**

# TwilioServerless
Example serverless service which can be used to fetch simple data and to be used in the builder

The files in the `functions` folder are the actual endpoints.
Files ending with `.protected.js` can be accessed only with valid SID and Token passed with the request.

### Docs
https://github.com/twilio-labs/serverless-toolkit/tree/main/packages/plugin-serverless

### Setup
`npm install -g twilio-cli`

`plugins:install @twilio-labs/plugin-serverless`

### Init new 
`twilio serverless:init`

### Deploy to DEV
`twilio serverless:deploy`

### Deploy to a named environment
`twilio serverless:deploy --environment=prod`

### Deploy to production (no domain suffix)
`twilio serverless:deploy --production`

### List all environments for a resource
`twilio api:serverless:v1:services:environments:list --service-sid xxxxxxxxxxxxxxxxxxxx`

### Delete 
`twilio api:serverless:v1:services:remove --sid SID`

### Promote a build
`twilio serverless:promote --build-sid ZB01119b906ee6de780d129728ffb5620b --environment=prod --create-environment`
