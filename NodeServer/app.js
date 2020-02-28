var io = require('socket.io')(process.env.PORT || 3000)
var shortid = require('shortid')

var clients = []

//Called when a new client has connected
io.on('connection', function(socket) {
    console.log('Client connected')

    //Create a unique generated id for the new client
    var clientId = shortid.generate()
    clients.push(clientId)
    console.log('Clients ' + clients.length)


    //Called when this client has disconnected from the server
    socket.on('disconnect', function() {
        console.log('Client disconnected')
        clients.pop(clientId)
        console.log('Clients ' + clients.length)
    })
})

console.log('Server connected')