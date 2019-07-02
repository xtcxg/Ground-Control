let Client = require('ssh2').Client;

var conn = new Client();


conn.connect({
    host: '192.168.0.105',
    port: 22,
    username: 'miex',
    password: 'lz199641'
});

setTimeout(function () {
    conn.exec('uname -a', function(err, stream) {
        if (err) throw err;
        stream.on('close', function(code, signal) {
            conn.end();
        }).on('data', function(data) {
            console.log('STDOUT: ' + data);
        }).stderr.on('data', function(data) {
            console.log('STDERR: ' + data);
        });
    });
},1000);