$(document).ready(function() {
    if (typeof(_gat) == 'object') {
        $('a').each(function() {
            var uri, filetype;
            uri = $(this).attr('href');
            if (uri) {
                if (uri.match(/^mailto/i)) {
                    $(this).click(function() {
                        ahTracker._trackEvent('mailto', 'click', document.domain + ' | ' + uri.substr(7));
                        siteTracker._trackEvent('mailto', 'click', uri.substr(7))
                    })
                } else if (uri.match(/^https?:/i)) {
                    $(this).click(function() {
                        ahTracker._trackEvent('external', 'click', document.domain + ' | ' + uri);
                        siteTracker._trackEvent('external', 'click', uri)
                    })
                } else if (uri.match(/\.[a-z]+$/i) && !uri.match(/\.(aspx?|html?)([\?#].*)?$/i)) {
                    filetype = uri.substr(uri.length - 3);
                    $(this).click(function() {
                        ahTracker._trackEvent('downloads', filetype.toUpperCase(), document.domain + ' | ' + uri);
                        siteTracker._trackEvent('downloads', filetype.toUpperCase(), uri)
                    })
                }
            }
        })
    }
	var url = window.location.href;
    var title = document.title;
	if ((window.sidebar && /Firefox/i.test(navigator.userAgent)) || (window.opera && window.print)) {
	//alert('1er, ' + title +',' + url);
	 // Firefox version >= 23 and Opera Hotlist
      var myBookmark = $("#serviceLinksBox img[src*='iconSL-bookmark.gif']").parent();
	  
	  $(myBookmark).attr({
        href: url,
        title: title,
        rel: 'sidebar'
      });
	  setTimeout(function (){
		  $("#serviceLinksBox img[src*='iconSL-bookmark.gif']").parent().click();
		  }, 500);
	  
    } 
});
$.urlParam = function(name) {
    var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (!results) {
        return 0
    }
    return results[1] || 0
};
(function($) {
    $.extend($.fn, {
        prepareBranches: function(settings) {
            this.filter(":not(.)").find(">ul").hide();
            return this.filter(":has(>ul)")
        },
        applyClasses: function(settings, toggler) {
            this.find("div.").click(toggler)
        },
        treeview: function(settings) {
            function toggler() {
                $(this).parent().end().find(">ul").heightToggle(settings.animated, settings.toggle)
            }
            this.data("toggler", toggler);
            var branches = this.find("li").prepareBranches(settings);
            var current = this.find("a").filter(function() {
                return this.href.toLowerCase() == location.href.toLowerCase()
            });
            if (current.length) {
                var items = current.addClass("selected").parents("ul, li").add(current.next()).show()
            }
            return this
        }
    });
    $.treeview = {}
})(jQuery);

function navDrop() {
    $("#navigation li").hover(function() {
        $(this).addClass("showUl")
    }, function() {
        $(this).removeClass("showUl")
    })
};

function navHasChild() {
    $("#navigation li ul li").each(function() {
        var theChild = $(this).find("ul");
        if (theChild.length > 0) {
            $(this).addClass("hasChild")
        }
    })
};

function leftmenu() {
    $('#subNav').treeview({
        collapsed: true,
        persist: "location",
        unique: true
    })
};
$(document).ready(function() {
    jsEnabled();
    corners();
    boxLink();
    blockQuote();
    serviceLinks();
    textLogo();
    navDrop();
    navHasChild();
    leftmenu()
});

function jsEnabled() {
    $("body").addClass("jsenabled")
};

function corners() {
    $("#header").append('<div class="corner24BL corner"><div class="shape01 dot"></div><div class="shape02 dot"></div><div class="shape03 dot"></div><div class="shape04 dot"></div><div class="shape05 dot"></div><div class="shape06 dot"></div><div class="shape07 dot"></div><div class="shape08 dot"></div><div class="shape09 dot"></div><div class="shape10 dot"></div><div class="shape11 dot"></div><div class="shape12 dot"></div><div class="shape13 dot"></div><div class="dot01 dot"></div><div class="dot02 dot"></div><div class="dot03 dot"></div><div class="dot04 dot"></div><div class="dot05 dot"></div><div class="dot06 dot"></div><div class="dot07 dot"></div><div class="dot08 dot"></div><div class="dot09 dot"></div><div class="dot10 dot"></div><div class="dot11 dot"></div><div class="dot12 dot"></div><div class="dot13 dot"></div><div class="dot14 dot"></div><div class="dot15 dot"></div><div class="dot16 dot"></div><div class="dot17 dot"></div><div class="dot18 dot"></div><div class="dot19 dot"></div><div class="dot20 dot"></div><div class="dot21 dot"></div><div class="dot22 dot"></div><div class="dot23 dot"></div><div class="dot24 dot"></div><div class="dot25 dot"></div><div class="dot26 dot"></div><div class="dot27 dot"></div><div class="dot28 dot"></div><div class="dot29 dot"></div><div class="dot30 dot"></div><div class="dot31 dot"></div><div class="dot32 dot"></div><div class="dot33 dot"></div><div class="dot34 dot"></div><div class="dot35 dot"></div><div class="dot36 dot"></div><div class="dot37 dot"></div><div class="dot38 dot"></div><div class="dot39 dot"></div><div class="dot40 dot"></div><div class="dot41 dot"></div><div class="dot42 dot"></div><div class="dot43 dot"></div><div class="dot44 dot"></div><div class="dot45 dot"></div></div>');
    $(".rounded .box").append('<div class="corner corner10TL"><div class="shape01 dot"></div><div class="shape02 dot"></div><div class="shape03 dot"></div><div class="dot01 dot"></div><div class="dot02 dot"></div><div class="dot03 dot"></div><div class="dot04 dot"></div><div class="dot05 dot"></div><div class="dot06 dot"></div><div class="dot07 dot"></div><div class="dot08 dot"></div><div class="dot09 dot"></div><div class="dot10 dot"></div><div class="dot11 dot"></div><div class="dot12 dot"></div><div class="dot13 dot"></div><div class="dot14 dot"></div><div class="dot15 dot"></div></div><div class="corner corner10TR"><div class="shape01 dot"></div><div class="shape02 dot"></div><div class="shape03 dot"></div><div class="dot01 dot"></div><div class="dot02 dot"></div><div class="dot03 dot"></div><div class="dot04 dot"></div><div class="dot05 dot"></div><div class="dot06 dot"></div><div class="dot07 dot"></div><div class="dot08 dot"></div><div class="dot09 dot"></div><div class="dot10 dot"></div><div class="dot11 dot"></div><div class="dot12 dot"></div><div class="dot13 dot"></div><div class="dot14 dot"></div><div class="dot15 dot"></div></div><div class="corner corner10BR"><div class="shape01 dot"></div><div class="shape02 dot"></div><div class="shape03 dot"></div><div class="dot01 dot"></div><div class="dot02 dot"></div><div class="dot03 dot"></div><div class="dot04 dot"></div><div class="dot05 dot"></div><div class="dot06 dot"></div><div class="dot07 dot"></div><div class="dot08 dot"></div><div class="dot09 dot"></div><div class="dot10 dot"></div><div class="dot11 dot"></div><div class="dot12 dot"></div><div class="dot13 dot"></div><div class="dot14 dot"></div><div class="dot15 dot"></div></div><div class="corner corner10BL"><div class="shape01 dot"></div><div class="shape02 dot"></div><div class="shape03 dot"></div><div class="dot01 dot"></div><div class="dot02 dot"></div><div class="dot03 dot"></div><div class="dot04 dot"></div><div class="dot05 dot"></div><div class="dot06 dot"></div><div class="dot07 dot"></div><div class="dot08 dot"></div><div class="dot09 dot"></div><div class="dot10 dot"></div><div class="dot11 dot"></div><div class="dot12 dot"></div><div class="dot13 dot"></div><div class="dot14 dot"></div><div class="dot15 dot"></div></div>');
    $(".rounded  p a em").append('<div class="corner corner10TL"><div class="shape01 dot"></div><div class="shape02 dot"></div><div class="shape03 dot"></div><div class="dot01 dot"></div><div class="dot02 dot"></div><div class="dot03 dot"></div><div class="dot04 dot"></div><div class="dot05 dot"></div><div class="dot06 dot"></div><div class="dot07 dot"></div><div class="dot08 dot"></div><div class="dot09 dot"></div><div class="dot10 dot"></div><div class="dot11 dot"></div><div class="dot12 dot"></div><div class="dot13 dot"></div><div class="dot14 dot"></div><div class="dot15 dot"></div></div><div class="corner corner10TR"><div class="shape01 dot"></div><div class="shape02 dot"></div><div class="shape03 dot"></div><div class="dot01 dot"></div><div class="dot02 dot"></div><div class="dot03 dot"></div><div class="dot04 dot"></div><div class="dot05 dot"></div><div class="dot06 dot"></div><div class="dot07 dot"></div><div class="dot08 dot"></div><div class="dot09 dot"></div><div class="dot10 dot"></div><div class="dot11 dot"></div><div class="dot12 dot"></div><div class="dot13 dot"></div><div class="dot14 dot"></div><div class="dot15 dot"></div></div><div class="corner corner10BR"><div class="shape01 dot"></div><div class="shape02 dot"></div><div class="shape03 dot"></div><div class="dot01 dot"></div><div class="dot02 dot"></div><div class="dot03 dot"></div><div class="dot04 dot"></div><div class="dot05 dot"></div><div class="dot06 dot"></div><div class="dot07 dot"></div><div class="dot08 dot"></div><div class="dot09 dot"></div><div class="dot10 dot"></div><div class="dot11 dot"></div><div class="dot12 dot"></div><div class="dot13 dot"></div><div class="dot14 dot"></div><div class="dot15 dot"></div></div><div class="corner corner10BL"><div class="shape01 dot"></div><div class="shape02 dot"></div><div class="shape03 dot"></div><div class="dot01 dot"></div><div class="dot02 dot"></div><div class="dot03 dot"></div><div class="dot04 dot"></div><div class="dot05 dot"></div><div class="dot06 dot"></div><div class="dot07 dot"></div><div class="dot08 dot"></div><div class="dot09 dot"></div><div class="dot10 dot"></div><div class="dot11 dot"></div><div class="dot12 dot"></div><div class="dot13 dot"></div><div class="dot14 dot"></div><div class="dot15 dot"></div></div>')
};

function boxLink() {
    var thisBox, i, j, linkTitle, linkHref, linkTarget, linkStruct;
    thisBox = $('.single');
    j = thisBox.length;
    for (i = 0; i < j; i += 1) {
        linkTitle = null;
        linkHref = null;
        linkTarget = null;
        linkStruct = $('.single h3 > a');
        if (linkStruct.length > 0) {
            linkTitle = $(linkStruct[0]).text();
            linkHref = $(linkStruct[0]).attr('href');
            linkTarget = $(linkStruct[0]).attr('target');
            $(linkStruct[0]).remove
        } else {
            linkTitle = $(thisBox[i]).children('h3').children('a').text();
            linkHref = $(thisBox[i]).children('h3').children('a').attr('href');
            linkTarget = $(thisBox[i]).children('h3').children('a').attr('target');
            $(thisBox[i]).children('h3').children('a').remove
        }
        $(thisBox[i]).append("<a href='" + linkHref + "' target='" + linkTarget + "' />");
        $(thisBox[i]).children('h3').text(linkTitle);
        $(thisBox[i]).addClass("boxLink")
    };
    $(".single").click(function() {
        var linkHref, linkTarget;
        linkHref = $(this).children('a').attr('href');
        linkTarget = $(this).children('a').attr('target');
        if (linkTarget.length > 0) {
            window.open(linkHref, linkTarget);
            return false
        } else {
            window.location = linkHref;
            return false
        }
    });
    $(".single").hover(function() {
        $(this).addClass('singleHover')
    }, function() {
        $(this).removeClass('singleHover')
    })
};

function blockQuote() {
    $("blockquote").prepend('<p class="quote"><span>&ldquo;</span></p>');
    $("blockquote").append('<p class="quote"><span>&rdquo;</span></p>');
    if ($("blockquote cite").length > 0) {
        $("blockquote cite").before('<p class="quote"><span>&rdquo;</span></p>');
        $("blockquote cite + p").remove()
    }
};

function serviceLinks() {
    $("#serviceLink").click(function() {
        $("#serviceLinksBox").slideToggle("slow");
        $(this).toggleClass("active")
    });
    $(".slClose").click(function() {
        $("#serviceLinksBox").slideToggle("slow");
        $(this).toggleClass("active")
    })
};

function textLogo() {
    var altText = $("#header img").attr("alt");
    $("#header img").parent().parent().prepend('<div class="col fakeTitle" id="pageTitle">' + altText + '</div>')
};

function createCookie(name, value, days) {
    var date, expires;
    if (days) {
        date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString()
    } else {
        expires = ""
    }
    document.cookie = name + "=" + value + expires + "; path=/"
};

function readCookie(name) {
    var nameEQ, ca, i, c;
    nameEQ = name + "=";
    ca = document.cookie.split(';');
    for (i = 0; i < ca.length; i += 1) {
        c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1, c.length)
        }
        if (c.indexOf(nameEQ) == 0) {
            return c.substring(nameEQ.length, c.length)
        }
    }
    return null
};

function fsize(newSize, id) {
    var vfontsize = document.getElementById(id);
    if (vfontsize) {
        vfontsize.style.fontSize = newSize + "%"
    }
    createCookie("fontSize", newSize, 0)
};

function setCurrFsize(id) {
    var cookie, currentSize;
    cookie = readCookie("fontSize");
    currentSize = cookie || 100;
    fsize(currentSize, id)
};
var cookie = readCookie("fontSize");
var textsize = cookie || 100;

function changetextsize(up) {
    if (up) {
        textsize = parseFloat(textsize) + 15
    } else {
        textsize = parseFloat(textsize) - 15
    }
};

function bookmark(title, url) {

   if ((window.sidebar && /Firefox/i.test(navigator.userAgent)) || (window.opera && window.print)) {
	//alert('1er, ' + title +',' + url);
	 // Firefox version >= 23 and Opera Hotlist
      var myBookmark = $("#serviceLinksBox img[src*='iconSL-bookmark.gif']").parent();
	  
	  $(myBookmark).attr({
        href: url,
        title: title,
        rel: 'sidebar'
      });
	  setTimeout(function (){
		  $("#serviceLinksBox img[src*='iconSL-bookmark.gif']").parent().click();
		  }, 500);
	  
    } else if (window.opera && window.print) {
		
        alert('Please press <Ctrl+D> to bookmark this page (<Ctrl+T> in older Opera versions).')
    } else if (document.all) {
        window.external.AddFavorite(url, title)
		//alert('2do ');
    } else if (navigator.appName == 'Netscape') {
       alert('Please press <Ctrl+D> to bookmark this page.')
    }  
	/*else {
		alert('ELSE');
        alert('Bookmarks cannot be set in your browser automatically. Please use the browser menu to manually bookmark this page.')
    }*/
};

function  getSiteName(){
	var metas = document.getElementsByTagName('meta'); 

   for (i=0; i<metas.length; i++) { 
      if (metas[i].getAttribute("name") == "sitename") { 
         return metas[i].getAttribute("content"); 
      } 
   } 

    return "";
}

function showHideBox(it, box) {
    var vis = (box.checked) ? "block" : "none";
    document.getElementById(it).style.display = vis
};

function showHideLink(id) {
    var vis = (document.getElementById(id).style.display) == "none" ? "block" : "none";
    document.getElementById(id).style.display = vis
};

function GP_AdvOpenWindow(theURL, winName, features, popWidth, popHeight, winAlign, ignorelink, alwaysOnTop, borderless) {
    var w, h, topPos, leftPos;
    w = screen.availWidth;
    h = screen.availHeight;
    topPos = (h - popHeight) / 2;
    leftPos = (w - popWidth) / 2;
    features += ',width=' + String(popWidth) + ',height=' + String(popHeight) + ',top=' + String(topPos) + ',left=' + String(leftPos);
    window.open(theURL, winName, features);
    document.MM_returnValue = false
};
var offsetfromcursorX = 12;
var offsetfromcursorY = 10;
var offsetdivfrompointerX = 10;
var offsetdivfrompointerY = 14;
document.write('<div id="dhtmltooltip"></div>');
document.write('<img id="dhtmlpointer" src="images/layout/tooltiparrow.gif">');
var ie = document.all;
var ns6 = document.getElementById && !document.all;
var enabletip = false;
if (ie || ns6) {
    var tipobj = document.all ? document.all["dhtmltooltip"] : document.getElementById ? document.getElementById("dhtmltooltip") : ""
}
var pointerobj = document.all ? document.all["dhtmlpointer"] : document.getElementById ? document.getElementById("dhtmlpointer") : "";

function ietruebody() {
    return (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body
};

function ddrivetip(thetext, thewidth, thecolor) {
    if (ns6 || ie) {
        if (typeof thewidth != "undefined") {
            tipobj.style.width = thewidth + "px"
        }
        if (typeof thecolor != "undefined" && thecolor != "") {
            tipobj.style.backgroundColor = thecolor
        }
        tipobj.innerHTML = thetext;
        enabletip = true;
        return false
    }
};

function positiontip(e) {
    if (enabletip) {
        var nondefaultpos = false;
        var curX = (ns6) ? e.pageX : event.clientX + ietruebody().scrollLeft;
        var curY = (ns6) ? e.pageY : event.clientY + ietruebody().scrollTop;
        var winwidth = ie && !window.opera ? ietruebody().clientWidth : window.innerWidth - 20;
        var winheight = ie && !window.opera ? ietruebody().clientHeight : window.innerHeight - 20;
        var rightedge = ie && !window.opera ? winwidth - event.clientX - offsetfromcursorX : winwidth - e.clientX - offsetfromcursorX;
        var bottomedge = ie && !window.opera ? winheight - event.clientY - offsetfromcursorY : winheight - e.clientY - offsetfromcursorY;
        var leftedge = (offsetfromcursorX < 0) ? offsetfromcursorX * (-1) : -1000;
        if (rightedge < tipobj.offsetWidth) {
            tipobj.style.left = curX - tipobj.offsetWidth + "px";
            nondefaultpos = true
        } else if (curX < leftedge) {
            tipobj.style.left = "5px"
        } else {
            tipobj.style.left = curX + offsetfromcursorX - offsetdivfrompointerX + "px";
            pointerobj.style.left = curX + offsetfromcursorX + "px"
        }
        if (bottomedge < tipobj.offsetHeight) {
            tipobj.style.top = curY - tipobj.offsetHeight - offsetfromcursorY + "px";
            nondefaultpos = true
        } else {
            tipobj.style.top = curY + offsetfromcursorY + offsetdivfrompointerY + "px";
            pointerobj.style.top = curY + offsetfromcursorY + "px"
        }
        tipobj.style.visibility = "visible";
        if (!nondefaultpos) {
            pointerobj.style.visibility = "visible"
        } else {
            pointerobj.style.visibility = "hidden"
        }
    }
};

function hideddrivetip() {
    if (ns6 || ie) {
        enabletip = false;
        tipobj.style.visibility = "hidden";
        pointerobj.style.visibility = "hidden";
        tipobj.style.left = "-1000px";
        tipobj.style.backgroundColor = '';
        tipobj.style.width = ''
    }
};
document.onmousemove = positiontip;

function jump(key, value) {
    var myRegExp, searchString;
    searchString = location.search;
    if (key.length > 0) {
        myRegExp = new RegExp("&*" + key + "=\\w+", "gi");
        searchString = searchString.replace(myRegExp, "");
        if (searchString.length > 0) {
            if (searchString == '?') {
                searchString += key + '=' + value
            } else {
                searchString += '&' + key + '=' + value
            }
        } else {
            searchString += '?' + key + '=' + value
        }
    } else {
        searchString += value
    }
    window.location.href = location.protocol + '//' + location.host + location.pathname + searchString;
    return false
};