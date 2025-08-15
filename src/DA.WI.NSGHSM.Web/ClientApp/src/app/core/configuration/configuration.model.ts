export interface Configuration {

    readonly apiUrl: string;
    readonly stsUrl: string;

    readonly stsClient: StsClientConfiguration;
}

export interface StsClientConfiguration {

  readonly clientId: string;
  readonly redirectUrl: string;
  readonly postLogoutRedirectUri: string;
  readonly responseType: string;
  readonly scope: string;

  readonly silentRenew: boolean;
  readonly silentRenewUrl: string;

  readonly oAuthTokenValidationTimeOut: string;
}
