import base64
import hashlib
from Crypto import Random
from Crypto.Cipher import AES
from Crypto.PublicKey import RSA
from Crypto.Cipher import PKCS1_OAEP

class AESCipher(object):
    def __init__(self, key): 
        self.bs = AES.block_size
        #encrypt the key with sha256 encryption
        self.key = hashlib.sha256(key.encode()).digest()
        

    def encrypt(self, raw):
        raw = self._pad(raw)
        iv = Random.new().read(AES.block_size)
        cipher = AES.new(self.key, AES.MODE_CBC, iv)
        
        #encode the encrypted data with base64 encryption
        return base64.b64encode(iv + cipher.encrypt(raw.encode()))

    def decrypt(self, enc):
        enc = base64.b64decode(enc)
        iv = enc[:AES.block_size]
        cipher = AES.new(self.key, AES.MODE_CBC, iv)
        return self._unpad(cipher.decrypt(enc[AES.block_size:])).decode('utf-8', errors='ignore')

    # pad when the length of the raw is not multiple of the BLOCK_SIZE (128)
    def _pad(self, s):
        return s + (self.bs - len(s) % self.bs) * chr(self.bs - len(s) % self.bs)
    
    @staticmethod
    def _unpad(s):
        return s[:-ord(s[len(s)-1:])]


class RSACipher(object):
    def __init__(self):
        self.generate_keys() 
    
    def generate_keys(self):
        modulus_length = 1024
        r = Random.new()
        # Create the public and the private kets
        self.private_key = RSA.generate(modulus_length, r.read)
        self.public_key = self.private_key.publickey()
        

    def encrypt(self, a_message, public_key):
        # Encrypt the message using the public key
        encryptor = PKCS1_OAEP.new(public_key)
        encrypted_msg = encryptor.encrypt(a_message)

        # Encrypt the encrypted message using base64 encryption
        encoded_encrypted_msg = base64.b64encode(encrypted_msg).strip()
        return encoded_encrypted_msg

    def decrypt(self, encoded_encrypted_msg, private_key):
        decryptor = PKCS1_OAEP.new(private_key)
        # Decrypt the encoded encrypted message using base64 encryption
        decoded_encrypted_msg = base64.b64decode(encoded_encrypted_msg)

        # Decrypt the encrypted message using the private key
        decoded_decrypted_msg = decryptor.decrypt(decoded_encrypted_msg)
        
        return decoded_decrypted_msg