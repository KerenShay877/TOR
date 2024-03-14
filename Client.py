from base64 import b64decode, encode
import socket
from _thread import *
from threading import Thread
from xmlrpc.client import Server
from Crypto.PublicKey.RSA import RsaKey
from Cipher import *
import win32pipe, win32file
import win32file
import requests


s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

serverS = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

nodeOneS = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
nodeTwoS = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
nodeThreeS = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

ip = ''
server = '176.58.113.7'
server_port = 8888
listen_port = 8804
server_ip = socket.gethostbyname(server)

nextPort = ''
circutId = ''
length = ''

nodeOneIp = ''
nodeTwoIp = ''
nodeThreeIp = ''
destClientIp = ''
nodeOnePort = ''
nodeTwoPort = ''
nodeThreePort = ''
destClientPort = '8804'

data = ''
numOfNode = ''
packet = ''

serverKey = ''

try:
    s.bind((ip, listen_port))
except socket.error as e:
    print("the error is - " + str(e))

s.listen(2)

fileHandle = win32file.CreateFile("\\\\.\\pipe\\torPipe",  # Create connection with the gui
                            win32file.GENERIC_READ | win32file.GENERIC_WRITE,
                            0, None,
                            win32file.OPEN_EXISTING,
                            0, None)

fileHandleSend = win32file.CreateFile("\\\\.\\pipe\\recieveTorPipe",  # Create connection with the gui for send message
                            win32file.GENERIC_READ | win32file.GENERIC_WRITE,
                            0, None,
                            win32file.OPEN_EXISTING,
                            0, None)

def main():
    try:
        serverS.connect((server, 8888))
        print("Connected to server!")
        setServerKey(serverS)
        authentication(serverS)
        
        guiThread = Thread(target=listen_thread_gui, args=(serverS,))
        guiThread.start()

        #sendThread = Thread(target=threaded_client, args=(serverS,))
        #sendThread.start()

        listenS, addr = s.accept()
        print("Connected to: ", addr)
        recivedThread = Thread(target=listening_thread_dest_client, args=(listenS,))
        recivedThread.start()

    except error:
        print("Could not connet please try again - ", error)
        serverS.sendall(str.encode("3")) # Tell the server to close the socket and remove the circut id
        serverS.close()
        

def listen_thread_gui(sock: serverS):
    global circutId, destClientIp

    reply = ''
    serverS.sendall(str.encode("1")) # Get node route from the server
    data = serverS.recv(2048)
    reply = data.decode('utf-8')
    print(reply)
    setNodeIpAndPorts(reply)
    
    serverS.sendall(str.encode("2")) # Get circut id from the server
    data = serverS.recv(2048)
    circutId = data.decode('utf-8')
    setKeys() 

    while True:
        data = win32file.ReadFile(fileHandle, 4096)[1].decode().rstrip('\x00')
        if data == 'getFriends':
            serverS.send(str.encode('9'))
            friendList = serverS.recv(1024).decode()
            print('friendList', friendList)
            win32file.WriteFile(fileHandle, friendList.encode())
        elif data == 'addFriend':
            friendUsername = win32file.ReadFile(fileHandle, 4096)[1].decode().rstrip('\x00')
            serverS.send(str.encode('10'))
            serverS.send(str.encode(friendUsername))
            status = serverS.recv(1024).decode()
            win32file.WriteFile(fileHandle, status.encode())
        elif data == 'msg':
            msg = win32file.ReadFile(fileHandle, 4096)[1].decode().rstrip('\x00')
            packet = createPacket(msg)
            nodeOneS.sendall(str.encode(packet))
        elif data == 'setDestIp':
            friendUsername = win32file.ReadFile(fileHandle, 4096)[1].decode().rstrip('\x00')
            serverS.send(str.encode('11'))
            serverS.send(str.encode(friendUsername))
            destClientIp = serverS.recv(1024).decode()
            print('destClientIp', destClientIp)


def listening_thread_dest_client(clientSock):
    """
    This function will wait from message from another client and communicate with the gui
    Input: socket
    Output: None
    """
    reply = ''

    while True:
        print('waiting for listenS accept')
        data = clientSock.recv(2048)
        reply = data.decode('utf-8')
        print('Listen reply - ' + reply)
        message = str(reply).split('*&^*')
        win32file.WriteFile(fileHandleSend, message[9].encode()) # Send msg to gui

def threaded_client(clientSock):
    """
    This function will deal with the client or the node
    Input: Socket of the client or the node
    Output: None
    """
    global circutId, destClientIp
    reply = ''
    clientSock.sendall(str.encode("1")) # Get node route from the server
    data = clientSock.recv(2048)
    reply = data.decode('utf-8')
    print(reply)
    setNodeIpAndPorts(reply)
    
    clientSock.sendall(str.encode("2")) # Get circut id from the server
    data = clientSock.recv(2048)
    circutId = data.decode('utf-8')
    setKeys() 
    
    try:
        while True:      
           
            createPacket()
            nodeOneS.sendall(str.encode(packet))
            
    except Exception:
        print("error - ", error)   

    clientSock.sendall(str.encode("3")) # Tell the server to close the socket and remove the circut id
    print("Connection Closed")
    clientSock.close()


def createPacket(data):
    """
    This function will create and encrypt the packet 
    Input: None
    Output: None 
    """
    global circutId, length , numOfNode, packet, nodeTwoPort, nodeThreePort, destClientPort, nodeTwoIp, nodeThreeIp, destClientIp
    chiperNodeOne = AESCipher(keyNodeOne)
    chiperNodeTwo = AESCipher(keyNodeTwo)
    chiperNodeThree = AESCipher(keyNodeThree)

    tempNodeTwoPort = nodeTwoPort
    tempNodeThreePort = nodeThreePort
    tempDestClientPort = destClientPort
    tempNodeTwoIp = nodeTwoIp
    tempNodethreeIp = nodeThreeIp
    tempDestClientIp = destClientIp

    length = len(data)
    length = str(length).zfill(4)
    data = '**' + data + '*' # fix bug 
    numOfNode = '1'

    data = chiperNodeThree.encrypt(str(data))
    data = chiperNodeTwo.encrypt(str(data))
    data = chiperNodeOne.encrypt(str(data))
    
    nodeTwoIp = chiperNodeOne.encrypt(str(nodeTwoIp))
    nodeTwoPort = chiperNodeOne.encrypt(str(nodeTwoPort))

    nodeThreeIp = chiperNodeTwo.encrypt(str(nodeThreeIp))
    nodeThreeIp = chiperNodeOne.encrypt(str(nodeThreeIp))
    nodeThreePort = chiperNodeTwo.encrypt(str(nodeThreePort))
    nodeThreePort = chiperNodeOne.encrypt(str(nodeThreePort))

    destClientIp = chiperNodeThree.encrypt(str(destClientIp))
    destClientIp = chiperNodeTwo.encrypt(str(destClientIp))
    destClientIp = chiperNodeOne.encrypt(str(destClientIp))
    destClientPort = chiperNodeThree.encrypt(str(destClientPort))
    destClientPort = chiperNodeTwo.encrypt(str(destClientPort))
    destClientPort = chiperNodeOne.encrypt(str(destClientPort))

    nodeTwoIp = nodeTwoIp.decode("utf-8")
    nodeThreeIp = nodeThreeIp.decode("utf-8")
    destClientIp = destClientIp.decode("utf-8")

    nodeTwoPort =nodeTwoPort.decode("utf-8")
    nodeThreePort = nodeThreePort.decode("utf-8")
    destClientPort = destClientPort.decode("utf-8")
    
    data = data.decode("utf-8")
    packet = str(length) + '*&^*' + str(numOfNode) + '*&^*' + str(circutId) + '*&^*' + str(nodeTwoIp) +'*&^*' +  str(nodeTwoPort) + '*&^*' + str(nodeThreeIp) + '*&^*' + str(nodeThreePort) + '*&^*' + str(destClientIp) + '*&^*' + str(destClientPort) + '*&^*' + str(data)
    print(packet)
    nodeTwoIp = tempNodeTwoIp
    nodeThreeIp = tempNodethreeIp
    destClientIp = tempDestClientIp
    nodeTwoPort= tempNodeTwoPort 
    nodeThreePort = tempNodeThreePort
    destClientPort = tempDestClientPort
    return packet

def setNodeIpAndPorts(portRoute):
    """
    This function will set all the ports and ips of the circut
    Input: Port route that received from the server
    Output: None 
    """
    global nodeTwoPort, nodeThreePort, nodeOnePort, nodeOneIp, nodeTwoIp, nodeThreeIp
    portRoute = portRoute.split('#')
    print(portRoute)
    nodeOneIp = portRoute[0]
    nodeOnePort = portRoute[1]
    nodeTwoIp = portRoute[2]
    nodeTwoPort = portRoute[3]
    nodeThreeIp = portRoute[4]
    nodeThreePort = portRoute[5]    

def getKeysFromNodes(public_key):    
    """
    This function will get all the symetric keys from the nodes
    Input: Public key to send to nodes
    Output: All the symetric key that received from the nodes
    """
    global circutId
    print(nodeOneIp + " " +  nodeOnePort)
    print(nodeTwoIp + " " + nodeTwoPort)
    print(nodeThreeIp + " " + nodeThreePort)
    nodeOneS.connect((nodeOneIp, int(nodeOnePort)))
    nodeTwoS.connect((nodeTwoIp, int(nodeTwoPort)))
    nodeThreeS.connect((nodeThreeIp, int(nodeThreePort)))

    nodeOneS.sendall(str.encode('key'))
    nodeTwoS.sendall(str.encode('key'))
    nodeThreeS.sendall(str.encode('key'))

    nodeOneS.recv(1024).decode('utf-8')
    nodeTwoS.recv(1024).decode('utf-8')
    nodeThreeS.recv(1024).decode('utf-8')

    nodeOneS.sendall(str.encode(public_key + "*&^*" + str(circutId)))
    nodeTwoS.sendall(str.encode(public_key+ "*&^*" + str(circutId)))
    nodeThreeS.sendall(str.encode(public_key+ "*&^*" + str(circutId)))

    keyNodeOne = nodeOneS.recv(1024).decode('utf-8')
    keyNodeTwo =nodeTwoS.recv(1024).decode('utf-8')
    keyNodeThree = nodeThreeS.recv(1024).decode('utf-8')

    
   
    return keyNodeOne, keyNodeTwo, keyNodeThree
 
def setServerKey(sock: serverS):
    """
    This function will send public a-symetic key to the server and then he will recive the symetric key
    Input: socket with the server
    Output: None
    """
    global serverKey
    serverS.sendall(str.encode('5'))
    chiperRSA = RSACipher()
    my_key = chiperRSA.public_key.exportKey('PEM')
    public_key = my_key.decode('ascii')
    serverS.sendall(str.encode(public_key))
    serverKey = serverS.recv(1024).decode('utf-8')
    serverKey = chiperRSA.decrypt(serverKey, chiperRSA.private_key).decode()

def setKeys():
    """
    This function will create the public key and send to the nodes 
    Input: None
    Output: None
    """
    global keyNodeOne, keyNodeTwo, keyNodeThree
    chiperRSA = RSACipher()

    my_key = chiperRSA.public_key.exportKey('PEM')
    public_key = my_key.decode('ascii')

    keyNodeOne, keyNodeTwo, keyNodeThree = getKeysFromNodes(public_key)
    
    keyNodeOne = chiperRSA.decrypt(keyNodeOne, chiperRSA.private_key).decode()
    keyNodeTwo = chiperRSA.decrypt(keyNodeTwo, chiperRSA.private_key).decode()
    keyNodeThree =chiperRSA.decrypt(keyNodeThree, chiperRSA.private_key).decode()
    
def authentication(sock: serverS):
    """
    This function is responsible for receiving the login information from the gui
    Input: socket with the server
    Output: None
    """
    chiperServer = AESCipher(serverKey)
    while True:
        data = win32file.ReadFile(fileHandle, 4096)[1].decode().rstrip('\x00')
        if data == 'Login':
            username = win32file.ReadFile(fileHandle, 4096)[1].decode().rstrip('\x00')
            password = win32file.ReadFile(fileHandle, 4096)[1].decode().rstrip('\x00')
            username = chiperServer.encrypt(str(username))
            password = chiperServer.encrypt(str(password))
            serverS.sendall(str.encode('6')) # Tell the server is Login request
            serverS.sendall(username + '()'.encode() + password)
            status = serverS.recv(1024).decode()
            win32file.WriteFile(fileHandle, status.encode())
            if status == '1':
                return
        if data == 'Register':
            username = win32file.ReadFile(fileHandle, 4096)[1].decode().rstrip('\x00')
            password = win32file.ReadFile(fileHandle, 4096)[1].decode().rstrip('\x00')
            mail = win32file.ReadFile(fileHandle, 4096)[1].decode().rstrip('\x00')

            username = chiperServer.encrypt(str(username))
            password = chiperServer.encrypt(str(password))
            mail = chiperServer.encrypt(str(mail))
    
            serverS.sendall(str.encode('8')) # Tell the server is Register request
            serverS.sendall(username + '()'.encode() + password + '()'.encode() + mail)
            status = serverS.recv(1024).decode()
            win32file.WriteFile(fileHandle, status.encode())
            if status == '1':
                return
        
if __name__ == "__main__":
    main()