import { ApplicationInfo } from './application-info.interface';
import { User } from './user.interface';

export interface AppUserInfo {

    readonly application: ApplicationInfo;
    readonly user: User;
}
