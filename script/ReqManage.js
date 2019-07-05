"use strict";
exports.__esModule = true;
var ReqManage = /** @class */ (function () {
    function ReqManage() {
        this.conns = new Array();
    }
    ReqManage.prototype.setConn = function (conn) {
        this.conns[(this.conns).length] = conn;
        conn.on('text', function (str) {
            console.log(str);
            conn.sendText("来自服务器的消息:" + str);
        });
        conn.on("close", function (code, reason) {
            console.log("Connection closed");
        });
    };
    ReqManage.prototype.distribute = function (msg) {
    };
    return ReqManage;
}());
exports.ReqManage = ReqManage;
