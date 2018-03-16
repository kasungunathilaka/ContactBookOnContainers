import { Injectable } from '@angular/core'

@Injectable()
export class ConfigurationService {
    public host_port = 'localhost:5103';
    public Server = 'http://' + this.host_port + '/';
    public ApiUrl = 'api/v1/';
    public ServerWithApiUrl = this.Server + this.ApiUrl;
}
