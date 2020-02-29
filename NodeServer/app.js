var io = require('socket.io')(process.env.PORT || 3000)
var shortid = require('shortid')

var clients = []

//Called when a new client has connected
io.on('connection', function(socket) {
    console.log('Client connected')

    //Create a unique generated id for the new client
    var clientId = shortid.generate()
    clients.push(clientId)



    //Called when this client has connected to the server
    socket.on('connected', function () {
        //console.log('Client connected')
        socket.emit('connectInitialize')
    })

    //Called when this client has disconnected from the server
    socket.on('disconnect', function() {
        console.log('Client disconnected')
        clients.pop(clientId)
    })
})

//Prints out all of the clients currently connected to the server
function clientCount() {
    console.log(clients.length + ' clients are connected to server')

    var allClients = []
    clients.forEach(element => {
        allClients.push(element)
    });
    console.log(allClients)
}

console.log('Server connected')