import { Locale } from './locale.interface';
import { FooterInfo } from './footer-info.interface';
import { MenuInfo } from './menu-info.interface';

export interface ApplicationInfo {

    readonly title: string;
    readonly name: string;
    readonly locales: { [code: string]: Locale; };
    readonly footer: FooterInfo;

    readonly version: string;
    readonly templateVersion: string;
    readonly environment: string;

    readonly description: string;
    readonly customer: string;
    readonly copyright: string;
    readonly menu: MenuInfo[];
}
