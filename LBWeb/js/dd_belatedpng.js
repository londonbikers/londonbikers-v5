var DD_belatedPNG={ns:"DD_belatedPNG",imgSize:{},delay:10,nodesFixed:0,createVmlNameSpace:function(){if(document.namespaces&&!document.namespaces[this.ns]){document.namespaces.add(this.ns,"urn:schemas-microsoft-com:vml")}},createVmlStyleSheet:function(){var a,b;a=document.createElement("style");a.setAttribute("media","screen");document.documentElement.firstChild.insertBefore(a,document.documentElement.firstChild.firstChild);if(a.styleSheet){a=a.styleSheet;a.addRule(this.ns+"\\:*","{behavior:url(#default#VML)}");a.addRule(this.ns+"\\:shape","position:absolute;");a.addRule("img."+this.ns+"_sizeFinder","behavior:none; border:none; position:absolute; z-index:-1; top:-10000px; visibility:hidden;");this.screenStyleSheet=a;b=document.createElement("style");b.setAttribute("media","print");document.documentElement.firstChild.insertBefore(b,document.documentElement.firstChild.firstChild);b=b.styleSheet;b.addRule(this.ns+"\\:*","{display: none !important;}");b.addRule("img."+this.ns+"_sizeFinder","{display: none !important;}")}},readPropertyChange:function(){var a,b,c;a=event.srcElement;if(!a.vmlInitiated){return}if(event.propertyName.search("background")!=-1||event.propertyName.search("border")!=-1){DD_belatedPNG.applyVML(a)}if(event.propertyName=="style.display"){b=a.currentStyle.display=="none"?"none":"block";for(c in a.vml){if(a.vml.hasOwnProperty(c)){a.vml[c].shape.style.display=b}}}if(event.propertyName.search("filter")!=-1){DD_belatedPNG.vmlOpacity(a)}},vmlOpacity:function(a){if(a.currentStyle.filter.search("lpha")!=-1){var b=a.currentStyle.filter;b=parseInt(b.substring(b.lastIndexOf("=")+1,b.lastIndexOf(")")),10)/100;a.vml.color.shape.style.filter=a.currentStyle.filter;a.vml.image.fill.opacity=b}},handlePseudoHover:function(a){setTimeout(function(){DD_belatedPNG.applyVML(a)},1)},fix:function(a){if(this.screenStyleSheet){var b,c;b=a.split(",");for(c=0;c<b.length;c++){this.screenStyleSheet.addRule(b[c],"behavior:expression(DD_belatedPNG.fixPng(this))")}}},applyVML:function(a){a.runtimeStyle.cssText="";this.vmlFill(a);this.vmlOffsets(a);this.vmlOpacity(a);if(a.isImg){this.copyImageBorders(a)}},attachHandlers:function(a){var b,c,d,e,f,g;b=this;c={resize:"vmlOffsets",move:"vmlOffsets"};if(a.nodeName=="A"){e={mouseleave:"handlePseudoHover",mouseenter:"handlePseudoHover",focus:"handlePseudoHover",blur:"handlePseudoHover"};for(f in e){if(e.hasOwnProperty(f)){c[f]=e[f]}}}for(g in c){if(c.hasOwnProperty(g)){d=function(){b[c[g]](a)};a.attachEvent("on"+g,d)}}a.attachEvent("onpropertychange",this.readPropertyChange)},giveLayout:function(a){a.style.zoom=1;if(a.currentStyle.position=="static"){a.style.position="relative"}},copyImageBorders:function(a){var b,c;b={borderStyle:true,borderWidth:true,borderColor:true};for(c in b){if(b.hasOwnProperty(c)){a.vml.color.shape.style[c]=a.currentStyle[c]}}},vmlFill:function(a){if(!a.currentStyle){return}else{var b,c,d,e,f,g;b=a.currentStyle}for(e in a.vml){if(a.vml.hasOwnProperty(e)){a.vml[e].shape.style.zIndex=b.zIndex}}a.runtimeStyle.backgroundColor="";a.runtimeStyle.backgroundImage="";c=true;if(b.backgroundImage!="none"||a.isImg){if(!a.isImg){a.vmlBg=b.backgroundImage;a.vmlBg=a.vmlBg.substr(5,a.vmlBg.lastIndexOf('")')-5)}else{a.vmlBg=a.src}d=this;if(!d.imgSize[a.vmlBg]){f=document.createElement("img");d.imgSize[a.vmlBg]=f;f.className=d.ns+"_sizeFinder";f.runtimeStyle.cssText="behavior:none; position:absolute; left:-10000px; top:-10000px; border:none; margin:0; padding:0;";g=function(){this.width=this.offsetWidth;this.height=this.offsetHeight;d.vmlOffsets(a)};f.attachEvent("onload",g);f.src=a.vmlBg;f.removeAttribute("width");f.removeAttribute("height");document.body.insertBefore(f,document.body.firstChild)}a.vml.image.fill.src=a.vmlBg;c=false}a.vml.image.fill.on=!c;a.vml.image.fill.color="none";a.vml.color.shape.style.backgroundColor=b.backgroundColor;a.runtimeStyle.backgroundImage="none";a.runtimeStyle.backgroundColor="transparent"},vmlOffsets:function(a){var b,c,d,e,f,g,h,i,j,k,l;b=a.currentStyle;c={W:a.clientWidth+1,H:a.clientHeight+1,w:this.imgSize[a.vmlBg].width,h:this.imgSize[a.vmlBg].height,L:a.offsetLeft,T:a.offsetTop,bLW:a.clientLeft,bTW:a.clientTop};d=c.L+c.bLW==1?1:0;e=function(a,b,c,d,e,f){a.coordsize=d+","+e;a.coordorigin=f+","+f;a.path="m0,0l"+d+",0l"+d+","+e+"l0,"+e+" xe";a.style.width=d+"px";a.style.height=e+"px";a.style.left=b+"px";a.style.top=c+"px"};e(a.vml.color.shape,c.L+(a.isImg?0:c.bLW),c.T+(a.isImg?0:c.bTW),c.W-1,c.H-1,0);e(a.vml.image.shape,c.L+c.bLW,c.T+c.bTW,c.W,c.H,1);f={X:0,Y:0};if(a.isImg){f.X=parseInt(b.paddingLeft,10)+1;f.Y=parseInt(b.paddingTop,10)+1}else{for(j in f){if(f.hasOwnProperty(j)){this.figurePercentage(f,c,j,b["backgroundPosition"+j])}}}a.vml.image.fill.position=f.X/c.W+","+f.Y/c.H;g=b.backgroundRepeat;h={T:1,R:c.W+d,B:c.H,L:1+d};i={X:{b1:"L",b2:"R",d:"W"},Y:{b1:"T",b2:"B",d:"H"}};if(g!="repeat"||a.isImg){k={T:f.Y,R:f.X+c.w,B:f.Y+c.h,L:f.X};if(g.search("repeat-")!=-1){l=g.split("repeat-")[1].toUpperCase();k[i[l].b1]=1;k[i[l].b2]=c[i[l].d]}if(k.B>c.H){k.B=c.H}a.vml.image.shape.style.clip="rect("+k.T+"px "+(k.R+d)+"px "+k.B+"px "+(k.L+d)+"px)"}else{a.vml.image.shape.style.clip="rect("+h.T+"px "+h.R+"px "+h.B+"px "+h.L+"px)"}},figurePercentage:function(a,b,c,d){var e,f;f=true;e=c=="X";switch(d){case"left":case"top":a[c]=0;break;case"center":a[c]=.5;break;case"right":case"bottom":a[c]=1;break;default:if(d.search("%")!=-1){a[c]=parseInt(d,10)/100}else{f=false}}a[c]=Math.ceil(f?b[e?"W":"H"]*a[c]-b[e?"w":"h"]*a[c]:parseInt(d,10));if(a[c]%2===0){a[c]++}return a[c]},fixPng:function(a){a.style.behavior="none";var b,c,d,e,f;if(a.nodeName=="BODY"||a.nodeName=="TD"||a.nodeName=="TR"){return}a.isImg=false;if(a.nodeName=="IMG"){if(a.src.toLowerCase().search(/\.png$/)!=-1){a.isImg=true;a.style.visibility="hidden"}else{return}}else if(a.currentStyle.backgroundImage.toLowerCase().search(".png")==-1){return}b=DD_belatedPNG;a.vml={color:{},image:{}};c={shape:{},fill:{}};for(e in a.vml){if(a.vml.hasOwnProperty(e)){for(f in c){if(c.hasOwnProperty(f)){d=b.ns+":"+f;a.vml[e][f]=document.createElement(d)}}a.vml[e].shape.stroked=false;a.vml[e].shape.appendChild(a.vml[e].fill);a.parentNode.insertBefore(a.vml[e].shape,a)}}a.vml.image.shape.fillcolor="none";a.vml.image.fill.type="tile";a.vml.color.fill.on=false;b.attachHandlers(a);b.giveLayout(a);b.giveLayout(a.offsetParent);a.vmlInitiated=true;b.applyVML(a)}};try{document.execCommand("BackgroundImageCache",false,true)}catch(r){}DD_belatedPNG.createVmlNameSpace();DD_belatedPNG.createVmlStyleSheet()