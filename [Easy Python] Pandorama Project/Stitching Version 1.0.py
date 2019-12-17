import wx
import time
import Source

class Example(wx.Frame):

    def __init__(self, parent, title):
        super(Example, self).__init__(parent, title=title, size=(900, 600))
       	self.Centre()
        self.listFile = []
       	self.panel = wx.Panel(self)

        vbox = wx.BoxSizer(wx.VERTICAL) 
        hbox1 = wx.BoxSizer(wx.HORIZONTAL)
        
        self.btnStart = wx.Button(self.panel, -1, "Start Stitching")
        self.btnAbout = wx.Button(self.panel, -1, "About")
        self.btnStart.SetPosition((20,150))
        self.btnAbout.SetPosition((20,400))
        self.btnStart.SetSize((300,100))
        self.btnAbout.SetSize((300,100))
        self.btnStart.Bind(wx.EVT_BUTTON, self.gettingStart)
        self.btnAbout.Bind(wx.EVT_BUTTON, self.gettingAbout)
       	image = wx.Image('logo.png', wx.BITMAP_TYPE_ANY).ConvertToBitmap()
        imageBitmap = wx.StaticBitmap(self.panel, wx.ID_ANY, wx.Bitmap(image))
        imageBitmap.SetPosition((330,5))
        
        hbox1.Add(self.btnStart)
        hbox1.Add(self.btnAbout)
        vbox.Add(hbox1)
        
 
    def OnQuit(self, e):
        self.Close()
        
    def gettingStart(self, event):
        start = time.time()
        a=Source.stitching()
        if(a==1):
            end = time.time()   
            dlg = wx.MessageDialog(self, "Stitching Success: " + str(end - start) +"Second(s)", "Finished", wx.OK)
            val = dlg.ShowModal()
            dlg.Show()
        else:
            dlg = wx.MessageDialog(self, "Failed","OK", wx.OK)
            val = dlg.ShowModal()
            dlg.Show()
    def gettingAbout(self,event):
        dlg = wx.MessageDialog(self, "Made by:\nPhạm Bá Công\nTô Việt Anh\nNguyễn Phạm Long Duy","OK", wx.OK)
        val = dlg.ShowModal()
        dlg.Show()
def main():

    app = wx.App()
    ex = Example(None, title = "Image Stitching Version 1.0")
    ex.Show()
    app.MainLoop()

if __name__ == '__main__':
    main()