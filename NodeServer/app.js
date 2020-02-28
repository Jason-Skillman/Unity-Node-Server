var io = require('socket.io')(process.env.PORT || 3000)
var shortid = require('shortid')

//Called when a new client has connected
io.on('connection', function(socket) {
    console.log('Client connected')
})

console.log('Server connected')