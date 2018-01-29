var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);

var cordinates = {
	"u" : 0,
	"d" : 0,
	"l" : 0,
	"r" : 0
};

/*
app.get('/', function(req, res){
	res.sendFile('D:/Practice/node'+'/index.html');
});
*/
var convertedData = {
	"H" : 0,
	"V" : 0
};
var convertType = ["H","V"];

function convert(posVal, negVal, cType)
{
	var mappedData = (convertedData[convertType[cType]])*10;
	var amount = 0.1 * 10;

	if(posVal == 0 && negVal == 0){
		if(mappedData > 0)
			mappedData -= amount;
		else if(mappedData < 0)
			mappedData += amount;
		else
			mappedData = 0;
	}
	else if(posVal == 1 && negVal == 0){
		if(mappedData < 10)
			mappedData += amount;
	}
	else if(posVal == 0 && negVal == 1){
		if(mappedData > -10)
			mappedData -= amount;
	}
	else if(posVal == 1 && negVal == 1){
		if(mappedData < 10)
			mappedData += amount;
	}

	convertedData[convertType[cType]] = mappedData / 10;
}


app.get('/', function(req, res){
	res.sendFile('D:/Practice/node'+'/index.html');
});

app.get('/chat', function(req, res){
	res.sendFile('D:/Practice/node'+'/chat.html');
});

io.on('connection', function(socket){
	console.log('a user connected');
	
	socket.on('send data',function(data){
		console.log('sending data to client');
		socket.emit('datafromserver',cordinates);
	});
	socket.on('disconnect', function(){
		console.log('user disconnected');
	});
	socket.on('chat message', function(msg){
		io.emit('chat message', msg);
    console.log('message: ' + msg);
  });

	socket.on('keydata', function(keydata){
		cordinates.u = keydata.u;
		cordinates.d = keydata.d;
		cordinates.l = keydata.l;
		cordinates.r = keydata.r;
		//console.log("U : "+keydata.u+" D : "+keydata.d+" R : "+keydata.r+" L : "+keydata.l);
	});

	var inte = setInterval(function(){
	// var data = sensor.readSync();
	// datasend.rot=data.rotation.z;
	//datasend.rot = ".5";
	convert(cordinates.u, cordinates.d, 1);
	convert(cordinates.r, cordinates.l, 0);
	socket.emit("datarec",convertedData);
	//console.log(convertedData);
	},100);
});

http.listen(3000, function(){
	console.log('listening on *:3000');
});