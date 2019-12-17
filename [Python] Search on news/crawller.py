from bs4 import BeautifulSoup
import requests
import codecs

def start_crawler():
    url = 'https://vnexpress.net'
    response = requests.get(url)
    soup = BeautifulSoup(response.text,'html.parser')
    str_ = soup.find(id = 'main_menu', class_ = "p_menu").find_all("a")
    a = []
    for x in str_:
        a.append(x.get('href'))#lay link trong moi thanh a
        
    del a[0:2]#loai bo ket qua trong va video
    del a[1]#3-2=1loai bo muc "goc nhin" vi tam thoi khong the xu li
    del a[-1]#loai bo rao vat, gia tri goc=19-2 =17, hoac -1 neu tinh nguoc lai
    i=1#so trang truy xuat de lay du lieu
    for item in a:
        for page in range(1,i+1,1):
            #o day co 2 loai link, i loai co duoi la -p+"so page" va 1 loai la /p+"so page"
            if(item[0] == "/"):
                
                url1 = url + item + '-p' + str(page) 
            else:
                url1 = item + '/p' + str(page) 
            print(url1)
            response1=requests.get(url1)
            print(response1)
            soup = BeautifulSoup(response1.text,"html.parser")
            containers = soup.find("section", class_ = "sidebar_1").find_all("article", class_ = "list_news")
            #voi moi bai bao, lay noi dung va link
            for item1 in containers:
                
                link = item1.h4.a.get("href")
                name = item1.h4.a.text
                response1=requests.get(link)
                soup = BeautifulSoup(response1.text,'html.parser')
                # neu la link video thi chuong trinh se bi loi=> su dung try
                try:
                    content = soup.find("section", class_ = "container").find_all("p", class_ = "Normal")
                    temp =''
                    for x in content:
                        temp += x.text
                    if temp != '':
                        #tao mot thu muc co ten "kq" de luu 
                        file1 = codecs.open('./demo/' + link[-12:-5] + ".txt", "w", "utf-8")
                        print(name + "\n" + temp, file = file1)
                        file1.close()
                        #print("success"+name)
                except:
                    pass
                    #print("error"+name)
			#break		
        Break

start_crawler()
