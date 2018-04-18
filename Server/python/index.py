from flask import Flask, render_template
from flask_socketio import SocketIO, emit
from threading import Timer

app = Flask(__name__)

app.config[ 'SECRET_KEY' ] = 'abhi'
socketio = SocketIO(app)

# def set_interval(func, sec):
# 	def func_wrapper():
# 		set_interval(func, sec)
# 		func()
# 	t = threading.Timer(sec, func_wrapper)
# 	t.start()
# 	return t

def hello():
	socketio.emit('chat message','hey')

@app.route( '/' )
def index():
	return render_template( './chat.html' )

@socketio.on('connect')
def fun():
	t = Timer(1,hello)
	t.start()

@socketio.on( 'chat message' )
def handle_this( msg ):
	print('recieved' + msg )
	socketio.emit('chat message', msg )

if __name__ == '__main__':
	socketio.run( app, debug= True)