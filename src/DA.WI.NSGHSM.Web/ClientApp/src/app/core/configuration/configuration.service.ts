import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Configuration, StsClientConfiguration } from './configuration.model';
import { LogService } from '../log/log.service';
import { UrlHelperService } from '../helpers/url-helper.service';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {


  private isInitialized: boolean = false;
  private configuration: Configuration;

  constructor(
    private http: HttpClient,
    private urlHelper: UrlHelperService,
    private logService: LogService
  ) {
  }

  public init(): Observable<Configuration> {

    this.logService.info('initializing ConfigurationService...');

    return this
      .http
      .get<Configuration>('/configuration')
      .pipe(
        map(result => {

          this.configuration = this.createConfiguration(result);
          this.logService.info('ConfigurationService successfully initialized.', this.configuration);

          this.isInitialized = true;

          return this.configuration;
        }));
  }

  public get() {

    if (!this.isInitialized) {

      throw Error('Configuration Service is not yet initialized. Call init() before get().');
    }

    return this.configuration;
  }

  private createConfiguration(result: Configuration): Configuration {

    return {

      apiUrl: this.urlHelper.trimTrailingSlash(result.apiUrl),
      stsUrl: this.urlHelper.trimTrailingSlash(result.stsUrl),
      stsClient: this.createStsClientConfig(result.stsClient)
    };
  }

  private createStsClientConfig(result: StsClientConfiguration): StsClientConfiguration {

    return {

      clientId: result && result.clientId,
      redirectUrl: this.setStsClientUrl(result && result.redirectUrl),
      postLogoutRedirectUri: this.setStsClientUrl(result && result.postLogoutRedirectUri),
      responseType: result && result.responseType,
      scope: result && result.scope,
      silentRenew: result && result.silentRenew,
      silentRenewUrl: this.setStsClientUrl(result && result.silentRenewUrl, 'silent-renew.html'),
      oAuthTokenValidationTimeOut: result && result.oAuthTokenValidationTimeOut
    };
  }

  private setStsClientUrl(url: string, segment: string = null): string {

    url = url || this.urlHelper.baseUrl;
    return this.urlHelper.combine(url, segment);
  }

}
