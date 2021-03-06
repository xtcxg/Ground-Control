"use strict";
exports.__esModule = true;
Object.defineProperty(exports, "__esModule", { value: true });
var ws = require("nodejs-websocket");
var electron = require('electron');
var ReqManage_1 = require("../script/ReqManage");
var app = electron.app;
var rmg = new ReqManage_1.ReqManage();
//创建监听
ws.createServer(function (conn) {
    console.log("new conn");
    rmg.setConn(conn);
}).listen(88);
//创建GUI
app.setName('Ground Control');
app.on('ready', function () {
    var mainWindow = new electron.BrowserWindow();
    mainWindow.loadURL('file://' + __dirname + '/electron-tabs.html');
    mainWindow.on('ready-to-show', function () {
        mainWindow.show();
        mainWindow.focus();
    });
});
