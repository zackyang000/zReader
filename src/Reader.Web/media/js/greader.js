var addFeed, auto_height, currentFeedUrl, errorHandler, generateContentList, generateFeed, getJsonFeed, refreshFeed, removeFeed, saveFavicon, showContent, showDetail, showFolderContent, start_folder, subscriptions, toggle, toggleAddBox, toggleShortcutMap, toggleStar;

subscriptions = JSON.parse(localStorage.getItem("subscriptions")) || [];

start_folder = "apple";

currentFeedUrl = "";

toggleAddBox = function() {
  var btnOffset, style;
  btnOffset = $(this).offset();
  style = {
    top: btnOffset.top + $(this).height(),
    left: btnOffset.left
  };
  $("#quick-add-bubble-holder").css(style).toggleClass('hidden');
  return $('#quickadd').val('').focus();
};

toggleStar = function(obj, item) {
  obj.find("div.entry-icons div").toggleClass("item-star");
  obj.find("div.entry-icons div").toggleClass("item-star-active");
  obj.find("div.entry-actions span:first").toggleClass("item-star");
  return obj.find("div.entry-actions span:first").toggleClass("item-star-active");
};

showDetail = function(obj, item) {
  var content, date, desc, entry_actions, entry_container, link, title;
  obj.toggleClass("expanded");
  obj.addClass("read");
  if (obj.attr("id") !== "current-entry") {
    if ($("#current-entry").hasClass("expanded")) {
      $("#current-entry").find("div:first").click();
    }
    $("#current-entry").attr("id", "");
    obj.attr("id", "current-entry");
  }
  if (obj.find(".entry-container").length > 0) {
    obj.find(".entry-container").remove();
    obj.find(".entry-actions").remove();
    return;
  }
  date = item.publishedDate;
  link = item.link;
  title = item.title;
  desc = item.contentSnippet;
  content = item.content;
  entry_container = $(sprintf('<div class="entry-container"><div class="entry-main"><div class="entry-date">%s</div><h2 class="entry-title"><a class="entry-title-link" target="_blank" href="%s">%s<div class="entry-title-go-to"></div></a><span class="entry-icons-placeholder"></span></h2><div class="entry-author"><span class="entry-source-title-parent">来源：<a class="entry-source-title" target="_blank" href=""></a></span> </div><div class="entry-debug"></div><div class="entry-annotations"></div><div class="entry-body"><div><div class="item-body"><div>%s</div></div></div></div></div></div>', date, link, title, content));
  entry_actions = $('<div class="entry-actions"><span class="item-star star link unselectable" title="加注星标"></span><wbr><span class="item-plusone" style="height: 15px; width: 70px; display: inline-block; text-indent: 0px; margin: 0px; padding: 0px; background-color: transparent; border-style: none; float: none; line-height: normal; font-size: 1px; vertical-align: baseline; background-position: initial initial; background-repeat: initial initial;"><iframe frameborder="0" hspace="0" marginheight="0" marginwidth="0" scrolling="no" style="position: absolute; top: -10000px; width: 70px; margin: 0px; border-style: none;" tabindex="0" vspace="0" width="100%" id="I6_1364822093465" name="I6_1364822093465" allowtransparency="true" data-gapiattached="true"></iframe></span><wbr><span class="email"><span class="link unselectable">电子邮件</span></span><wbr><span class="read-state-not-kept-unread read-state link unselectable" title="保持为未读状态">保持为未读状态</span><wbr><span></span><wbr><span class="tag link unselectable"><span class="entry-tagging-action-title">修改标签: </span><ul class="user-tags-list"><li><a href="/reader/view/user%2F-%2Flabel%2FIT.%E6%95%B0%E7%A0%81">IT.数码</a></li></ul></span></div>');
  obj.append(entry_container);
  return obj.append(entry_actions);
};

showContent = function(feedUrl) {
  var feed;
  feed = JSON.parse(localStorage.getItem(feedUrl));
  $("#entries").addClass("single-source");
  $("#entries").find(".entry").remove();
  $("#title-and-status-holder").css("display", "block");
  $("#chrome-title").html(sprintf('<a target="_blank" href="%s">%s<span class="chevron">»</span></a>', feed.link, feed.title));
  $("#chrome-view-links").css("display", "block");
  currentFeedUrl = feedUrl;
  return generateContentList(feed.entries);
};

showFolderContent = function(dict) {
  var entries, item, _i, _len, _ref, _results;
  $("#entries").removeClass("single-source");
  $("#entries").find(".entry").remove();
  $("#title-and-status-holder").css("display", "block");
  $("#chrome-title").html(dict.title);
  $("#chrome-view-links").css("display", "block");
  _ref = dict.item;
  _results = [];
  for (_i = 0, _len = _ref.length; _i < _len; _i++) {
    item = _ref[_i];
    if (localStorage.getItem(item.feedUrl) === null) {
      _results.push(refreshFeed(item.feedUrl, function(feed, faviconUrl) {
        return generateContentList(feed.entries);
      }));
    } else {
      entries = JSON.parse(localStorage.getItem(item.feedUrl)).entries;
      _results.push(generateContentList(entries));
    }
  }
  return _results;
};

toggleShortcutMap = function() {
  var content, div, div2;
  div = $('<div class="banner banner-background keyboard-help-banner"></div>');
  div2 = $('<div class="banner banner-foreground keyboard-help-banner"></div>');
  content = $('<div class="primary-message">键盘快捷键</div>\n<div class="secondary-message">\n<div id="keyboard-help-container">\n<table id="keyboard-help"><tbody><tr><td class="help-section"><table><tbody><tr><th colspan="2">浏览</th></tr>\n<tr><td class="key">j/k：</td>\n<td class="desc">下一个/上一个条目</td></tr>\n<tr><td class="key">空格：</td>\n<td class="desc">下一个条目或页面</td></tr>\n<tr><td class="key">&lt;Shift&gt; + 空格：</td>\n<td class="desc">上一个条目或页面</td></tr>\n<tr><td class="key">n/p：</td>\n<td class="desc">向下/向上扫描条目（仅列表）</td></tr>\n<tr><td class="key">&lt;Shift&gt; + n/p：</td>\n<td class="desc">下一个/上一个订阅</td></tr>\n<tr><td class="key">&lt;Shift&gt; + x：</td>\n<td class="desc">展开文件夹</td></tr>\n<tr><td class="key">&lt;Shift&gt; + o：</td>\n<td class="desc">打开订阅或文件夹</td></tr></tbody></table></td>\n<td class="help-section"><table><tbody><tr><th colspan="2">应用</th></tr>\n<tr><td class="key">r：</td>\n<td class="desc">刷新</td></tr>\n<tr><td class="key">f：</td>\n<td class="desc">切换至全屏模式</td></tr>\n<tr><td class="key">u：</td>\n<td class="desc">隐藏/取消隐藏左侧模块</td></tr>\n<tr><td class="key">1:</td>\n<td class="desc">切换至扩展视图</td></tr>\n<tr><td class="key">2:</td>\n<td class="desc">切换至列表视图</td></tr>\n<tr><td class="key">/:</td>\n<td class="desc">将光标移动到搜索框</td></tr>\n<tr><td class="key">a:</td>\n<td class="desc">添加订阅</td></tr>\n<tr><td class="key">=:</td>\n<td class="desc">提高放大倍数</td></tr>\n<tr><td class="key">-:</td>\n<td class="desc">降低放大倍数</td></tr></tbody></table></td></tr>\n<tr><td class="help-section"><table><tbody><tr><th colspan="2">跳转</th></tr>\n<tr><td class="key">g 然后 h：</td>\n<td class="desc">转到主页</td></tr>\n<tr><td class="key">g 然后 a：</td>\n<td class="desc">转到所有条目</td></tr>\n<tr><td class="key">g 然后 s：</td>\n<td class="desc">转到加星标条目</td></tr>\n<tr><td class="key">g 然后 u：</td>\n<td class="desc">打开订阅选择器</td></tr>\n<tr><td class="key">g 然后 t：</td>\n<td class="desc">打开标签选择器</td></tr>\n<tr><td class="key">g 然后 &lt;Shift&gt; + t：</td>\n<td class="desc">转到趋势页</td></tr>\n<tr><td class="key">g 然后 d：</td>\n<td class="desc">转到查找页</td></tr>\n<tr><td class="key">依次按 g 和 e：</td>\n<td class="desc">开始探索</td></tr>\n<tr><td class="key">依次按 g 和 p：</td>\n<td class="desc">转到热门条目</td></tr></tbody></table></td>\n<td class="help-section"><table><tbody><tr><th colspan="2">对条目采取行动</th></tr>\n<tr><td class="key">s：</td>\n<td class="desc">为条目加注星标</td></tr>\n<tr><td class="key">t：</td>\n<td class="desc">标记条目</td></tr>\n<tr><td class="key">e：</td>\n<td class="desc">通过电子邮件发送条目</td></tr>\n<tr><td class="key">&lt;Shift&gt; + s：</td>\n<td class="desc">共享条目</td></tr>\n<tr><td class="key">v：</td>\n<td class="desc">查看原始内容</td></tr>\n<tr><td class="key">或<enter>键：</enter></td>\n<td class="desc">展开/折叠条目（仅限列表）</td></tr>\n<tr><td class="key">m：</td>\n<td class="desc">将条目标为已读/未读</td></tr>\n<tr><td class="key">&lt;Shift&gt; + a：</td>\n<td class="desc">全部标为已读</td></tr>\n<tr><td class="key">&lt;Shift&gt; + t：</td>\n<td class="desc">打开“发送到”菜单</td></tr></tbody></table></td></tr></tbody></table>\n<div id="keyboard-help-tearoff-link-container"><span class="link keyboard-help-tearoff-link open-new-window-link">在新窗口中打开</span>\n-\n<span class="link keyboard-help-tearoff-link close-help-link">关闭</span>\n</div>\n</div>\n</div>');
  div.append(content);
  div2.append(content);
  $('body').append(div);
  return $('body').append(div2);
};

generateContentList = function(entries) {
  var a, date, desc, div, dt, i, item, link, stitle, title, _i, _len, _results;
  i = 0;
  _results = [];
  for (_i = 0, _len = entries.length; _i < _len; _i++) {
    item = entries[_i];
    dt = new Date(item.publishedDate);
    date = dt.toLocaleTimeString();
    link = item.link;
    stitle = item.stitle;
    title = item.title;
    desc = item.contentSnippet;
    $("#viewer-header-container").css("display", "block");
    $("#viewer-entries-container").css("display", "block");
    $("#viewer-page-container").css("display", "none");
    div = $(sprintf('<div class="entry entry-%s ril_marked"><div class="collapsed"><div class="entry-icons"><div class="item-star star link unselectable empty"></div></div><div class="entry-date">%s</div><div class="entry-main"><a class="entry-original" target="_blank" href="%s"></a><span class="entry-source-title">%s</span><div class="entry-secondary"><h2 class="entry-title">%s</h2><span class="entry-secondary-snippet"> - <span class="snippet">%s</span></span></div></div></div></div>', i, date, link, stitle, title, desc));
    i += 1;
    a = function(obj, args) {
      obj.find(".collapsed").click(function() {
        return showDetail(obj, args);
      });
      obj.find("div.entry-icons").click(function(e) {
        toggleStar(obj, args);
        return e.stopPropagation();
      });
      obj.find("div.entry-actions span:first").on("click", function(e) {
        return toggleStar(obj, args);
      });
      return obj.find("div.entry-actions span.read-state").on("click", function(e) {
        $(this).toggleClass("read-state-not-kept-unread");
        $(this).toggleClass("read-state-kept-unread");
        return obj.toggleClass("read");
      });
    };
    a(div, item);
    _results.push($("#entries").append(div));
  }
  return _results;
};

errorHandler = function(e) {
  var msg;
  msg = "";
  switch (e.code) {
    case FileError.QUOTA_EXCEEDED_ERR:
      msg = 'QUOTA_EXCEEDED_ERR';
      break;
    case FileError.NOT_FOUND_ERR:
      msg = 'NOT_FOUND_ERR';
      break;
    case FileError.SECURITY_ERR:
      msg = 'SECURITY_ERR';
      break;
    case FileError.INVALID_MODIFICATION_ERR:
      msg = 'INVALID_MODIFICATION_ERR';
      break;
    case FileError.INVALID_STATE_ERR:
      msg = 'INVALID_STATE_ERR';
      break;
    default:
      msg = 'Unknown Error';
      break;
  }
  return alert(msg);
};

addFeed = function() {
  var url;
  url = $("#quickadd").val();
  if (url.indexOf("http://") !== 0) {
    alert("invalid feed url");
    return;
  }
  if (localStorage.getItem(url)) {
    alert("You have subscribed to " + url);
    return;
  }
  return refreshFeed(url, function(feed, faviconUrl) {
    var f, li;
    f = {
      title: feed.title,
      type: "rss",
      feedUrl: feed.feedUrl,
      favicon: faviconUrl
    };
    li = generateFeed(f);
    $("#sub-tree-item-0-main ul:first").append(li);
    $("#quick-add-bubble-holder").toggleClass("show");
    $("#quick-add-bubble-holder").toggleClass("hidden");
    subscriptions.push(f);
    return localStorage.setItem("subscriptions", JSON.stringify(subscriptions));
  });
};

saveFavicon = function(faviconUrl, domainName, cb) {
  var xhr;
  xhr = new XMLHttpRequest();
  xhr.open('GET', faviconUrl, true);
  xhr.responseType = 'blob';
  xhr.onerror = function() {
    return cb("img/default.gif");
  };
  xhr.onreadystatechange = function(e) {
    if (this.readyState < 4) {
      return $("#loading-area-container").removeClass("hidden");
    } else {
      return $("#loading-area-container").addClass("hidden");
    }
  };
  xhr.onload = function(e) {
    $("#loading-area-container").addClass("hidden");
    if (this.status !== 200 || xhr.response.size === 0) {
      return saveFavicon("/media/img/default.gif", domainName, cb);
    } else {
      return fs.root.getFile(domainName + ".ico", {
        create: true
      }, function(fileEntry) {
        return fileEntry.createWriter(function(fileWriter) {
          fileWriter.onwriteend = function(e) {
            return cb(fileEntry.toURL());
          };
          fileWriter.onerror = function(e) {
            return console.log('Write failed:' + e.toString());
          };
          return fileWriter.write(xhr.response);
        }, errorHandler);
      }, errorHandler);
    }
  };
  return xhr.send();
};

getJsonFeed = function(url, cb) {
  $("#loading-area-container").removeClass("hidden");
  return $.ajax({
    url: 'https://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=50&callback=?&q=' + encodeURIComponent(url),
    dataType: 'json',
    success: function(data) {
      var feed, item, _i, _len, _ref;
      feed = data.responseData.feed;
      _ref = feed.entries;
      for (_i = 0, _len = _ref.length; _i < _len; _i++) {
        item = _ref[_i];
        item.stitle = feed.title;
      }
      return cb(feed);
    },
    complete: function() {
      return $("#loading-area-container").addClass("hidden");
    }
  });
};

generateFeed = function(feed) {
  var li;
  li = $(sprintf('<li class="sub unselectable expanded unread">\n<div class="toggle sub-toggle toggle-d-2 hidden"></div>\n<a class="link" title="%s">\n <div style="background-image: url(%s); background-size:16px 16px" class="icon sub-icon icon-d-2 favicon">\n </div>\n <div class="name-text sub-name-text name-text-d-2 name sub-name name-d-2 name-unread">%s</div>\n <div class="unread-count sub-unread-count unread-count-d-2"></div>\n <div class="tree-item-action-container">\n <div class="action tree-item-action section-button section-menubutton goog-menu-button"></div>\n </div>\n </a>\n </li>', feed.feedUrl, feed.favicon, feed.title));
  li.find("a:first").click(function() {
    if (localStorage.getItem(feed.feedUrl) === null) {
      return refreshFeed(feed.feedUrl, function(feed, faviconUrl) {
        return showContent(feed.feedUrl);
      });
    } else {
      return showContent(feed.feedUrl);
    }
  });
  return li;
};

toggle = function(obj) {
  obj.toggleClass("collapsed");
  return obj.toggleClass("expanded");
};

removeFeed = function() {
  var feed, _i, _len;
  localStorage.removeItem(currentFeedUrl);
  for (_i = 0, _len = subscriptions.length; _i < _len; _i++) {
    feed = subscriptions[_i];
    if (feed.feedUrl === currentFeedUrl) {
      subscriptions.splice(subscriptions.indexOf(feed), 1);
      localStorage.setItem("subscriptions", JSON.stringify(subscriptions));
      $("#sub-tree-item-0-main ul:first li a[title='" + currentFeedUrl + "']").parent().remove();
      $("#stream-prefs-menu").click();
      return;
    }
  }
};

refreshFeed = function(feedUrl, cb) {
  return getJsonFeed(feedUrl, function(feed) {
    var domainName, url;
    localStorage.setItem(feedUrl, JSON.stringify(feed));
    domainName = feed.link.split("/")[2];
    url = "http://" + domainName + "/favicon.ico";
    return saveFavicon(url, domainName, function(faviconUrl) {
      return cb(feed, faviconUrl);
    });
  });
};

auto_height = function() {
  var $section, $viewer;
  $section = $('#scrollable-sections');
  $section.css({
    height: $(window).height() - $section.offset().top - 10
  });
  $viewer = $('#viewer-entries-container');
  return $viewer.css({
    height: $(window).height() - $viewer.offset().top - 10
  });
};

(function($) {
  var f, feed_ul, item, _i, _len;
  $("#lhn-add-subscription").on('click', toggleAddBox);
  $('#quick-add-close').on('click', toggleAddBox);
  $("#add-feed").on('click', addFeed);
  $(".folder-toggle").click(function() {
    return toggle($(this).parent());
  });
  $("#viewer-refresh").click(function() {
    return refreshFeed(currentFeedUrl, function(feed, favcionUrl) {
      return showContent(feed.feedUrl);
    });
  });
  $("#lhn-selectors-minimize").click(function() {
    return $("#lhn-selectors").toggleClass("section-minimized");
  });
  $("#lhn-recommendations-minimize").click(function() {
    return $("#lhn-recommendations").toggleClass("section-minimized");
  });
  $("#lhn-subscriptions-minimize").click(function() {
    return $("#lhn-subscriptions").toggleClass("section-minimized");
  });
  $('#settings-button-container').on('click', function() {
    return $('#settings-button-menu').toggle();
  });
  $("#settings-button-menu .goog-menuitem-settings").on('click', showSettingsPage);
  $('#quickadd').bind('keypress', function(e) {
    var code;
    code = e.keyCode ? e.keyCode : e.which;
    console.log(code);
    if (code === 13) {
      return $("#add-feed").click();
    }
  });
  setInterval(auto_height, 200);
  $("div[role=button]").hover(function() {
    return $(this).toggleClass("jfk-button-hover");
  });
  $("div[role=listbox]").hover(function() {
    return $(this).toggleClass("goog-flat-menu-button-hover");
  });
  $("#entries-up").on("click", function() {
    return $("#current-entry").prev().find(".collapsed").click();
  });
  $("#entries-down").on("click", function() {
    return $("#current-entry").next().find(".collapsed").click();
  });
  window.requestFileSystem = window.requestFileSystem || window.webkitRequestFileSystem;
  if (window.requestFileSystem) {
    window.requestFileSystem(window.TEMPORARY, 100 * 1024 * 1024, function(filesystem) {
      var fs;
      return fs = filesystem;
    }, errorHandler);
  }
  feed_ul = $("#sub-tree-item-0-main ul:first");
  for (_i = 0, _len = subscriptions.length; _i < _len; _i++) {
    item = subscriptions[_i];
    if (item.type === "rss" && (item.categories === void 0 || item.categories.length === 0)) {
      feed_ul.append(generateFeed(item));
    }
    if (item.type === "folder") {
      f = generateFolder(item);
      feed_ul.append(f);
      if (item.title === start_folder) {
        f.find("a:first").click();
      }
    }
  }
  return $("body").bind('keypress', function(e) {
    var code;
    code = e.keyCode ? e.keyCode : e.which;
    if (code === 106) {
      $("#current-entry").next().find(".collapsed").click();
    }
    if (code === 107) {
      $("#current-entry").prev().find(".collapsed").click();
    }
    if (code === 13) {
      $("#current-entry").removeClass("expanded");
      $("#current-entry").find(".entry-container").remove();
      $("#current-entry").find(".entry-actions").remove();
    }
    if (code === 102) {
      $("body").toggleClass("fullscreen");
      $("body").toggleClass("lhn-hidden");
    }
    if (code === 63) {
      return toggleShortcutMap();
    }
  });
})(jQuery);
