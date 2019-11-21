import wx
import Search
import codecs
import time
#import crawller


class Example(wx.Frame):

    def __init__(self, parent, title):
        super(Example, self).__init__(parent, title=title, size=(960, 720))
       	self.Centre()
        self.listFile = []
       	self.panel = wx.Panel(self)

        vbox = wx.BoxSizer(wx.VERTICAL) 
        hbox1 = wx.BoxSizer(wx.HORIZONTAL)

        self.t1 = wx.TextCtrl(self.panel, size = ((480,30)))

        self.t1.SetPosition((240,180))
        hbox1.Add(self.t1,1,wx.EXPAND|wx.ALIGN_LEFT|wx.ALL,5)

        self.t1.Bind(wx.EVT_TEXT,self.OnKeyTyped)
        self.btnDis = wx.Button(self.panel, -1, "Get New Data")
        self.btnCosine = wx.Button(self.panel, -1, "Search by Cosine")
        self.btnCosine.SetPosition((400,220))
        self.btnDis.SetPosition((20,20))
        self.btnCosine.SetSize((150,25))
        self.btnDis.SetSize((150,25))
        self.btnDis.Bind(wx.EVT_BUTTON, self.getData)
        self.btnCosine.Bind(wx.EVT_BUTTON, self.getSearchCos)
       	self.t2 = wx.TextCtrl(self.panel, style = wx.TE_MULTILINE, size = ((460,400)))
       	self.t2.SetPosition((480,300))
       	image = wx.Image('logo.png', wx.BITMAP_TYPE_ANY).ConvertToBitmap()
        imageBitmap = wx.StaticBitmap(self.panel, wx.ID_ANY, wx.Bitmap(image))
        imageBitmap.SetPosition((330,5))
        self.mlist = []
       	self.listBox = wx.ListBox(self.panel, wx.ID_ANY, wx.Point(10,300), wx.Size(460, 400), self.mlist, style = wx.LB_HSCROLL)
       	self.listBox.Bind(wx.EVT_LISTBOX, self.get)
        self.listBox.Hide()
        self.t2.Hide()
       
       	hbox1.Add(self.t2)
        hbox1.Add(self.btnCosine)
        hbox1.Add(self.btnDis)
        vbox.Add(hbox1) 
 
    def OnQuit(self, e):
        self.Close()
    def OnKeyTyped(self, event): 
    	pass
    def getSearchCos(self, event):
        start = time.time()
        text = self.t1.GetValue()
        text = str(text)
        if(text != "" and len(text.strip()) != 0):
            cosine = Search.finalCosine(text)
            self.listFile = [key for key in cosine.keys()]

            for item in self.listFile:
                file = codecs.open('./kq/' + item, 'r', 'utf-8')
                line = file.readline()            
                while(len(line) <= 2):
                    line = file.readline()
                file.close()
                self.mlist.append(line.strip())
            self.listBox.Set(self.mlist)
            self.mlist = []
            self.listBox.Show()
            end = time.time()
            dlg = wx.MessageDialog(self, "Search Done!\nTime Processing: " + str(int(end - start)) +" seconds", "Finished", wx.OK)
            val = dlg.ShowModal()
            dlg.Show()

        elif(text == ""):
            dlg = wx.MessageDialog(self, "Query can not be empty", "Empty Query Error", wx.OK)
            val = dlg.ShowModal()
            dlg.Show()
        else:
            dlg = wx.MessageDialog(self, "Query can not contain only spaces", "Spaces Only Query Error",wx.OK)
            val = dlg.ShowModal()
            dlg.Show()
    
    def getData(self, event):
        start = time.time()
        #crawller.start_crawler()
        Search.build_inverted_tf()
        end = time.time()   
        dlg = wx.MessageDialog(self, "Data Updated!\nTime Processing About: " + str(int((end - start)/60)) +" minutes", "Finished", wx.OK)
        val = dlg.ShowModal()
        dlg.Show()
        
    def get(self, e):
        i = self.listFile[self.listBox.GetSelection()]
        file = codecs.open('./kq/' + i, 'r', 'utf-8')
        texts = file.read()
        file.close()
        self.t2.SetValue(texts)
        self.t2.Show()

def main():

    app = wx.App()
    ex = Example(None, title = "Simple Search")
    ex.Show()
    app.MainLoop()

if __name__ == '__main__':
    main()