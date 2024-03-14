from genericpath import exists
from glob import glob
from logging import exception
import socket
from _thread import *
from random import randint
from Cipher import *
import os
import sqlite3 as sql

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

listen_port = 8888 
ip = ''

circutId = 0

port_node_list = []
ip_node_list = []
online_users = []
con = sql.connect("users.db", check_same_thread=False) # Create connection with the user database

try:
    sock.bind((ip, listen_port))

except socket.error as e:
    print("the error is - " + str(e))

sock.listen(2)

def main():
    print('Waiting for client connection request')
    while True:
        listenS, addr = sock.accept()
        print("Connected to: ", addr)
        start_new_thread(client_thread, (listenS, addr[0], ))
        
def client_thread(clientS : socket, ip_addr):
    global online_users
    """
    This function will deal with the client or the node
    Input: Socket of the client or the node, his ip address
    Output: None
    """
    symetircKey = ''
    username = ''
    try:
        while True:
            reply = clientS.recv(1024).decode()
            print('reply', reply)
            
            if reply == '1': # Send node route
                nodeRoute = getNodesRoute()
                clientS.sendall(str.encode(nodeRoute))
            elif reply == '2': # Send the circutId
                circutId = getCircutId()
                clientS.sendall(str.encode(str(circutId)))
            elif reply == '3': # Close the connection
                clientS.close()
            elif reply == '4': # Insert the node to the nodes list 
                node_port = "8800"
                if doesNodeExists(ip_addr, node_port):
                    clientS.sendall(str.encode('2'))
                else:
                    insert_node(ip_addr, node_port)
                    clientS.sendall(str.encode('1'))
            elif reply == '5': # Create and send the syemtric key
                data = clientS.recv(1024).decode()
                symetircKey = getKey(data, clientS)
            elif reply == '6': # Login
                data = clientS.recv(1024).decode()
                data = str(data).split('()')
                print(data[0] + ' ' + data[1])
                status, username = login(data[0], data[1], symetircKey)
                if status == '1':
                    online_users.append(username)
                clientS.sendall(str.encode(status))
            elif reply == '7': # Delete the node from the node list
                deleteNode(ip_addr)
            elif reply == '8': # Register
                data = clientS.recv(1024).decode()
                data = str(data).split('()')
                print(data[0] + ' ' + data[1] + ' ' + data[2])
                status, username = register(data[0], data[1], data[2], ip_addr, symetircKey)
                if status == '1':
                    online_users.append(username)
                clientS.sendall(str.encode(status))
            elif reply == '9': # Get friend list of the user
                friendList = getFriends(username)
                clientS.sendall(str.encode(friendList))
            elif reply == '10':
                friendUsername = clientS.recv(1024).decode()
                status = addFriend(username, friendUsername)
                clientS.sendall(str.encode(status))
            elif reply == '11':
                friendUsername = clientS.recv(1024).decode()
                ip = getIpOfUsername(friendUsername)
                clientS.sendall(str.encode(ip))
            else:
                online_users.remove(username)
                clientS.close()

    except error and ConnectionResetError as err:
        online_users.remove(username)
        clientS.close()
        print('error is', err)

def getIpOfUsername(username):
    cur = con.cursor()
    ip = [row[0] for row in cur.execute("SELECT ip FROM Users WHERE username = '%s'" %username)]
    print('dest client ip', ip)
    return ip
    
def addFriend(username, friendUsername):
    cur = con.cursor()
    
    if username == friendUsername:
        return '3'
    
    users = [row[0] for row in cur.execute("SELECT username FROM Users WHERE username = '%s'" %friendUsername)]
    if len(users) == 0: # Check if username exists
        return '2'

    cur = con.cursor()
    rows = [row[0] for row in cur.execute("SELECT friends FROM Users WHERE username = '%s'" %username)]
    friendList = rows[0].split(", ")
    friendList.pop() # remove the last element (empty)
    if friendUsername in friendList: # Check if user already friend
        return '1'

    friendsStr = rows[0] + friendUsername + ', '
    statement = "UPDATE Users SET friends = '" + friendsStr + "' WHERE username = '" + username + "'"  
    cur.execute(statement)
    con.commit()


    # Add the user to the list friends of the friend user
    cur = con.cursor()
    rows = [row[0] for row in cur.execute("SELECT friends FROM Users WHERE username = '%s'" %friendUsername)]
    friendList = rows[0].split(", ")
    friendList.pop() # remove the last element (empty)

    friendsStr = rows[0] + username + ', '
    statement = "UPDATE Users SET friends = '" + friendsStr + "' WHERE username = '" + friendUsername + "'"  
    cur.execute(statement)
    con.commit()
 
    return '0'

def getFriends(username):
    # 1 - online, 0 - offline
    cur = con.cursor()
    rows = [row[0] for row in cur.execute("SELECT friends FROM Users WHERE username = '%s'" %username)]
    
    if(len(rows) == 0):
        return ""
    
    print(rows)
    print(rows[0])
    friendList = rows[0].split(", ")
    friendListStr = ""
    print('online users', online_users)
    for friend in friendList:
        if friend in online_users:
            friendListStr += friend + '1' + ', '
        else:
            friendListStr += friend + '0' + ', ' 

    print('friend list str', friendListStr)
    return friendListStr
    

def register(username, password, mail, ip, key):
    
    """
    This function will register the user if he is not exists
    Input: username, password, email, ip of the client, symetric key to decrypt
    Output: Status
    """
    decryptor = AESCipher(key)
    username = decryptor.decrypt(username.encode())
    password = decryptor.decrypt(password.encode())
    mail = decryptor.decrypt(mail.encode())
    cur = con.cursor()
    rows = [row[0] for row in cur.execute("SELECT username FROM Users WHERE username = '%s'" %username)]
    print(rows)
    if len(rows) != 0: # Check if username already exists
        return '2', username

    cur = con.cursor()
    rows = [row[0] for row in cur.execute("SELECT mail FROM Users WHERE mail = '%s'" %mail)]
    print(rows)
    if len(rows) != 0: # Check if mail already exists
        return '3', username
    
    # Insert the user to the database
    statement = "INSERT INTO Users VALUES ('" + username +"','"+ password +"', '"+ mail +"', '" + ip + "', '')" 
    cur.execute(statement)
    con.commit()

    return '1', username

def login(username, password, key):
    """
    This function will check if the username and the password are correct
    Input: username, password, symetric key to decrypt
    Output: Status
    """
    decryptor = AESCipher(key)
    username = decryptor.decrypt(username.encode())
    password = decryptor.decrypt(password.encode())

    cur = con.cursor()
    rows = [row[0] for row in cur.execute("SELECT username and password FROM 'Users' WHERE username = '" + username + "' and password = '" + password + "'")]
    print(rows)
    if len(rows) == 0: # Check if userExists and password valid
        return '2', username
    return '1', username

def getKey(data, clientS: socket):
    """
    This function will send the symetric key to the client
    Input: Public key, previous node socket
    Output: None
    """
    key = createKey()
    public_key = RSA.importKey(data)
    chiperRSA = RSACipher()
    encryptedKey = chiperRSA.encrypt(key.encode(), public_key)
    clientS.sendall(encryptedKey)
    return key

def createKey():
    """
    This function will create the symetric key
    Input: None
    Output: The symetric key
    """
    #Create symetric key 
    m = os.urandom(8)
    #Decode from bytes to ascii
    key = m.hex().encode().decode('ascii')
    return key

def getNodesRoute():
    """
    This function will generate a node route
    Input: None
    Output: Node route
    """
    global ip_node_list, port_node_list
    
    firstNode = randint(0,2)
    secondNode = randint(0,2)
    while firstNode == secondNode:
        secondNode = randint(0,2)
    thirdNode = randint(0,2)
    while thirdNode == secondNode or thirdNode == firstNode:
        thirdNode = randint(0, 2)
    
    nodeRoute = ''.join(ip_node_list[firstNode]) + '#' + ''.join(port_node_list[firstNode]) + '#' +''.join(ip_node_list[secondNode]) + '#' +''.join(port_node_list[secondNode])+ '#' +''.join(ip_node_list[thirdNode])+ '#' +''.join(port_node_list[thirdNode])
    
    return nodeRoute

def getCircutId():
    """
    This function will create new circut and return his number
    Input: None
    Output: Circut number
    """
    global circutId
    circutId+=1
    return circutId


def insert_node(ip, port):
    """
    This function will insert the node to the nodes list
    Input: ip and port of the node
    Output: None 
    """
    global ip_node_list, port_node_list

    ip_node_list.append(ip)
    port_node_list.append(port)


def doesNodeExists(ip, port):
    """
    This function will check if the node already exists in the node list
    Input: ip and port of the node
    Output: True if exists else False
    """
    global ip_node_list
    ipExists = False
    
    ipExists = ip in ip_node_list
    print('ip',ip)
    print('port',port)
    return ipExists

def deleteNode(ip):
    """
    This function will remove the node from the nodes list
    Input: Node's ip
    Output: None
    """
    global ip_node_list, port_node_list
    
    index = ip_node_list.index(ip)
    ip_node_list.remove(ip)
    port_node_list.pop(index)

if __name__ == "__main__":
    main()