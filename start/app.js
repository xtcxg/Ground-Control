const electron = require('electron');
const app = electron.app;

app.setName('Ground Control');


app.on('ready', function () {

    const mainWindow = new electron.BrowserWindow();
    mainWindow.loadURL('file://' + __dirname + '/electron-tabs.html');
    mainWindow.on('ready-to-show', function () {
        mainWindow.show();
        mainWindow.focus();
    });

});
