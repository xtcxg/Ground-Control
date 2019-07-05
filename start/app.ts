"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ws = require("nodejs-websocket");
const electron = require('electron');
import {ReqManage} from "../script/ReqManage";
const app = electron.app;
const rmg = new ReqManage();
//创建监听
ws.createServer(function (conn) {
    console.log("new conn");
    rmg.setConn(conn);
}).listen(88);
//创建GUI
app.setName('Ground Control');
app.on('ready', function () {
    const mainWindow = new electron.BrowserWindow();
    mainWindow.loadURL('file://' + __dirname + '/electron-tabs.html');
    mainWindow.on('ready-to-show', function () {
        mainWindow.show();
        mainWindow.focus();
    });
});
