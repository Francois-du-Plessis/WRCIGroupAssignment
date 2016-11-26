from PIL import Image
import os

for filename in os.listdir('./'):
    if filename.endswith(".JPG"): 
        image = Image.open(filename)
        foo = image.resize((int(100),int(100)),Image.ANTIALIAS)
        foo.save('./SmallerImages/' + filename,quality=100,optimize=True)