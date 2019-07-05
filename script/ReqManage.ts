export class ReqManage{
    public conns : any[] = new Array();
    public setConn(conn:any){
        this.conns[(this.conns).length] = conn;
        conn.on('text', function (str) {
            console.log(str);
            conn.sendText("来自服务器的消息:" + str);
        });
        conn.on("close", function (code, reason) {
            console.log("Connection closed");
        });
    }

    public distribute(msg:String){

    }
}