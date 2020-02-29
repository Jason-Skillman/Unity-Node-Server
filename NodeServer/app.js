var io = require('socket.io')(process.env.PORT || 3000)
var shortid = require('shortid')

var clients = []

//Called when a new client has connected
io.on('connection', function(socket) {
    console.log('Client connected')

    //Create a unique generated id for the new client
    var clientId = shortid.generate()
    clients.push(clientId)



    //Notify all other clients that this client has connected. Pass them this client'd id
    socket.broadcast.emit('clientConnect', {
        id: clientId
    })

    //Notify this client of all of the other clients that have already connected
    clients.forEach(function(currentId) {
        //Dont spawn this client again
        if(currentId == clientId) return
        
        socket.emit('clientConnect', {
            id: currentId
        })
    })



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

    //Called when any ...
    socket.on('move', function(data) {
        console.log('Player is moving', JSON.stringify(data))
        data.id = clientId;
        socket.broadcast.emit('move', data)
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