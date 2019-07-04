"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ws = require("nodejs-websocket");
const electron = require('electron');
const app = electron.app;

app.setName('Ground Control');


app.on('ready', function () {

    const mainWindow = new electron.BrowserWindow();
    mainWindow.loadURL('file://' + __dirname + '/electron-tabs.html');
    mainWindow.on('ready-to-show', function () {
        mainWindow.show();
        mainWindow.focus();
        ws.createServer(function (conn) {
            console.log("new conn");
            conn.on('text', function (str) {
                console.log(str);
                conn.sendText("来自服务器的消息:" + str);
            });
            conn.on("close", function (code, reason) {
                console.log("Connection closed");
            });
        }).listen(88);
    });
});
