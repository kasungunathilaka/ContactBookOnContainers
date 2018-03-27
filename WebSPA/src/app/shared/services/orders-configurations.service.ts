import { Injectable } from '@angular/core'

@Injectable()
export class OrdersConfigurationService {
  public host_port = 'localhost:5104';
  public Server = 'http://' + this.host_port + '/';
  public ApiUrl = 'api/v1/';
  public ServerWithApiUrl = this.Server + this.ApiUrl;
}
