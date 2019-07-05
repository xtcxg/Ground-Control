let Client = require('ssh2').Client;

export class SClient{
    private client : any;
    constructor(um){
        //username,password,host,port
        // let um:[String,String,String,number];
        this.client = new Client();
        this.client.connect(function(){
            um[0],
            um[1],
            um[2],
            um[3]
        })(um);
    }
}
