import cv2
import os



image_path = "C:/Users/zz525/iMeta2023/images/"
label_path = "C:/Users/zz525/iMeta2023/labels2/"

img_list =  os.listdir(image_path)
label_list =  os.listdir(image_path)

print(len(img_list))
print(len(label_list))


img_list.sort()
label_list.sort()
i = 0
for img in img_list:
    img_file = image_path + img
    label_file = label_path + img.replace('jpg','txt')
    with open(label_file) as label:
        datas = label.readlines()
        for data in datas:
            if data == None:
                pass
            else:
                c,x,y,w,h = data.split(" ")
                c = int(float(c) * 500)
                x = int(float(x) * 500)
                y = int(float(y) * 500)
                w = int(float(w) * 500)
                h = int(float(h) * 500)
 
            img_ = cv2.imread(img_file, cv2.IMREAD_ANYCOLOR)
            cv2.rectangle(img_, )
            
            
            


           