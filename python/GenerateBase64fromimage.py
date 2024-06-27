import base64
import os

def read_file(filename):
    with open(filename, 'rb') as f:
        return f.read()

def encode_base64(data):
    return base64.b64encode(data).decode('utf-8')

if __name__ == '__main__':
    
    
    image_data = read_file(input())
    
    base64_data = encode_base64(image_data)
    

    

# Input string
input_path = R"C:\Users\mahmo\OneDrive\Pictures\khalod.jpg"

# Get the file name from the path
file_name_with_extension = os.path.basename(input_path)

# Remove the file extension
file_name = os.path.splitext(file_name_with_extension)[0]



with open(file_name, "w") as file:
    file.write(base64_data)


print("finished")
