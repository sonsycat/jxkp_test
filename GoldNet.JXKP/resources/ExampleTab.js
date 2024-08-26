function createExampleTab(id, url,title,ico){
    var win, tab, hostName, exampleName, node;
    var tabItems = ExampleTabs.items; //获取已经生成的tabpanl
    var index = tabItems.length;
    if (index > 8) {
        Ext.Msg.alert('提示', '标签数量过多,请关闭不必要的标签项');
        return;
    }
    if(id == "-"){
        id = Ext.id();
        url = "/ERP"+url;
    }
    

    hostName = window.location.protocol+"//"+window.location.host;
    exampleName = url.substr(9);
    
    tab = ExampleTabs.add(new Ext.Panel({
        id: id,
        title: title,
        iconCls: 'icon-' + ico.toString().toLowerCase(),
        autoLoad: {
            showMask: true,
            maskMsg: '页面加载中... ',
            scripts: true,
            mode: "iframe",
            url:url+ ((url.indexOf('?')>0)?('&') :('?'))+'pageid='+id
        },
        listeners: {
            deactivate: {
                fn: function(el) {
                    if (this.sWin && this.sWin.isVisible()) {
                        this.sWin.hide();
                    }
                }
            }
        },
        
        autoDestroy:true,
        destroy:function (){//销毁tabpanel
            if(this.fireEvent("destroy",this)!=false){
                this.el.remove();
                tab = null;
                if(Ext.isIE){
                    CollectGarbage();
                }
            }
        },
        closable: true
    }));
    
    tab.sWin = win;
    ExampleTabs.setActiveTab(tab);
    
 
}