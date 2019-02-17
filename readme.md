# Identity Server CLI [![Build Status](https://travis-ci.com/cassiofariasmachado/identity-server-cli.svg?branch=master)](https://travis-ci.com/cassiofariasmachado/identity-server-cli) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=cassiofariasmachado_identity-server-cli&metric=alert_status)](https://sonarcloud.io/dashboard?id=cassiofariasmachado_identity-server-cli) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=cassiofariasmachado_identity-server-cli&metric=coverage)](https://sonarcloud.io/dashboard?id=cassiofariasmachado_identity-server-cli)

_**IMPORTANT: Work in progress.**_

Command line interface to manage API resources, clients and identity resources of your Identity Server 4 instance. 

## Commands

### New

Command to add new clients, api and identity resources.

_Usage:_

`is4 new [command]`

#### Subcommands

##### Api Resource

Create an api resource.

_Usage:_

`is4 new api-resource [arguments] [options]`

_Arguments:_

`Name` The unique name of the resource.

_Options:_

`-?|-h|--help ` Show help information

`--disabled` Indicates if this resource is disabled. Defaults to enabled.
  
`--display-name <display-name>` Display name of the resource.

`--description <description>` Description of the resource.

`--user-claims <user-claims>` List of accociated user claims that should be included when this resource is requested.

`--scopes <scopes>` The scopes of API


##### Client

_Usage:_

`is4 new client [arguments] [options]`

_Arguments:_

`ClientId`Unique ID of the client.

_Options:_

`-?|-h|--help` Show help information

`--disabled` Indicates if this client is disabled. Defaults to enabled.

`--client-name <client-name>` Client display name (used for logging and consent screen).

`--description <description>` Description of the client.

`--client-uri <client-uri>` URI to further information about client (used on consent screen).
  
`--logo-uri <logo-uri>` URI to client logo (used on consent screen).
  
`--client-secrets <client-secrets>` Client secrets - only relevant for flows that require a secret.

`--secret-algorithm <secret-algorithm>` The algorithm used to encode the client secrets, can be "sha256" or "sha512". Defaults to sha256.

`--allowed-grant-types <allowed-grant-types>` Specifies the allowed grant types (legal combinations of AuthorizationCode, Implicit, Hybrid, ResourceOwner, ClientCredentials).

`--redirect-uris <redirect-uris>` Specifies allowed URIs to return tokens or authorization codes to.

`--post-logout-redirect-uris <post-logout-redirect-uris>` Specifies allowed URIs to redirect to after logout.

`--allowed-scopes <allowed-scopes>` Specifies the api scopes that the client is allowed to request. If empty, the client can't access any scope.

`--allowed-cors-origins <allowed-cors-origins>` The allowed CORS origins for JavaScript clients.

#### Identity Resource

Create an identity resource.

_Usage:_ 

`is4 new identity-resource [arguments] [options]`

_Arguments:_

`Name` The unique name of the resource.

_Options:_

`-?|-h|--help` Show help information

`--disabled` Indicates if this resource is disabled. Defaults to enabled.
  
`--display-name <display-name>` Display name of the resource.
  
`--description <description>` Description of the resource.
  
`--user-claims <user-claims>` List of accociated user claims that should be included when this resource is requested.
  
`--emphasize` Specifies whether the consent screen will emphasize this scope. Defaults to false.

`--required` Specifies whether the user can de-select the scope on the consent screen. Defaults to false.

`--no-show-in-discovery-document` Specifies whether this scope isn't shown in the discovery document. Defaults to false.