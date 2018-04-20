import { Injectable } from '@angular/core'
import { environment } from '../../../environments/environment';
import  * as env_variables  from '../../../../config.json';

@Injectable()
export class ContactsConfigurationService {
  public host_port = (<any>env_variables).contacts_host_port;
  public Server = 'http://' + this.host_port + '/';
  public ApiUrl = 'api/v1/';
  public ServerWithApiUrl = this.Server + this.ApiUrl;
}
