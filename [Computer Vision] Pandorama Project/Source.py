import os
import cv2

#check your cv2 version... becarefull different versions has different functions... 
#print(cv2.__version__) 

def stitching():
#the folder has images which need stitching
    path = "./input/"
    imagelist=os.listdir(path)
    images = [] #image list

    for x in imagelist:
        images.append(cv2.imread(path+x))
        
    stitcher = cv2.Stitcher.create() #notice... this function depends on your cv2 version

    stitched = stitcher.stitch(images)
    
    #there are two return results
    if (stitched[0] == 0):#stitched[0] this one is status code... check online for more information
        cv2.imwrite("./output/result.png", stitched[1])#stitched[1] this one is the image result
        cv2.imshow("Stitched", stitched[1])
        return 1
    else:
        #print("[INFO] image stitching failed ({})".format(stitched[0]))
        return 0