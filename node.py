import socket
from _thread import *
import os
from Cipher import *

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
nextNodeS = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
serverS = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

server = '176.58.113.7'
server_ip = socket.gethostbyname(server)
ip = ''

nextIp = ''
nextPort = ''
circutId = ''
length = ''
nodeTwoPort = ''
nodeThreePort = ''
destClientPort = ''

nodeTwoIp = ''
nodeThreeIp = ''
destClientIp = ''

replyData = ''
numOfNode = ''
key = ''
keys_map = {} # key = circutId, value = key

port = 8800


try:
    s.bind((ip, port))
except socket.error as e:
    print(str(e))

s.listen(2)
print("Waiting for a connection")

def main():
    serverS.connect((server, 8888))
    print("Connected to server!")

    serverS.sendall(str.encode('4')) # ask for register the node in the server nodes list
    status = s.recv(1024).decode()
    print('code', status)
        

    while True:
        previousNodeS, addr = s.accept()
        print("Connected to: ", addr)
        start_new_thread(threaded_client, (previousNodeS,))



def threaded_client(previousNodeS):
    """
    This function will deal with the client or the node
    Input: Socket of the client or the node
    Output: None
    """
    while True:
        try:
            data = previousNodeS.recv(2048)
            reply = data.decode('utf-8')
      
            if not reply: # Message is empty
                break

            elif reply == 'key': # The next Message will be the key
                previousNodeS.sendall(str.encode('key'))
                data = previousNodeS.recv(2048)
                reply = data.decode('utf-8')
                sendKey(reply, previousNodeS)
           
            else:
                setVars(reply)                
      
                # Set the next port and ip by the number of the node
                setNextIpAndPort()
                reply = createPacket()
                
                # Connect to next node
                try:
                    nextNodeS.send(str.encode(reply))
                except:
                    nextNodeS = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                    nextNodeS.connect((nextIp, int(nextPort)))
                    nextNodeS.send(str.encode(reply))
                    
                    
        except error:
            print('error: ' + error)
            break
    print("Connection Closed")
    previousNodeS.close()


def setVars(message):
    """
    This function will set all the config of the circuit
    Input: The message from the previous hop
    Output: None
    """
    global circutId, length, nodeTwoPort, replyData, numOfNode, nodeTwoPort, nodeThreePort, destClientPort, keys_map, nodeTwoIp, nodeThreeIp, destClientIp
    
    
    message = str(message).split('*&^*')
    length = message[0]
    numOfNode = message[1]
    circutId = message[2]
  
    decryptor = AESCipher(keys_map.get(circutId))
    
    if numOfNode == '1':
        nodeTwoIp = decryptor.decrypt(message[3].encode())
        nodeTwoPort = decryptor.decrypt(message[4].encode())
        nodeThreeIp = decryptor.decrypt(message[5].encode())
        nodeThreePort = decryptor.decrypt(message[6].encode())
        destClientIp = decryptor.decrypt(message[7].encode('latin-1'))
        destClientPort = decryptor.decrypt(message[8].encode('latin-1'))
        replyData = decryptor.decrypt(message[9].encode())

        nodeThreeIp = nodeThreeIp[2:-1]
        nodeThreePort = nodeThreePort[2:-1]
        destClientIp = destClientIp[2:-1]
        destClientPort = destClientPort[2:-1]
        replyData = replyData[2:-1]
        
    elif numOfNode == '2':
        nodeTwoIp = message[3]
        nodeTwoPort = message[4]
        nodeThreeIp = decryptor.decrypt(message[5].encode())
        nodeThreePort = decryptor.decrypt(message[6].encode())
        destClientIp = decryptor.decrypt(message[7].encode('latin-1'))
        destClientPort = decryptor.decrypt(message[8].encode('latin-1'))
        replyData = decryptor.decrypt(message[9].encode())
        
        destClientIp = destClientIp[2:-1]
        destClientPort = destClientPort[2:-1]
        replyData = replyData[2:-1]
        
    elif numOfNode == '3':
        nodeTwoIp = message[3]
        nodeTwoPort = message[4]
        nodeThreeIp = message[5]
        nodeThreePort = message[6]
        destClientIp = decryptor.decrypt(message[7].encode('latin-1'))
        destClientPort = decryptor.decrypt(message[8].encode('latin-1'))
        replyData = decryptor.decrypt(message[9].encode())

        replyData = replyData[2:-1]


def createPacket():
    """
    This function will create the packet message by the vars
    input: None
    Output: packet to send to the next node
    """
    global circutId, length, nodeTwoPort, replyData, numOfNode, nodeTwoPort, nodeThreePort, destClientPort, nodeTwoIp, nodeThreeIp, destClientIp
    packet = str(length) + '*&^*' + str(int(numOfNode)+1) + '*&^*' + str(circutId) + '*&^*' + str(nodeTwoIp) + '*&^*' + str(nodeTwoPort) + '*&^*' + str(nodeThreeIp)  + '*&^*' + str(nodeThreePort) + '*&^*' + str(destClientIp) + '*&^*' + str(destClientPort) + '*&^*' + str(replyData)
    print('packet -', packet)
    return packet


def setNextIpAndPort():
    """
    This function will set the whose the next port to send the message
    Input: None
    Output: None
    """
    global nextPort, nextIp
    if int(numOfNode) == 1:
        nextIp = nodeTwoIp
        nextPort = nodeTwoPort
    elif int(numOfNode) == 2:
        nextIp = nodeThreeIp
        nextPort = nodeThreePort
    elif int(numOfNode) == 3:
        nextIp = destClientIp
        nextPort = destClientPort

def sendKey(data, previousNodeS):
    """
    This function will send the symetric key to the client
    Input: Public key, previous node socket
    Output: None
    """
    global keys_map

    data = str(data).split('*&^*')
    key = createKey()

    circutKey = {str(data[1]): str(key)}
    keys_map.update(circutKey) # add the key to the dict of all the circuit id and theirs symetric keys
    public_key = RSA.importKey(data[0])

    chiperRSA = RSACipher()
    encryptedKey = chiperRSA.encrypt(key.encode(), public_key)
    previousNodeS.sendall(encryptedKey)

def createKey():
    """
    This function will create the symetric key
    Input: None
    Output: The symetric key
    """
    global key
    #Create symetric key 
    m = os.urandom(8)
    #Decode from bytes to ascii
    key = m.hex().encode().decode('ascii')
    
    return key


if __name__ == "__main__":
    main()